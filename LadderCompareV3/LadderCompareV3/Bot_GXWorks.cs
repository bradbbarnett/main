using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Linq;

namespace LadderCompareV3
{
    public static class Bot_GXWorks
    {        
        public static void Run(string path)
        {
            Process[] p = null;
            Stopwatch sw = new Stopwatch();
            NodeData node = new NodeData();
            string fullPath = Directory.GetFiles(path, "*.gxw")[0];
            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            IntPtr hMain = IntPtr.Zero;
            IntPtr hNavTree = IntPtr.Zero;
            IntPtr hCursor = IntPtr.Zero;
            int count = 0;

            StartApplication();

            //Check for expanded navigation tree
            IntPtr hMDI = NativeMethods.FindWindowClass(hMain, "MDIClient");
            List<IntPtr> children = GetChildWindows(hMDI);
            if (children.Count > 0)
            {
                CloseMDIClient();
                SaveApplication();
                p[0].Kill();
                WaitUntil(() => p[0].HasExited);
                StartApplication();
            }

            ResetDockWindow();
            GetNavigationTree();
            DeleteGlobalVar();
            CountSFB();
            DeleteSFB();
            Compile();
            ExportToGXDev();
            //SaveApplication();
            //CloseApplication();
            //WaitForSavePopup();
            p[0].Kill();
            WaitUntil(() => p[0].HasExited);

            DeleteResources();

            void StartApplication()
            {
                //Start GX Works2 and wait until it opens
                Process.Start(fullPath).WaitForInputIdle();

                //Find GX Works2 process information
                p = Process.GetProcessesByName("GD2");

                //Find "Opening the project." window and wait for it to close
                //IntPtr hTemp = WaitForWindowOpen("#32770", "Opening the project.");
                //WaitForWindowClose(IntPtr.Zero, "Opening the project.");
                //WaitUntil(() => !NativeMethods.IsWindow(hTemp));
                //bool result = SpinWait.SpinUntil(() => !NativeMethods.IsWindow(hTemp), TimeSpan.FromSeconds(5));
                //if (!result) { throw new TimeoutException("Failed to find 'Opening the project.' window"); }

                //Close 'language selected in GX Works2 and Windows do not match if it pops up
                FindGXWindowCaption("The language selected in GX Works2 and Windows do not match.");
                SendKeyWrapper("{ENTER}");

                //Get GX Works2 handle
                hMain = p[0].MainWindowHandle;
            }

            //Center window where ladder is displayed.  Any window causes tree expansion.
            void CloseMDIClient()
            {
                SendKeyWrapper("%");
                SendKeyWrapper("w");
                SendKeyWrapper("l");

                while (children.Count > 0)
                {
                    children = GetChildWindows(hMDI);
                }
            }

            void SaveApplication()
            {
                SendKeyWrapper("%");
                SendKeyWrapper("p");
                SendKeyWrapper("s");
            }

            void ResetDockWindow()
            {
                SendKeyWrapper("%");
                SendKeyWrapper("v");
                SendKeyWrapper("k");
                SendKeyWrapper("p");

                FindGXWindowCaption("Reset the Docking Window position");

                SendKeyWrapper("{LEFT}");
                SendKeyWrapper("{ENTER}");
            }

            void GetNavigationTree()
            {
                //Get parent Navigation pane handle
                IntPtr hTemp = NativeMethods.FindWindowClass(hMain, "XTPDockingPaneTabbedContainer");

                //Open Navigation pane if not open
                if (hTemp == IntPtr.Zero)
                {
                    SendKeyWrapper("%");
                    SendKeyWrapper("v");
                    SendKeyWrapper("k");
                    SendKeyWrapper("n");
                }

                //Get child handle of SysTreeView32 from Navigation pane
                hTemp = WaitForWindowOpen(hMain, "XTPDockingPaneTabbedContainer");
                hTemp = WaitForWindowOpen(hTemp, "XTPShortcutBar");
                hTemp = GetChildWindows(hTemp)[0]; //Not always Afx:00400000:0:00010007:00000000:00000000
                hNavTree = WaitForWindowOpen(hTemp, "SysTreeView32");
            }

            void DeleteGlobalVar()
            {
                node = new NodeData();
                hCursor = IntPtr.Zero;

                //Wait until Navigation tree is set to foreground
                SetForeground(hNavTree);

                //Move cursor to Global Label
                sw.Reset();
                sw.Start();
                while (node.Text != "Global Label")
                {
                    //if (sw.ElapsedMilliseconds > 10000) { throw new TimeoutException("Failed to find Global Label in Navigation pane"); }
                    SendKeyWrapper("{DOWN}");
                    hCursor = NativeMethods.SendMessage(hNavTree, (int)NativeMethods.TVM.TVM_GETNEXTITEM, (int)NativeMethods.TVGN.TVGN_CARET, 0);
                    node = AllocTest(p[0], hNavTree, hCursor);
                }

                //Move cursor to Global1
                SendKeyWrapper("{RIGHT}");
                SendKeyWrapper("{DOWN}");
                SendKeyWrapper("{ENTER}");

                //Delete all labels
                SendKeyWrapper("^a");
                SendKeyWrapper("{DELETE}");

                //Wait for popup window
                FindGXWindowCaption("Unable to execute Undo/Redo if execute this operation.");

                SendKeyWrapper("{LEFT}");
                SendKeyWrapper("{ENTER}");
                SendKeyWrapper("{LEFT}");
                SendKeyWrapper("{ENTER}");
            }

            void CountSFB()
            {
                //Find POU
                sw.Reset();
                sw.Start();
                while (node.Text != "POU")
                {
                    if (sw.ElapsedMilliseconds > 5000) { throw new TimeoutException("Failed to find POU in Navigation pane"); }
                    hCursor = NativeMethods.SendMessage(hNavTree, (int)NativeMethods.TVM.TVM_GETNEXTITEM, (int)NativeMethods.TVGN.TVGN_NEXT, (int)hCursor);
                    node = AllocTest(p[0], hNavTree, hCursor);
                }
                hCursor = NativeMethods.SendMessage(hNavTree, (int)NativeMethods.TVM.TVM_GETNEXTITEM, (int)NativeMethods.TVGN.TVGN_CHILD, (int)hCursor);
                node = AllocTest(p[0], hNavTree, hCursor);

                //Find FB_Pool
                sw.Reset();
                sw.Start();
                while (node.Text != "FB_Pool")
                {
                    if (sw.ElapsedMilliseconds > 5000) { throw new TimeoutException("Failed to find FB_Pool in Navigation pane"); }
                    hCursor = NativeMethods.SendMessage(hNavTree, (int)NativeMethods.TVM.TVM_GETNEXTITEM, (int)NativeMethods.TVGN.TVGN_NEXT, (int)hCursor);
                    node = AllocTest(p[0], hNavTree, hCursor);
                }
                hCursor = NativeMethods.SendMessage(hNavTree, (int)NativeMethods.TVM.TVM_GETNEXTITEM, (int)NativeMethods.TVGN.TVGN_CHILD, (int)hCursor);
                node = AllocTest(p[0], hNavTree, hCursor);

                //Count SFB routines
                sw.Reset();
                sw.Start();
                while (hCursor != IntPtr.Zero)
                {
                    if (sw.ElapsedMilliseconds > 5000) { throw new TimeoutException("Failed to count SFB routines in Navigation pane"); }
                    hCursor = NativeMethods.SendMessage(hNavTree, (int)NativeMethods.TVM.TVM_GETNEXTITEM, (int)NativeMethods.TVGN.TVGN_NEXT, (int)hCursor);
                    node = AllocTest(p[0], hNavTree, hCursor);
                    count++;
                }
            }

            void DeleteSFB()
            {
                //Wait until Navigation tree is set to foreground
                SetForeground(hNavTree);

                //Move to POU
                sw.Reset();
                sw.Start();
                while (node.Text != "POU")
                {
                    if (sw.ElapsedMilliseconds > 5000) { throw new TimeoutException("Failed to move to POU in Navigation pane"); }
                    SendKeyWrapper("{DOWN}");
                    hCursor = NativeMethods.SendMessage(hNavTree, (int)NativeMethods.TVM.TVM_GETNEXTITEM, (int)NativeMethods.TVGN.TVGN_CARET, 0);
                    node = AllocTest(p[0], hNavTree, hCursor);
                }

                //Expand POU
                SendKeyWrapper("{RIGHT}");

                //Move to FB_Pool
                sw.Reset();
                sw.Start();
                while (node.Text != "FB_Pool")
                {
                    if (sw.ElapsedMilliseconds > 5000) { throw new TimeoutException("Failed to move to FB_Pool in Navigation pane"); }
                    SendKeyWrapper("{DOWN}");
                    hCursor = NativeMethods.SendMessage(hNavTree, (int)NativeMethods.TVM.TVM_GETNEXTITEM, (int)NativeMethods.TVGN.TVGN_CARET, 0);
                    node = AllocTest(p[0], hNavTree, hCursor);
                }

                //Expand FB_Pool
                SendKeyWrapper("{RIGHT}");

                //Select SFB routines
                SendKeyWrapper("{DOWN}");
                for (int i = 0; i < count - 1; i++)
                {
                    SendKeyWrapper("+{DOWN}");
                }

                //Delete all SFB routines
                SendKeyWrapper("{DELETE}");

                //Wait for popup window
                FindGXWindowCaption("Do you want to delete the selected file?");

                //Confirm deletion of all SFB routines
                SendKeyWrapper("{LEFT}");
                SendKeyWrapper("{ENTER}");
                SendKeyWrapper("{ENTER}");

                //Wait for popup window
                FindGXWindowCaption("Completed to delete the function blocks in programs.");

                //Close popup window
                SendKeyWrapper("{ENTER}");
                SendKeyWrapper("{ENTER}");

                //Wait for popup to disappear
                WaitForWindowClose(IntPtr.Zero, "FB Program Synchronization");
            }

            void Compile()
            {
                //Compile
                SendKeyWrapper("%");
                SendKeyWrapper("c");
                SendKeyWrapper("r");

                //Wait for popup window
                FindGXWindowCaption("Caution");

                //Uncheck box for 'check for duplicated coils' and confirm.
                SendKeyWrapper("{TAB}");
                SendKeyWrapper("{TAB}");
                SendKeyWrapper(" ");
                SendKeyWrapper("y");
            }

            void ExportToGXDev()
            {
                FindGXWindowCaption("Rebuild All");
                WaitForWindowClose(IntPtr.Zero, "Rebuild All");

                //Export
                SendKeyWrapper("%");
                SendKeyWrapper("p");
                SendKeyWrapper("x");

                //Wait for popup window
                FindGXWindowCaption("Export to GX Developer Format File");
                IntPtr hTemp = NativeMethods.FindWindowCaption(IntPtr.Zero, "Export to GX Developer Format File");

                hTemp = WaitForWindowOpen(hTemp, "ComboBoxEx32");
                hTemp = WaitForWindowOpen(hTemp, "ComboBox");
                IntPtr hWndSave = WaitForWindowOpen(hTemp, "Edit");

                //Wait until file name window is set to foreground
                SetForeground(hWndSave);
                NativeMethods.SendMessage(hWndSave, NativeMethods.WM_SETTEXT, IntPtr.Zero, fileName + "_GXDev");

                //Save
                SendKeyWrapper("{ENTER}");

                //Wait for popup window
                FindGXWindowCaption("Do you want to save GX Developer project?");

                //Confirm save
                SendKeyWrapper("{LEFT}");
                SendKeyWrapper("{ENTER}");

                //Wait for popup window
                FindGXWindowCaption("Project saving has completed.");

                SendKeyWrapper("{ENTER}");
            }

            void WaitForSavePopup()
            {
                //Wait for popup window
                FindGXWindowCaption("Do you want to save the project?");

                //Don't save GX Works 2 file
                SendKeyWrapper("n");
            }

            void DeleteResources()
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                DeleteFiles(desktopPath + @"\" + fileName + @"_GXDev\Resource\POU\Body", ".WPG");
                DeleteFiles(desktopPath + @"\" + fileName + @"_GXDev\Resource\Others", ".WCD");
            }
        }
        public static void SendKeyWrapper(string keys)
        {
            SendKeys.SendWait(keys);
            Thread.Sleep(100);
        }

        public static void WaitUntil(Func<bool> method)
        {
            Stopwatch swX = Stopwatch.StartNew();

            while (swX.ElapsedMilliseconds < 5000)
            {
                if (method())
                {
                    return;
                }
                Thread.Sleep(100);
            }
            throw new TimeoutException("Failed at the WaitUntil command");
        }

        public static void FindGXWindowCaption(string caption)
        {
            IntPtr hWnd = IntPtr.Zero;
            Stopwatch sw = Stopwatch.StartNew();

            while (sw.ElapsedMilliseconds < 10000)
            {
                hWnd = NativeMethods.FindWindowCaption(IntPtr.Zero, "MELSOFT Series GX Works2");
                if (GetChildrenText(hWnd).Any(s => s.Contains(caption)))
                {
                    return;
                }
                Thread.Sleep(100);
            }
            throw new TimeoutException("Failed to find the window that contains " + caption);
        }

        public static void SetForeground(IntPtr hWnd)
        {
            Stopwatch sw = Stopwatch.StartNew();

            while (sw.ElapsedMilliseconds < 5000)
            {
                NativeMethods.SetForegroundWindow(hWnd);

                if (NativeMethods.GetForegroundWindow() == hWnd)
                {
                    return;
                }
                Thread.Sleep(100);
            }
            throw new TimeoutException("Failed to set window to foreground");
        }

        ///<summary>Deletes all files in path except extensions defined with KeepThisExtension.</summary>
        public static void DeleteFiles(string path, string KeepThisExtension)
        {
            string[] files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                //check if files are currently in use and wait until they are not
                WaitUntil(() => !IsFileLocked(new FileInfo(file)));

                if (!file.Contains(KeepThisExtension))
                {
                    File.Delete(file);
                }
            }
        }

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        ///<summary>Waits for a window to open containing either class or caption text.</summary>
        public static IntPtr WaitForWindowOpen(IntPtr parent, string text)
        {
            IntPtr hWnd = IntPtr.Zero;
            List<string> list = new List<string>();
            Stopwatch sw = Stopwatch.StartNew();

            while (sw.ElapsedMilliseconds < 10000)
            {
                hWnd = NativeMethods.FindWindowClass(parent, text);
                if (hWnd != IntPtr.Zero)
                {
                    return hWnd;
                }
                hWnd = NativeMethods.FindWindowCaption(parent, text);
                if (hWnd != IntPtr.Zero)
                {
                    return hWnd;
                }
                Thread.Sleep(100);
            }
            throw new TimeoutException("Failed to find the window that contains " + text);
        }

        public static IntPtr WaitForWindowOpen(string parent, string text)
        {
            //find first instance of parent
            IntPtr hWnd = NativeMethods.FindWindowEx(IntPtr.Zero, IntPtr.Zero, parent, null);
            Stopwatch sw = Stopwatch.StartNew();

            while (sw.ElapsedMilliseconds < 10000)
            {
                //Check children for text
                List<string> listOfChildrenText = GetChildrenText(hWnd);
                bool found = GetChildrenText(hWnd).Any(s => s.Contains(text));

                if (found == true)
                {
                    return hWnd;
                }

                //if not found, index to next parent
                hWnd = NativeMethods.FindWindowEx(IntPtr.Zero, hWnd, parent, null);

                if (hWnd == IntPtr.Zero)
                {
                    //find first instance of parent
                    hWnd = NativeMethods.FindWindowEx(IntPtr.Zero, IntPtr.Zero, parent, null);
                }
                Thread.Sleep(100);
            }
            throw new TimeoutException("Failed to find the window that contains " + text);
        }

        ///<summary>Waits for a window to close containing caption text.</summary>
        public static void WaitForWindowClose(IntPtr parent, string captionText)
        {
            WaitUntil(() => NativeMethods.FindWindowCaption(parent, captionText) == IntPtr.Zero);
        }


        ///<summary>Gets all children text from a parent handle</summary>
        public static List<string> GetChildrenText(IntPtr parentHndl)
        {
            List<string> childrenText = new List<string>();
            List<IntPtr> childrenHndl = GetChildWindows(parentHndl);

            for (int i = 0; i < childrenHndl.Count; i++)
            {
                StringBuilder sb = new StringBuilder(260);
                NativeMethods.SendMessage(childrenHndl[i], NativeMethods.WM_GETTEXT, 260, sb);
                childrenText.Add(sb.ToString());
            }

            return childrenText;
        }

        ///<summary>Gets all children window handles</summary>
        public static List<IntPtr> GetChildWindows(IntPtr parent)
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

        public static bool EnumWindow(IntPtr handle, IntPtr pointer)
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

        ///<summary>Returns the tree node information from another process.</summary>
        ///<param name="hwndItem">Handle to a tree node item.</param>
        ///<param name="hwndTreeView">Handle to a tree view control.</param>
        ///<param name="process">Process hosting the tree view control.</param>
        public static NodeData AllocTest(Process process, IntPtr hwndTreeView, IntPtr hwndItem)
        {
            // code based on article posted here: http://www.codingvision.net/miscellaneous/c-inject-a-dll-into-a-process-w-createremotethread

            // handle of the process with the required privileges
            IntPtr procHandle = NativeMethods.OpenProcess(NativeMethods.PROCESS_CREATE_THREAD | NativeMethods.PROCESS_QUERY_INFORMATION | NativeMethods.PROCESS_VM_OPERATION | NativeMethods.PROCESS_VM_WRITE | NativeMethods.PROCESS_VM_READ, false, process.Id);

            // Write TVITEM to memory
            // Invoke TVM_GETITEM
            // Read TVITEM from memory

            var item = new NativeMethods.TVITEMEX();
            item.hItem = hwndItem;
            item.mask = (int)(NativeMethods.TVIF.TVIF_HANDLE | NativeMethods.TVIF.TVIF_CHILDREN | NativeMethods.TVIF.TVIF_TEXT);
            item.cchTextMax = 1024;
            item.pszText = NativeMethods.VirtualAllocEx(procHandle, IntPtr.Zero, (uint)item.cchTextMax, NativeMethods.MEM_COMMIT | NativeMethods.MEM_RESERVE, NativeMethods.PAGE_READWRITE); // node text pointer

            byte[] data = getBytes(item);

            uint dwSize = (uint)data.Length;
            IntPtr allocMemAddress = NativeMethods.VirtualAllocEx(procHandle, IntPtr.Zero, dwSize, NativeMethods.MEM_COMMIT | NativeMethods.MEM_RESERVE, NativeMethods.PAGE_READWRITE); // TVITEM pointer

            uint nSize = dwSize;
            UIntPtr bytesWritten;
            bool successWrite = NativeMethods.WriteProcessMemory(procHandle, allocMemAddress, data, nSize, out bytesWritten);

            var sm = NativeMethods.SendMessage(hwndTreeView, (int)NativeMethods.TVM.TVM_GETITEM, IntPtr.Zero, allocMemAddress);

            UIntPtr bytesRead;
            bool successRead = NativeMethods.ReadProcessMemory(procHandle, allocMemAddress, data, nSize, out bytesRead);

            UIntPtr bytesReadText;
            byte[] nodeText = new byte[item.cchTextMax];
            bool successReadText = NativeMethods.ReadProcessMemory(procHandle, item.pszText, nodeText, (uint)item.cchTextMax, out bytesReadText);

            bool success1 = NativeMethods.VirtualFreeEx(procHandle, allocMemAddress, dwSize, NativeMethods.MEM_DECOMMIT);
            bool success2 = NativeMethods.VirtualFreeEx(procHandle, item.pszText, (uint)item.cchTextMax, NativeMethods.MEM_DECOMMIT);

            var item2 = fromBytes<NativeMethods.TVITEMEX>(data);

            String name = Encoding.Unicode.GetString(nodeText);
            int x = name.IndexOf('\0');
            if (x >= 0)
                name = name.Substring(0, x);

            NodeData node = new NodeData();
            node.Text = name;
            node.HasChildren = (item2.cChildren == 1);

            return node;
        }

        public class NodeData
        {
            public String Text { get; set; }
            public bool HasChildren { get; set; }
        }

        public static byte[] getBytes(Object item)
        {
            int size = Marshal.SizeOf(item);
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(item, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        public static T fromBytes<T>(byte[] arr)
        {
            T item = default(T);
            int size = Marshal.SizeOf(item);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(arr, 0, ptr, size);
            item = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
            return item;
        }
    }
}
