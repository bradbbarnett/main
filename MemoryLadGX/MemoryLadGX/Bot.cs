using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace MemoryLadGX
{
    public static class Bot
    {
        public static void Run(string fileDirectory)
        {
            int fileIndex = fileDirectory.LastIndexOf("\\");
            string fileName = fileDirectory.Substring(fileIndex + 1);
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //string currentDir = Directory.GetCurrentDirectory();
            string currentDir = desktopPath + @"\Other\cnvgp8";

            //Block all keyboard and mouse input (will only work if UAC is disabled)
            //NativeMethods.BlockInput(true);

            #region Folder management

            //Delete old temp folder if it exists
            Methods.DeleteFolder(currentDir + @"\temp");

            //Create temp folder
            Directory.CreateDirectory(currentDir + @"\temp");

            //Copy file to temp folder
            File.Copy(fileDirectory, currentDir + @"\temp\tempFile", true);

            //Delete ladder project if it exists
            Methods.DeleteFolder(desktopPath + @"\LAD_" + fileName);

            #endregion

            #region cnvgp8 - Create mem file from lad file

            //Settings of CNVGP8 program can be found under Settings > Environment
            //The work folder is fixed: Checked
            //location: "C:\Program Files (x86)\cnvgp8\temp"
            //Conversion is executed at the same time as opening file: Checked
            //Type: m800
            //NC Setting: Nothing checked.
            //GX Setting: Both checked.  Output folder is "output"

            //Start cnvgp8
            //Process.Start(@"C:\Program Files (x86)\cnvgp8\cnvgp8.exe").WaitForInputIdle();
            Process.Start(currentDir + @"\cnvgp8.exe").WaitForInputIdle();

            //Find cnvgp8 process information
            Process[] pCnvgp8 = Process.GetProcessesByName("CNVGP8");

            //If more than one application is open, display error
            if (pCnvgp8.Length > 1)
            {
                MessageBox.Show(
                    "Please close all cnvgp8 programs before running application",
                    "Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                Process.GetCurrentProcess().Kill();
            }

            //Get handle
            IntPtr cnvgp8Hndl = pCnvgp8[0].MainWindowHandle;

            //Open file and convert
            Methods.PressKey("%"); //ALT
            Methods.PressKey("{F}"); //File
            Methods.PressKey("{O}"); //Open
            Methods.PressKey("{DOWN}");
            Methods.PressKey("{ENTER}"); //Select "tempFile"

            //Close application
            Methods.PressKey("%"); //ALT
            Methods.PressKey("{F}"); //File
            Methods.PressKey("{X}"); //Exit
            pCnvgp8[0].WaitForExit();

            #endregion

            #region GX Developer

            //Start GX Developer
            Process.Start(@"C:\MELSEC\Gppw\Gppw.exe").WaitForInputIdle();

            //Find GX Developer process information
            Process[] pGX = Process.GetProcessesByName("Gppw");

            //If more than one GX Developer application is open, display error
            if (pGX.Length > 1)
            {
                MessageBox.Show(
                    "Please close all GX Developer programs before running application",
                    "Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                Process.GetCurrentProcess().Kill();
            }

            //Get handle
            IntPtr hWndGx = pGX[0].MainWindowHandle;

            //Open new project
            Methods.PressKey("%"); //ALT
            Methods.PressKey("{F}"); //File
            Methods.PressKey("{N}"); //New

            //New project window
            IntPtr hWndProj = Methods.WaitForWindow(pGX[0], "ﾌﾟﾛｼﾞｪｸﾄ新規作成"); //"Create a new project"

            //Get all children handles for new project window
            List<IntPtr> hWndProjChildren = NativeMethods.GetChildWindows(hWndProj);

            //Select PLC series and type
            Methods.SelectText("QCPU(Qﾓｰﾄﾞ)", hWndProjChildren[0]);
            Methods.PressKey("{TAB}");
            Methods.SelectText("Q26UDH", hWndProjChildren[1]);
            Methods.PressKey("{ENTER}");

            //Wait for ladder creation complete
            Thread.Sleep(50);

            //Get all children handles for GX Developer
            List<IntPtr> hWndGxChildren = NativeMethods.GetChildWindows(hWndGx);

            //Bring focus to project list on lefthand side
            for (int i = 0; i < hWndGxChildren.Count; i++)
            {
                StringBuilder sb = new StringBuilder(260);
                NativeMethods.SendMessage(hWndGxChildren[i], NativeMethods.WM_GETTEXT, 260, sb);
                if (sb.ToString() == "ﾌﾟﾛｼﾞｪｸﾄﾃﾞｰﾀ一覧") //"List of project data"
                {
                    NativeMethods.SetForegroundWindow(hWndGxChildren[i + 2]);
                    Thread.Sleep(50);
                    break;
                }
            }

            //Delete MAIN
            Methods.PressKey("{RIGHT}");
            Methods.PressKey("{RIGHT}");
            Methods.PressKey("{RIGHT}");
            Methods.PressKey("{RIGHT}");
            Methods.PressKey("{DELETE}");
            Methods.PressKey("{TAB}");
            Methods.PressKey("{ENTER}");

            //Bring focus to main GX Developer window
            NativeMethods.SetForegroundWindow(hWndGx);

            //Open Memory window
            Methods.PressKey("%");
            Methods.PressKey("{T}");
            Methods.PressKey("{I}");
            Methods.PressKey("{M}");

            //Memory card data window
            IntPtr hWndMem = Methods.WaitForWindow(pGX[0], "ICﾒﾓﾘｶｰﾄﾞ  ｲﾒｰｼﾞﾃﾞｰﾀ読出"); //IC memory card read image data

            //Get all children handles for memory window
            List<IntPtr> hWndMemChildren = NativeMethods.GetChildWindows(hWndMem);

            //Select memory type
            Methods.SelectText("Q3MEM-8MBS", hWndMemChildren[10]);
            Methods.PressKey("{TAB}");
            Methods.PressKey("{ENTER}");

            //Insert path of memory file
            Methods.PressKey(currentDir + @"\temp\output\PRJ1_ICMEM");
            Methods.PressKey("{ENTER}");

            //Select all components
            Methods.PressKey("{TAB}");
            Methods.PressKey("{TAB}");
            Methods.PressKey("{TAB}");
            Methods.PressKey("{ENTER}");
            Methods.PressKey("{TAB}");
            Methods.PressKey("{TAB}");
            Methods.PressKey("{TAB}");
            Methods.PressKey("{ENTER}");

            //Are you sure? window
            IntPtr zzHndl = Methods.WaitForWindow(pGX[0], "MELSOFTｼﾘｰｽﾞ GX Developer"); //MELSOFT series GX Developer
            Methods.PressKey("{ENTER}");

            //Are you sure? (again) window
            IntPtr finishHndl = Methods.WaitForWindow(pGX[0], "MELSOFT ｼﾘｰｽﾞ GX Developer"); //MELSOFT series GX Developer
            Methods.PressKey("{TAB}");
            Methods.PressKey("{ENTER}");

            //Finished window
            IntPtr doneHndl = Methods.WaitForWindow(pGX[0], "MELSOFTｼﾘｰｽﾞ GX Developer"); //MELSOFT series GX Developer
            Methods.PressKey("{ENTER}");

            //Close memory card data window
            Methods.PressKey("{TAB}");
            Methods.PressKey("{ENTER}");

            //Save file
            Methods.PressKey("%");
            Methods.PressKey("{F}");
            Methods.PressKey("{S}");

            //Save file window
            IntPtr saveHndl = Methods.WaitForWindow(pGX[0], "ﾌﾟﾛｼﾞｪｸﾄの名前を付けて保存"); //Save as project name

            //Delete default path and insert new path
            Methods.PressKey("{TAB}");
            Methods.PressKey("{TAB}");
            Methods.PressKey("^{DELETE}");
            Methods.PressKey(desktopPath);
            Methods.PressKey("{TAB}");
            Methods.PressKey("LAD_" + fileName);
            Methods.PressKey("{TAB}");
            Methods.PressKey("{TAB}");
            Methods.PressKey("{ENTER}");
            Methods.PressKey("{ENTER}");

            //Close GX Developer
            pGX[0].CloseMainWindow();

            //Are you sure? window
            IntPtr ztHndl = Methods.WaitForWindow(pGX[0], "MELSOFTｼﾘｰｽﾞ GX Developer");

            //Close window
            Methods.PressKey("{TAB}");
            Methods.PressKey("{ENTER}");

            #endregion

            //Delete old temp folder if it exists
            Methods.DeleteFolder(currentDir + @"\temp");

            //Unblock all keyboard and mouse input
            NativeMethods.BlockInput(false);
        }
    }
}