using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace GithubStalker
{

    internal static class Program
    {

        [STAThread]
        static void Main()
        {
            new GithubStalker().Run();
        }

    }

    public class GithubStalker
    {

        private readonly Dictionary<String, Status> users = new();
        private readonly Dictionary<String, Status> repositories = new();
        private readonly List<String> skippedRepositories = new();

        private readonly List<String> removedRepositories = new();
        private readonly List<String> searchedUsers = new();

        private readonly MainWindowForm mainWindow;

        public GithubStalker()
        {
            ApplicationConfiguration.Initialize();
            mainWindow = new MainWindowForm(this);
        }

        internal class SaveData
        {
            public readonly bool automaticallyAddRepositories;
            public readonly string[] users;
            public readonly string[] repositories;
            public readonly string[] skippedRepositories;

            public SaveData(bool automaticallyAddRepositories, string[] users, string[] repositories, string[] skippedRepositories)
            {
                this.automaticallyAddRepositories = automaticallyAddRepositories;
                this.users = users;
                this.repositories = repositories;
                this.skippedRepositories = skippedRepositories;
            }
        }

        public void Run()
        {
            Console.SetOut(new TextBoxWriter(mainWindow.outputTextbox));

            Load();
            Application.Run(mainWindow);
            Save();
        }

        private void Load()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string applicationFolder = Path.Combine(appdata, "github-stalker");
            string filePath = Path.Combine(applicationFolder, "saved.json");

            if (File.Exists(filePath))
            {
                string jsonText = File.ReadAllText(filePath);
                SaveData saveData = JsonConvert.DeserializeObject<SaveData>(jsonText);

                mainWindow.automaticallyAddRepositories.Checked = saveData.automaticallyAddRepositories;
                foreach (string user in saveData.users)
                {
                    users.Add(user, Status.Waiting);
                }
                foreach (string repository in saveData.repositories)
                {
                    repositories.Add(repository, Status.Waiting);
                }
                foreach (string repository in saveData.skippedRepositories)
                {
                    skippedRepositories.Add(repository);
                }
            }
        }

        private void Save()
        {
            string jsonString = JsonConvert.SerializeObject(new SaveData(mainWindow.automaticallyAddRepositories.Checked, 
                users.Keys.ToArray(),
                repositories.Keys.ToArray(),
                skippedRepositories.ToArray()), Formatting.Indented);

            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string applicationFolder = Path.Combine(appdata, "github-stalker");
            string filePath = Path.Combine(applicationFolder, "saved.json");

            using (StreamWriter outputFile = new StreamWriter(filePath))
            {
                outputFile.Write(jsonString);
            }
        }

        public void CreateWorkerThread()
        {
            // Ui Update thread
            _ = Task.Run(() =>
            {
                while (mainWindow != null)
                {
                    try
                    {
                        mainWindow.usersLabel.Invoke((Action)delegate
                        {
                            string usersText = String.Empty;
                            foreach (string username in users.Keys)
                            {

                                usersText += username + ": " + Enum.GetName(users[username]) + "\n";
                            }
                            mainWindow.usersLabel.Text = usersText;
                        });

                        mainWindow.repositoriesLabel.Invoke((Action)delegate
                        {
                            string repositoryText = String.Empty;
                            foreach (string repository in repositories.Keys)
                            {
                                repositoryText += repository + ": " + Enum.GetName(repositories[repository]) + "\n";
                            }
                            mainWindow.repositoriesLabel.Text = repositoryText;
                        });

                        mainWindow.skippingLabel.Invoke((Action)delegate
                        {
                            string skipLabelText = String.Empty;

                            foreach (String skipped in skippedRepositories)
                            {
                                skipLabelText += skipped + "\n";
                            }

                            mainWindow.skippingLabel.Text = skipLabelText;
                        });
                    }
                    catch (Exception ex)
                    {
                        if (!(ex is InvalidOperationException || ex is KeyNotFoundException))
                            Console.WriteLine(ex.Message);
                    }
                }
            });

            // User search thread
            _ = Task.Run(() =>
            {
                while(mainWindow != null)
                {
                    try
                    {
                        foreach (string username in users.Keys)
                        {
                            if (users[username] == Status.Waiting)
                            {
                                users[username] = Status.Searching;

                                string userUrl = $"https://github.com/{username}";
                                Console.WriteLine($"Contacting {userUrl}");
                                
                                String userPageSource = Util.Get_HTML(userUrl);
                                String title = Util.GetTagInnerText(userPageSource, "title");
                                Console.WriteLine($"Found title in {userUrl}: {title}");
                                
                                if (title.Contains("not found"))
                                {
                                    users[username] = Status.Bad;
                                }
                                else
                                {
                                    users[username] = Status.Good;
                                }
                            }
                        }
                    } catch (Exception ex) {
                        if (!(ex is InvalidOperationException || ex is KeyNotFoundException))
                            Console.WriteLine(ex.Message);
                    }
                }
            });

            // Repository search thread
            _ = Task.Run(() =>
            {
                while (mainWindow != null)
                {
                    try
                    {
                        if(mainWindow.automaticallyAddRepositories.Checked)
                        {
                            foreach (string username in users.Keys)
                            {
                                if (!searchedUsers.Contains(username) && users[username] == Status.Good)
                                {
                                    searchedUsers.Add(username);

                                    string repositoriesUrl = $"https://github.com/{username}?tab=repositories";

                                    Console.WriteLine($"Contacting {repositoriesUrl}");
                                    string source = Util.Get_HTML(repositoriesUrl);

                                    List<String> foundHrefs = Util.GetTagAttribute(source, "href");
                                    Console.WriteLine($"Found {foundHrefs.Count} hrefs in {repositoriesUrl}");

                                    string repositoryURL = $"href=\"/{username}/";

                                    foreach (String href in foundHrefs)
                                    {
                                        if (href.StartsWith(repositoryURL) && !href.Contains("/stargazers") && !href.Contains("/forks"))
                                        {
                                            string repository = href.Substring(6, href.Length - 7);
                                            if (!repositories.ContainsKey(repository) && !removedRepositories.Contains(repository))
                                            {
                                                repositories.Add(repository, Status.Waiting);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!(ex is InvalidOperationException || ex is KeyNotFoundException))
                            Console.WriteLine(ex.Message);
                    }
                }
            });

            // Downloading repositories
            _ = Task.Run(() =>
            {
                while (mainWindow != null)
                {
                    try
                    {
                        foreach (string repository in repositories.Keys)
                        {
                            if (!skippedRepositories.Contains(repository))
                            {
                                Console.WriteLine($"Attempting to find latest commit id of {repository}");
                                string repositoryUrl = $"https://github.com{repository}.git";
                                string latestCommit = Util.RunCommand($"git ls-remote {repositoryUrl} HEAD").Substring(0, 40);

                                string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                                string applicationFolder = Path.Combine(appdata, "github-stalker");
                                string userFolder = Path.Combine(applicationFolder, repository.Split("/")[1]);
                                string repositoryFolder = Path.Combine(userFolder, repository.Split("/")[2]);
                                string commitFolder = Path.Combine(repositoryFolder, latestCommit);

                                if (!Directory.Exists(commitFolder))
                                {
                                    repositories[repository] = Status.Downloading;
                                    if (!Directory.Exists(repositoryFolder)) {
                                        Directory.CreateDirectory(repositoryFolder);
                                    }

                                    Console.WriteLine($"Clonning {repository}");
                                    Util.RunCommandWithOutput($"git clone https://github.com{repository} {commitFolder}");
                                }
                                repositories[repository] = Status.Good;
                            }
                            else
                            {
                                repositories[repository] = Status.Skipped;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!(ex is InvalidOperationException || ex is KeyNotFoundException))
                            Console.WriteLine(ex.Message);
                    }
                }
            });
        }

        public void RunActionThread(UserAction action)
        {
            Task.Run(() => { OnAction(action); });
        }

        private void OnAction(UserAction action)
        {
            InputWindow inputWindow = new();
            Application.Run(inputWindow);

            string? text = inputWindow.InputText;
                        
            if (text == null || text.Equals(string.Empty))
                return;

            switch (action)
            {
                case UserAction.AddUser:
                    if(!users.ContainsKey(text))
                        users.Add(text, Status.Waiting);
                    break;
                case UserAction.RemoveUser:
                    if(users.ContainsKey(text))
                        users.Remove(text);
                    break;
                case UserAction.AddRepository:
                    if (!repositories.ContainsKey(text))
                    {
                        repositories.Add(text, Status.Waiting);
                        if(removedRepositories.Contains(text))
                            removedRepositories.Remove(text);
                    }
                    break;
                case UserAction.RemoveRepository:
                    if (repositories.ContainsKey(text))
                    {
                        repositories.Remove(text);
                        removedRepositories.Add(text);
                    }
                    break;
                case UserAction.AddSkippedRepositroy:
                    skippedRepositories.Add(text);
                    break;
                case UserAction.RemoveSkippedRepository:
                    if (skippedRepositories.Contains(text))
                        skippedRepositories.Remove(text);
                    break;
            }
        }

        public enum Status
        {
            Good, Bad, Downloading, Searching, Skipped, Waiting
        }

        public enum UserAction
        {
            AddUser, RemoveUser, AddRepository, RemoveRepository, AddSkippedRepositroy, RemoveSkippedRepository
        }
    }

    public static class Util
    {
        public static string Get_HTML(string Url)
        {
            System.Net.WebResponse Result = null;
            string Page_Source_Code;
            try
            {
                System.Net.WebRequest req = System.Net.WebRequest.Create(Url);
                Result = req.GetResponse();
                System.IO.Stream RStream = Result.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(RStream);
                new System.IO.StreamReader(RStream);
                Page_Source_Code = sr.ReadToEnd();
                sr.Dispose();
            }
            catch
            {
                // error while reading the url: the url dosen’t exist, connection problem...
                Page_Source_Code = "";
            }
            finally
            {
                if (Result != null) Result.Close();
            }
            return Page_Source_Code;
        }

        public static string GetTagInnerText(string html, string tagName)
        {
            string pattern = $"<{tagName}.*?>(?<InnerText>.*?)</{tagName}>";
            Regex regex = new Regex(pattern, RegexOptions.Singleline);

            Match match = regex.Match(html);

            if (match.Success)
            {
                return match.Groups["InnerText"].Value.Trim();
            }
            else
            {
                return "Tag not found or empty";
            }
        }

        public static List<string> GetTagAttribute(string text, string attribute)
        {
            List<string> found = new List<string>();
            string attributeRegex = $"{attribute}=\"([^\"]*)\"";
            Regex regex = new Regex(attributeRegex, RegexOptions.IgnoreCase);
            Match attributeMatcher = regex.Match(text);

            while (attributeMatcher.Success)
            {
                found.Add(attributeMatcher.Groups[0].Value);
                attributeMatcher = attributeMatcher.NextMatch();
            }

            return found;
        }

        public static string RunCommand(string command)
        {
            string output = "";

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = "/c " + command
            };

            using (Process process = new Process { StartInfo = processStartInfo })
            {
                process.Start();

                // Read the output of the command
                using (StreamReader reader = process.StandardOutput)
                {
                    output = reader.ReadToEnd();
                }

                process.WaitForExit();
            }

            return output;
        }

        public static void RunCommandWithOutput(string command)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = "/c " + command
            };

            using (Process process = new Process { StartInfo = processStartInfo })
            {
                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

                process.Start();

                // Begin asynchronous reading of the output and error streams
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();
            }
        }
    }

    public class TextBoxWriter : TextWriter
    {
        TextBox _output = null;

        public TextBoxWriter(TextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            _output.Invoke((Action)delegate
            {
                _output.AppendText(value.ToString() + "\n");
            });
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}