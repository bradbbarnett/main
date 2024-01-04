using System;
using System.Windows.Forms;
using System.IO;

namespace FileSelectExample
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        private void ButtonFilePath_Click(object sender, EventArgs e)
        {
            textBoxFilePath.Text = GetFileName(); //textBoxFilePath.Text is where the file path is stored.
        }
        
        private void buttonVerifyPath_Click(object sender, EventArgs e)
        {
            if (!ValidFile(textBoxFilePath.Text.Trim()))
            {
                MessageBox.Show("File name is invalid.", "Invalid File");
            }
            if (ValidFile(textBoxFilePath.Text.Trim()))
            {
                MessageBox.Show("File name is valid.", "Valid File");
            }
        }

        private string GetFileName()
        {
            string fname = string.Empty;
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "txt files (*.txt)|*.txt"; //Allows selection of only .txt files
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fname = dlg.FileName;
            }
            return fname;
        }

        private bool ValidFile(string fname)
        {
            if (fname != string.Empty)
            {
                if (File.Exists(fname))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
