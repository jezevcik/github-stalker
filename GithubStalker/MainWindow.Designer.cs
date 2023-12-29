namespace GithubStalker
{
    partial class MainWindowForm
    {

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            users = new GroupBox();
            usersLabel = new Label();
            removeUser = new Label();
            addUser = new Label();
            repositories = new GroupBox();
            repositoriesLabel = new Label();
            removeRepository = new Label();
            addRepository = new Label();
            skipping = new GroupBox();
            skippingLabel = new Label();
            removeSkippedRepo = new Label();
            addSkippedRepo = new Label();
            consoleOutput = new GroupBox();
            outputTextbox = new TextBox();
            automaticallyAddRepositories = new CheckBox();
            users.SuspendLayout();
            repositories.SuspendLayout();
            skipping.SuspendLayout();
            consoleOutput.SuspendLayout();
            SuspendLayout();
            // 
            // users
            // 
            users.Controls.Add(usersLabel);
            users.Controls.Add(removeUser);
            users.Controls.Add(addUser);
            users.Location = new Point(12, 12);
            users.Name = "users";
            users.Size = new Size(296, 351);
            users.TabIndex = 0;
            users.TabStop = false;
            users.Text = "Users";
            // 
            // usersLabel
            // 
            usersLabel.AutoSize = true;
            usersLabel.Location = new Point(6, 19);
            usersLabel.Name = "usersLabel";
            usersLabel.Size = new Size(0, 15);
            usersLabel.TabIndex = 3;
            // 
            // removeUser
            // 
            removeUser.AutoSize = true;
            removeUser.Location = new Point(279, -3);
            removeUser.Name = "removeUser";
            removeUser.Size = new Size(12, 15);
            removeUser.TabIndex = 2;
            removeUser.Text = "-";
            removeUser.Click += removeUser_Click;
            // 
            // addUser
            // 
            addUser.AutoSize = true;
            addUser.Location = new Point(258, -3);
            addUser.Name = "addUser";
            addUser.Size = new Size(15, 15);
            addUser.TabIndex = 1;
            addUser.Text = "+";
            addUser.Click += addUser_Click;
            // 
            // repositories
            // 
            repositories.Controls.Add(repositoriesLabel);
            repositories.Controls.Add(removeRepository);
            repositories.Controls.Add(addRepository);
            repositories.Location = new Point(314, 12);
            repositories.Name = "repositories";
            repositories.Size = new Size(296, 351);
            repositories.TabIndex = 1;
            repositories.TabStop = false;
            repositories.Text = "Repositories";
            // 
            // repositoriesLabel
            // 
            repositoriesLabel.AutoSize = true;
            repositoriesLabel.Location = new Point(6, 19);
            repositoriesLabel.Name = "repositoriesLabel";
            repositoriesLabel.Size = new Size(0, 15);
            repositoriesLabel.TabIndex = 4;
            // 
            // removeRepository
            // 
            removeRepository.AutoSize = true;
            removeRepository.Location = new Point(278, -3);
            removeRepository.Name = "removeRepository";
            removeRepository.Size = new Size(12, 15);
            removeRepository.TabIndex = 3;
            removeRepository.Text = "-";
            removeRepository.Click += removeRepository_Click;
            // 
            // addRepository
            // 
            addRepository.AutoSize = true;
            addRepository.Location = new Point(257, -3);
            addRepository.Name = "addRepository";
            addRepository.Size = new Size(15, 15);
            addRepository.TabIndex = 3;
            addRepository.Text = "+";
            addRepository.Click += addRepository_Click;
            // 
            // skipping
            // 
            skipping.Controls.Add(skippingLabel);
            skipping.Controls.Add(removeSkippedRepo);
            skipping.Controls.Add(addSkippedRepo);
            skipping.Location = new Point(616, 12);
            skipping.Name = "skipping";
            skipping.Size = new Size(296, 351);
            skipping.TabIndex = 2;
            skipping.TabStop = false;
            skipping.Text = "Skipping Repositories";
            // 
            // skippingLabel
            // 
            skippingLabel.AutoSize = true;
            skippingLabel.Location = new Point(6, 19);
            skippingLabel.Name = "skippingLabel";
            skippingLabel.Size = new Size(0, 15);
            skippingLabel.TabIndex = 4;
            // 
            // removeSkippedRepo
            // 
            removeSkippedRepo.AutoSize = true;
            removeSkippedRepo.Location = new Point(278, -3);
            removeSkippedRepo.Name = "removeSkippedRepo";
            removeSkippedRepo.Size = new Size(12, 15);
            removeSkippedRepo.TabIndex = 3;
            removeSkippedRepo.Text = "-";
            removeSkippedRepo.Click += removeSkippedRepo_Click;
            // 
            // addSkippedRepo
            // 
            addSkippedRepo.AutoSize = true;
            addSkippedRepo.Location = new Point(257, -3);
            addSkippedRepo.Name = "addSkippedRepo";
            addSkippedRepo.Size = new Size(15, 15);
            addSkippedRepo.TabIndex = 3;
            addSkippedRepo.Text = "+";
            addSkippedRepo.Click += addSkippedRepo_Click;
            // 
            // consoleOutput
            // 
            consoleOutput.Controls.Add(outputTextbox);
            consoleOutput.Location = new Point(12, 369);
            consoleOutput.Name = "consoleOutput";
            consoleOutput.Size = new Size(900, 165);
            consoleOutput.TabIndex = 3;
            consoleOutput.TabStop = false;
            consoleOutput.Text = "Console Output";
            // 
            // outputTextbox
            // 
            outputTextbox.BackColor = SystemColors.Control;
            outputTextbox.BorderStyle = BorderStyle.None;
            outputTextbox.Enabled = false;
            outputTextbox.Location = new Point(3, 22);
            outputTextbox.Multiline = true;
            outputTextbox.Name = "outputTextbox";
            outputTextbox.ReadOnly = true;
            outputTextbox.Size = new Size(894, 137);
            outputTextbox.TabIndex = 0;
            // 
            // automaticallyAddRepositories
            // 
            automaticallyAddRepositories.AutoSize = true;
            automaticallyAddRepositories.Location = new Point(12, 540);
            automaticallyAddRepositories.Name = "automaticallyAddRepositories";
            automaticallyAddRepositories.Size = new Size(186, 19);
            automaticallyAddRepositories.TabIndex = 4;
            automaticallyAddRepositories.Text = "AutomaticallyAddRepositories";
            automaticallyAddRepositories.UseVisualStyleBackColor = true;
            // 
            // MainWindowForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(921, 564);
            Controls.Add(automaticallyAddRepositories);
            Controls.Add(consoleOutput);
            Controls.Add(skipping);
            Controls.Add(repositories);
            Controls.Add(users);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainWindowForm";
            Text = "Github Stalker";
            Load += MainWindowForm_Load;
            users.ResumeLayout(false);
            users.PerformLayout();
            repositories.ResumeLayout(false);
            repositories.PerformLayout();
            skipping.ResumeLayout(false);
            skipping.PerformLayout();
            consoleOutput.ResumeLayout(false);
            consoleOutput.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public GroupBox users;
        public GroupBox repositories;
        public GroupBox skipping;
        public GroupBox consoleOutput;
        private Label addUser;
        private Label removeUser;
        private Label removeRepository;
        private Label addRepository;
        private Label addSkippedRepo;
        private Label removeSkippedRepo;
        public TextBox outputTextbox;
        public Label usersLabel;
        public Label repositoriesLabel;
        public Label skippingLabel;
        public CheckBox automaticallyAddRepositories;
    }
}
