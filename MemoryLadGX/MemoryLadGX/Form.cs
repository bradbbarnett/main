using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace MemoryLadGX
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        private void Textbox_DragDrop(object sender, DragEventArgs e)
        {
            string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
            Textbox.Text = file[0];
        }

        private void Textbox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
        
        private void ButtonFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fileDialog.ShowDialog();
            Textbox.Text = fileDialog.FileName;
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (!ValidDirectory(Textbox.Text))
            {
                MessageBox.Show("Please select a memory file", "Invalid File");
                Textbox.Focus();
            }

            if (ValidDirectory(Textbox.Text))
            {
                //Disable form controls
                ButtonFile.Enabled = false;
                ButtonStart.Enabled = false;
                Textbox.Enabled = false;

                //Enable timeout timer
                System.Timers.Timer timeout = new System.Timers.Timer(60000);
                timeout.Enabled = true;
                timeout.Elapsed += Methods.Timeout;

                //Run bot
                Bot.Run(Textbox.Text);

                //Disable timeout timer
                timeout.Enabled = false;

                //Exit program
                Process.GetCurrentProcess().Kill();
            }
        }

        private bool ValidDirectory(string path)
        {
            if (path != string.Empty)
            {
                return true;
            }

            return false;
        }
    }
}