using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GithubStalker
{
    public partial class InputWindow : Form
    {
        public string? InputText;

        public InputWindow()
        {
            InitializeComponent();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            InputText = textBox1.Text;
            Close();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();

        }
    }
}
