namespace GithubStalker
{
    public partial class MainWindowForm : Form
    {

        private readonly GithubStalker program;

        public MainWindowForm(GithubStalker program)
        {
            InitializeComponent();
            this.program = program;

            this.HandleCreated += new EventHandler((sender, args) =>
            {
                program.CreateWorkerThread();
            });
        }

        private void addUser_Click(object sender, EventArgs e)
        {
            program.RunActionThread(GithubStalker.UserAction.AddUser);
        }

        private void removeUser_Click(object sender, EventArgs e)
        {
            program.RunActionThread(GithubStalker.UserAction.RemoveUser);
        }

        private void addRepository_Click(object sender, EventArgs e)
        {
            program.RunActionThread(GithubStalker.UserAction.AddRepository);
        }

        private void removeRepository_Click(object sender, EventArgs e)
        {
            program.RunActionThread(GithubStalker.UserAction.RemoveRepository);
        }

        private void addSkippedRepo_Click(object sender, EventArgs e)
        {
            program.RunActionThread(GithubStalker.UserAction.AddSkippedRepositroy);
        }

        private void removeSkippedRepo_Click(object sender, EventArgs e)
        {
            program.RunActionThread(GithubStalker.UserAction.RemoveSkippedRepository);
        }

        private void MainWindowForm_Load(object sender, EventArgs e)
        {

        }
    }
}
