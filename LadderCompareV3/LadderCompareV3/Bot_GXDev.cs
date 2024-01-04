using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace LadderCompareV3
{
    public class Bot_GXDev
    {
        public static void Run(string path)
        {
            //Block all keyboard and mouse input (will only work if UAC is disabled)
            NativeMethods.BlockInput(true);

            //Start GX Developer and wait until it opens
            Process.Start(path + @"\gppw.gpj").WaitForInputIdle();

            //Find GX Developer process information
            Process[] p = Process.GetProcessesByName("Gppw");

            //If more than one GX Developer application is open, display error
            if (p.Length > 1)
            {
                MessageBox.Show(
                    "Please close all GX Developer programs before running application",
                    "Ladder Compare",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                Process.GetCurrentProcess().Kill();
            }

            //Get GX Developer handle
            IntPtr gxDevHndl = p[0].MainWindowHandle;

            //Wait for GX Developer to finish opening
            while (true)
            {
                IntPtr currentHndl = NativeMethods.GetForegroundWindow();

                if (currentHndl == gxDevHndl)
                {
                    break;
                }

                //Check for popup window for coordinates are set outside the screen
                IntPtr coordHndl = FindWindowInProcess(p[0], s => s.Equals("MELSOFTｼﾘｰｽﾞ GX Developer"));  //MELSOFT series GX Developer
                List<string> coordText = GetChildrenText(coordHndl);
                bool textFound = listContainsSubstring(coordText, "ｳｨﾝﾄﾞｳ座標が画面外に設定されています"); //Window coordinates are set outside the screen
                if (textFound == true)
                {
                    PressKey("{ENTER}");
                }
            }

            //Run macro for getting to Data Conversion Wizard
            IntPtr dataWizardHndl = IntPtr.Zero;
            dataWizardHndl = ConvWizMacro(p, dataWizardHndl);

            //Get all children window handles from DataWizard
            List<IntPtr> dataWizardChildren = GetChildWindows(dataWizardHndl);

            //Get window captions of ComboBox that contains routine names
            List<string> list = new List<string>();
            StringBuilder sb = new StringBuilder();
            string lastItem = null;

            while (lastItem != sb.ToString())
            {
                sb = new StringBuilder(260);
                NativeMethods.SendMessage(dataWizardChildren[9], NativeMethods.WM_GETTEXT, 260, sb);

                if (list.Count > 0)
                {
                    lastItem = list[list.Count - 1];
                }

                if (lastItem != sb.ToString())
                {
                    list.Add(sb.ToString());
                    PressKey("{DOWN}");
                    Thread.Sleep(100);
                }
            }

            //Go back to top routine in ComboBox
            for (int i = 0; i < list.Count - 1; i++)
            {
                PressKey("{UP}");
            }

            //Run macro to get to export page
            SelectExportMacro(p, path, list[0]);

            //Loop through and do the other routines
            for (int i = 1; i < list.Count; i++)
            {
                dataWizardHndl = IntPtr.Zero;
                dataWizardHndl = ConvWizMacro(p, dataWizardHndl);

                for (int j = 0; j < i; j++)
                {
                    PressKey("{DOWN}");
                }

                SelectExportMacro(p, path, list[i]);
            }

            //Close GX Developer
            p[0].CloseMainWindow();

            //Wait for the close confirmation window to pop up
            IntPtr quitProjectHndl = IntPtr.Zero;

            while (quitProjectHndl == IntPtr.Zero)
            {
                quitProjectHndl = FindWindowInProcess(p[0], s => s.Equals(/*"MELSOFT series GX Developer"*/"MELSOFTｼﾘｰｽﾞ GX Developer"));
            }

            //Answer yes to the close confirmation window
            PressKey("{TAB}");
            PressKey("{ENTER}");

            //Wait until GX Developer closes
            p[0].WaitForExit();

            //Unblock all keyboard and mouse input
            NativeMethods.BlockInput(false);
        }


        private static IntPtr ConvWizMacro(Process[] p, IntPtr dataWizardHndl)
        {
            while (dataWizardHndl == IntPtr.Zero)
            {
                //Check for popup window for coordinates are set outside the screen
                IntPtr coordHndl = FindWindowInProcess(p[0], s => s.Equals("MELSOFTｼﾘｰｽﾞ GX Developer"));  //MELSOFT series GX Developer
                List<string> coordText = GetChildrenText(coordHndl);
                bool textFound = listContainsSubstring(coordText, "ｳｨﾝﾄﾞｳ座標が画面外に設定されています"); //Window coordinates are set outside the screen
                if (textFound == true)
                {
                    PressKey("{ENTER}");
                }

                Thread.Sleep(200);

                //Check if GX Developer popup is reading from project file
                IntPtr busyHndl = FindWindowInProcess(p[0], s => s.Contains("イル読")); //reading

                if (busyHndl == IntPtr.Zero)
                {
                    PressKey("%");
                    PressKey("{F}");
                    PressKey("{E}");
                    PressKey("{T}");
                    dataWizardHndl = FindWindowInProcess(p[0], s => s.StartsWith("ﾃﾞｰﾀ変換ｳｨｻﾞｰﾄﾞ")); //Data conversion wizard
                }
            }

            //Cursor to routines ComboBox
            PressKey("{DOWN}");
            PressKey("{DOWN}");

            //scroll down in ComboBox until circuit diagram is found 
            while (true)
            {
                Thread.Sleep(50);

                List<string> wizChildText = GetChildrenText(dataWizardHndl);

                //Ensure a match.  If not, index down.
                if (!(wizChildText.Find(x => x.Equals("回路図")) == "回路図")) //circuit diagram
                {
                    PressKey("{DOWN}");
                }
                else
                {
                    break;
                }
            }

            PressKey("{TAB}");
            Thread.Sleep(50);

            return dataWizardHndl;
        }

        private static void SelectExportMacro(Process[] p, string path, string routine)
        {
            //Get to SaveAs function
            PressKey("{TAB}");
            PressKey("{TAB}");
            PressKey("{TAB}");
            PressKey("{TAB}");
            PressKey("{ENTER}");

            //Wait for SaveAs window to open
            IntPtr saveAsHndl = IntPtr.Zero;

            while (saveAsHndl == IntPtr.Zero)
            {
                saveAsHndl = FindWindowInProcess(p[0], s => s.StartsWith("ﾌｧｲﾙ名を付けて保存"));
            }

            //Cursor to Path
            PressKey("{TAB}");
            PressKey("{TAB}");

            //Get all children from the SaveAs window
            List<string> saveAsChildText = GetChildrenText(saveAsHndl);

            //Ensure the paths match.  If not, insert it. (this can be replaced by listContainsSubstring method)
            if (!(saveAsChildText.Find(x => x.Contains(path)) == (path + @"\")))
            {
                PressKey(path + @"\");

            }

            PressKey("{TAB}");
            PressKey("LAD_" + routine);
            PressKey("{TAB}");
            PressKey("{TAB}");
            PressKey("{ENTER}");
            Thread.Sleep(100);

            //Wait until exporting is finished
            while (true)
            {
                IntPtr gxConvHndl = IntPtr.Zero;
                gxConvHndl = NativeMethods.FindWindow(null, "GX Converter");

                Thread.Sleep(100);

                if (gxConvHndl != IntPtr.Zero)
                {
                    List<string> gxConvChildrenStr = GetChildrenText(gxConvHndl);

                    //Overwrite file if asked
                    if (gxConvChildrenStr.Count == 4)
                    {
                        if (gxConvChildrenStr[3].ToString().Contains("は既に存在しています")) //Already exists
                        {
                            PressKey("{ENTER}");
                        }
                    }

                    //Overwrite file if asked
                    if (gxConvChildrenStr.Count == 4)
                    {
                        if (gxConvChildrenStr[3].ToString().Contains("前回値保存ﾃﾞｰﾀを更新してよろしいですか")) //Are you sure you want to update the value saved last time
                        {
                            PressKey("{TAB}");
                            PressKey("{ENTER}");
                        }
                    }

                    //Once export is finished, get out of this loop
                    if (gxConvChildrenStr.Count == 3)
                    {
                        if (gxConvChildrenStr[2].ToString().StartsWith(/*"Completed"*/"完了しました"))
                        {
                            PressKey("{ENTER}");
                            break;
                        }
                    }
                }
            }
        }

        private static List<string> GetChildrenText(IntPtr parentHndl)
        {
            List<string> childrenText = new List<string>();

            //Get all children handles from the parent handle
            List<IntPtr> childrenHndl = GetChildWindows(parentHndl);

            //Get all text from children handles
            for (int i = 0; i < childrenHndl.Count; i++)
            {
                StringBuilder sb = new StringBuilder(260);
                NativeMethods.SendMessage(childrenHndl[i], NativeMethods.WM_GETTEXT, 260, sb);
                childrenText.Add(sb.ToString());
            }

            return childrenText;
        }

        private static void PressKey(string key)
        {
            if ((key.Contains("(") || key.Contains(")") ||
                key.Contains("+") || key.Contains("~") ||
                key.Contains("%") || key.Contains("^") ||
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

            Thread.Sleep(10);
        }

        private static bool listContainsSubstring(List<string> text, string substring)
        {
            bool found = false;

            foreach (var line in text)
            {
                bool test = line.Contains(substring);
                if (test == true)
                {
                    found = true;
                }
            }

            return found;
        }

        private static IntPtr FindWindowInProcess(Process process, Func<string, bool> compareTitle)
        {
            IntPtr windowHandle = IntPtr.Zero;

            foreach (ProcessThread t in process.Threads)
            {
                windowHandle = FindWindowInThread(t.Id, compareTitle);
                if (windowHandle != IntPtr.Zero)
                {
                    break;
                }
            }

            return windowHandle;
        }

        private static IntPtr FindWindowInThread(int threadId, Func<string, bool> compareTitle)
        {
            IntPtr windowHandle = IntPtr.Zero;
            NativeMethods.EnumThreadWindows(threadId, (hWnd, lParam) =>
            {
                StringBuilder text = new StringBuilder(200);
                NativeMethods.GetWindowText(hWnd, text, 200);
                if (compareTitle(text.ToString()))
                {
                    windowHandle = hWnd;
                    return false;
                }
                return true;
            }, IntPtr.Zero);

            return windowHandle;
        }

        private static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                NativeMethods.EnumWindowProc childProc = new NativeMethods.EnumWindowProc(EnumWindow);
                NativeMethods.EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }

            return result;
        }

        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }

            list.Add(handle);

            return true;
        }
    }
}
