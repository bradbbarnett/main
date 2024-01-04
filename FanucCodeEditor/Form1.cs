using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FanucCodeEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
            textBox.Text = file[0];
        }

        private void textBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void buttonFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowDialog();
            textBox.Text = folderBrowser.SelectedPath;
        }

        private void DisableFormControls()
        {
            buttonFile.Enabled = false;
            buttonCombine.Enabled = false;
            buttonSeparate.Enabled = false;
            textBox.Enabled = false;
        }

        private void EnableFormControls()
        {
            buttonFile.Enabled = true;
            buttonCombine.Enabled = true;
            buttonSeparate.Enabled = true;
            textBox.Enabled = true;
        }

        private void buttonCombine_Click(object sender, EventArgs e)
        {
            if (ValidDirectory(textBox.Text))
            {
                DisableFormControls();

                //Delete text file for compilation of all .LS files
                string fileName = textBox.Text + @"\Combined.txt";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                //Find all .LS files and copy all text to a single list
                string[] files = Directory.GetFiles(textBox.Text, "*.LS");
                List<string> programCompilation = new List<string>();
                foreach (var file in files)
                {
                    programCompilation.AddRange(File.ReadAllLines(file));
                }

                //Remove line numbers in program and add visual seperator
                bool hasLineNumbers = false;
                List<string> programCompilationUpdated = new List<string>();
                foreach (string line in programCompilation)
                {
                    if (line.Contains("/POS") == true)
                    {
                        hasLineNumbers = false;
                    }

                    if (hasLineNumbers == true)
                    {
                        programCompilationUpdated.Add(line.Substring(5, line.Length - 5));
                    }
                    else
                    {
                        programCompilationUpdated.Add(line);
                    }

                    if (line.Contains("/MN") == true)
                    {
                        hasLineNumbers = true;
                    }

                    if (line.Contains("/END") == true)
                    {
                        programCompilationUpdated.Add("--------------------------------------------------------------");
                    }
                }

                //Create text file with updates
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    foreach (var line in programCompilationUpdated)
                    {
                        sw.WriteLine(line);
                        sw.Flush();
                    }
                }
                EnableFormControls();
            }

            else
            {
                MessageBox.Show("Please select folder containing .LS files", "Invalid Path");
                textBox.Focus();
            }
        }



        private void buttonSeparate_Click(object sender, EventArgs e)
        {
            if (ValidDirectory(textBox.Text))
            {
                DisableFormControls();

                //Create new folder for updated programs
                string updatedFolder = textBox.Text + @"\Updated";
                if (Directory.Exists(updatedFolder))
                {
                    DirectoryInfo di = new DirectoryInfo(updatedFolder);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }
                Directory.CreateDirectory(updatedFolder);

                //Read program compilation file
                string fileName = textBox.Text + @"\Combined.txt";
                if (File.Exists(fileName) != true)
                {
                    MessageBox.Show("Combined text file does not exist in current directory");
                    Application.Exit();
                }
                List<string> programCompilation = new List<string>();
                foreach (string line in File.ReadLines(fileName))
                {
                    programCompilation.Add(line);
                }

                //add line numbers and remove visual aid
                bool needsLineNumber = false;
                int count = 1;
                List<string> programCompilationUpdated = new List<string>();

                foreach (string line in programCompilation)
                {
                    //POS indicates end of line numbers
                    if (line.Contains("/POS") == true)
                    {
                        needsLineNumber = false;
                        count = 1;
                    }

                    if (needsLineNumber == true)
                    {
                        string buffer = (count.ToString() + ":  ").PadLeft(5);
                        programCompilationUpdated.Add(string.Concat(buffer, line));
                        count++;
                    }
                    else
                    {
                        programCompilationUpdated.Add(line);
                    }

                    //MN indicates start of line numbers
                    if (line.Contains("/MN") == true)
                    {
                        needsLineNumber = true;
                    }

                    if (line.Contains("--------------------------------------------------------------") == true)
                    {
                        programCompilationUpdated.Remove(line);
                    }
                }

                //update LINE_COUNT
                List<string> xyz = new List<string>();
                foreach (string line in programCompilationUpdated)
                {

                }


                //Find "/PROG  " and create new text file
                string progName = null;
                foreach (string line in programCompilationUpdated)
                {
                    //Create program file
                    if (line.Contains("/PROG  ") == true)
                    {
                        if (line.IndexOf("\t") < 0)
                        {
                            progName = line.Substring(7, line.Length - 7);
                        }
                        else
                        {
                            progName = line.Substring(7, line.IndexOf("\t") - 7);
                        }
                        File.Create(updatedFolder + @"\" + progName + ".LS").Close();
                    }

                    using (StreamWriter sw = new StreamWriter(updatedFolder + @"\" + progName + ".LS", true))
                    {
                        sw.WriteLine(line);
                        sw.Flush();

                        if (line.Contains("/END") == true)
                        {
                            sw.Close();
                        }
                    }
                }
                EnableFormControls();
            }

            else
            {
                MessageBox.Show("Please select folder containing .LS files", "Invalid Path");
                textBox.Focus();
            }
        }

        private bool ValidDirectory(string path)
        {
            if (Directory.GetFiles(path, "*.LS").Length == 0)
                return false;
            else
                return true;
        }
    }
}