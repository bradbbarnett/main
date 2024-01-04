using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Taskbar;

namespace LadderCompareV3
{
    public partial class Main : Form
    {
        public static string orientation;
        public static bool debug;
        public static List<string> rejectedSubRoutines;

        public Main()
        {
            InitializeComponent();
        }

        private void TextboxBefore_DragDrop(object sender, DragEventArgs e)
        {
            string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
            TextboxBefore.Text = file[0];
        }

        private void TextboxAfter_DragDrop(object sender, DragEventArgs e)
        {
            string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
            TextboxAfter.Text = file[0];
        }

        private void TextboxBefore_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void TextboxAfter_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void ButtonBefore_Click(object sender, EventArgs e)
        {
            FolderBrowserDialogBefore.ShowDialog();
            TextboxBefore.Text = FolderBrowserDialogBefore.SelectedPath.ToString();
        }

        private void ButtonAfter_Click(object sender, EventArgs e)
        {
            FolderBrowserDialogAfter.ShowDialog();
            TextboxAfter.Text = FolderBrowserDialogAfter.SelectedPath.ToString();
        }

        private void ButtonCompare_Click(object sender, EventArgs e)
        {
            //Check if before path and after path are identical
            if (TextboxBefore.Text == TextboxAfter.Text)
            {
                MessageBox.Show("Cannot compare ladders with same name");
                return;
            }

            //Check that each directory has only one application
            string[] dirBeforeGXW = Directory.GetFiles(TextboxBefore.Text, "*.gxw");
            string[] dirBeforeGPPW = Directory.GetFiles(TextboxBefore.Text, "*.gpj");

            if (dirBeforeGXW.Length + dirBeforeGPPW.Length == 0)
            {
                MessageBox.Show("'Before' folder does not contain a valid application file");
                return;
            }
            if (dirBeforeGXW.Length + dirBeforeGPPW.Length > 1)
            {
                MessageBox.Show("'Before' folder contains multiple application files");
                return;
            }

            string[] dirAfterGXW = Directory.GetFiles(TextboxAfter.Text, "*.gxw");
            string[] dirAfterGPPW = Directory.GetFiles(TextboxAfter.Text, "*.gpj");

            if (dirAfterGXW.Length + dirAfterGPPW.Length == 0)
            {
                MessageBox.Show("'After' folder does not contain a valid application file");
                return;
            }
            if (dirAfterGXW.Length + dirAfterGPPW.Length > 1)
            {
                MessageBox.Show("'After' folder contains multiple application files");
                return;
            }

            //Check that both applications are GXW or GPPW, not mixed
            if ((dirBeforeGXW.Length + dirAfterGXW.Length) % 2 != 0)
            {
                MessageBox.Show("Can't compare GX Developer file to GX Works2 file");
                return;
            }

            //Ensure application is not already open
            if (Process.GetProcessesByName("Gppw").Length > 0)
            {
                MessageBox.Show("Please close all GX Developer programs before running application");
                return;
            }
            if (Process.GetProcessesByName("GD2").Length > 0)
            {
                MessageBox.Show("Please close all GX Works2 programs before running application");
                return;
            }

            //Check if temporary GX Developer folders already exist (GXW2)
            string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string pathBeforeTempGXDev = null;
            string pathAfterTempGXDev = null;

            if (dirBeforeGXW.Length > 0)
            {
                string fileNameBefore = Path.GetFileNameWithoutExtension(dirBeforeGXW[0]);
                string fileNameAfter = Path.GetFileNameWithoutExtension(dirAfterGXW[0]);

                pathBeforeTempGXDev = pathDesktop + @"\" + fileNameBefore + "_GXDev";
                pathAfterTempGXDev = pathDesktop + @"\" + fileNameAfter + "_GXDev";
            }

            if (Directory.Exists(pathBeforeTempGXDev))
            {
                Directory.Delete(pathBeforeTempGXDev, true);
            }
            if (Directory.Exists(pathAfterTempGXDev))
            {
                Directory.Delete(pathAfterTempGXDev, true);
            }

            //Disable form controls
            ButtonBefore.Enabled = false;
            ButtonAfter.Enabled = false;
            ButtonCompare.Enabled = false;
            TextboxBefore.Enabled = false;
            TextboxAfter.Enabled = false;
            radioButtonPortrait.Enabled = false;
            radioButtonLandscape.Enabled = false;
            checkBoxDebug.Enabled = false;

            //Set orientation of final Word/PDF documents
            if (radioButtonPortrait.Checked == true)
            {
                orientation = "Portrait";
            }
            else
            {
                orientation = "Landscape";
            }

            //Set debug checkbox
            if (checkBoxDebug.Checked == true)
            {
                debug = true;
            }

            //Run bot
            if (dirBeforeGXW.Length == 1)
            {
                //Block all keyboard and mouse input (will only work if UAC is disabled)
                //NativeMethods.BlockInput(true);

                Bot_GXWorks.Run(TextboxBefore.Text);
                Thread.Sleep(1000);
                Bot_GXWorks.Run(TextboxAfter.Text);
                Thread.Sleep(1000);

                TextboxBefore.Text = pathBeforeTempGXDev;
                TextboxAfter.Text = pathAfterTempGXDev;
            }

            Bot_GXDev.Run(TextboxBefore.Text);
            Thread.Sleep(1000);
            Bot_GXDev.Run(TextboxAfter.Text);

            //Reject subroutines that don't exist in both before and after ladders
            rejectedSubRoutines = Merge.Reject(TextboxBefore.Text, TextboxAfter.Text);

            //Run merge and compare thread
            backgroundWorker.RunWorkerAsync();

            //Run compare status display thread
            backgroundText.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> before = Merge.Run(TextboxBefore.Text);
            List<string> after = Merge.Run(TextboxAfter.Text);
            Compare.Run(TextboxBefore.Text, TextboxAfter.Text, before, after);
        }

        private void backgroundText_DoWork(object sender, DoWorkEventArgs e)
        {
            while (backgroundWorker.IsBusy)
            {
                LabelStatus.Text = "Comparing";
                Thread.Sleep(500);
                LabelStatus.Text = "Comparing.";
                Thread.Sleep(500);
                LabelStatus.Text = "Comparing..";
                Thread.Sleep(500);
                LabelStatus.Text = "Comparing...";
                Thread.Sleep(500);
                LabelStatus.Text = "Comparing....";
                Thread.Sleep(500);
                LabelStatus.Text = "Comparing.....";
                Thread.Sleep(500);
            }

            LabelStatus.Text = "";

            string routinesOmitted = null;
            foreach (var item in rejectedSubRoutines)
            {
                routinesOmitted = routinesOmitted + item + "\n";
            }

            if (rejectedSubRoutines.Count == 0)
            {
                MessageBox.Show("Compare Successful!");
            }
            else
            {
                MessageBox.Show("Compare Successful!\n\n" + "Routines omitted from compare:\n" + routinesOmitted);
            }

            Close();
        }
    }
}
