using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace MemoryLadGX
{
    public static class Methods
    {
        public static List<string> GetChildrenText(IntPtr parentHndl)
        {
            List<string> childrenText = new List<string>();

            //Get all children handles from the parent handle
            List<IntPtr> childrenHndl = NativeMethods.GetChildWindows(parentHndl);

            //Get all text from children handles
            for (int i = 0; i < childrenHndl.Count; i++)
            {
                StringBuilder sb = new StringBuilder(260);
                NativeMethods.SendMessage(childrenHndl[i], NativeMethods.WM_GETTEXT, 260, sb);
                childrenText.Add(sb.ToString());
            }

            return childrenText;
        }

        public static void DeleteFolder(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (folderExists == true)
            {
                try
                {
                    Directory.Delete(path, true);
                }
                catch (IOException)
                {
                    MessageBox.Show("Folder open by another process and cannot be deleted",
                        "Fatal Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                    Process.GetCurrentProcess().Kill();
                }
            }
        }

        public static void PressKey(string key)
        {
            //SendKeys does not normally allow symbols to be entered (for file paths)
            if ((key.Contains("(") || key.Contains(")") ||
                 key.Contains("+") || key.Contains("~") ||
                 key.Contains("[") || key.Contains("]")) & key.Length > 3)
            {
                key = Regex.Replace(key, "[+^%~()]", "{$0}");
            }

            bool x = Control.IsKeyLocked(Keys.CapsLock);
            if (x == true)
            {
                SendKeys.SendWait("{CAPSLOCK}" + key);
            }
            else
            {
                SendKeys.SendWait(key);
            }

            Thread.Sleep(50);
        }

        public static IntPtr WaitForWindow(Process process, string text)
        {
            System.Timers.Timer timeout = new System.Timers.Timer(5000);
            timeout.Enabled = true;
            timeout.Elapsed += Timeout;

            IntPtr hndl = new IntPtr();
            while (hndl.Equals(IntPtr.Zero))
            {
                hndl = NativeMethods.FindWindowInProcess(process, s => s.Equals(text));
            }
            timeout.Enabled = false;
            Thread.Sleep(50);
            return hndl;
        }

        public static void Timeout(object source, System.Timers.ElapsedEventArgs e)
        {
            MessageBox.Show(
                "Bot lost its place.  Please try again.  If this keeps happening, please contact bbarnett@mazakcorp.com",
                "Fatal Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
            Process.GetCurrentProcess().Kill();
        }

        //Used to find text in comboboxes
        public static void SelectText(string text, IntPtr hWnd)
        {
            bool directionUp = true;
            string previousText = null;

            //Get process ID from handle
            NativeMethods.GetWindowThreadProcessId(hWnd, out int processID);

            while (true)
            {
                //Wait for window idle
                Process.GetProcessById(processID).WaitForInputIdle();
                Thread.Sleep(100);

                //Get combobox text
                StringBuilder sb = new StringBuilder(260);
                NativeMethods.SendMessage(hWnd, NativeMethods.WM_GETTEXT, 260, sb);
                string currentText = sb.ToString();

                //Text didn't change (top or bottom of combobox list)
                if (currentText == previousText)
                {
                    //Change direction
                    if (directionUp == true)
                    {
                        directionUp = false;
                    }
                    //Desired text not found
                    else
                    {
                        string msg = "Couldn't locate " + text;
                        MessageBox.Show(msg, null, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        Process.GetCurrentProcess().Kill();
                    }
                }

                //Desired text found
                if (currentText == text)
                {
                    break;
                }
                //Desired text not found
                else
                {
                    //Store combobox text for comparing after index
                    previousText = currentText;

                    //Index combobox text
                    if (directionUp == true)
                    {
                        PressKey("{UP}");
                    }
                    else
                    {
                        PressKey("{DOWN}");
                    }
                }
            }
        }
    }
}
