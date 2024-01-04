using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LadderCompareV3
{
    public class NativeMethods
    {
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

        public const uint WM_SETTEXT = 0X000C;
        public const uint WM_GETTEXT = 0X000D;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern void SendMessage(IntPtr hWnd, uint Msg, int wParam, [Out] StringBuilder lParam);

        [DllImport("user32.dll")]
        public static extern void SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);

        [DllImport("user32.dll")]
        public static extern void SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern void EnumThreadWindows(int threadId, EnumWindowsProc callback, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern void EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BlockInput(bool fBlockIt);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void GetWindowText(IntPtr hWnd, StringBuilder text, int maxCount);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpClassName, string lpCaptionName);

        /// <summary>Finds window containing text in the class field</summary>
        public static IntPtr FindWindowClass(IntPtr parent, string text)
        {
            return FindWindowEx(parent, IntPtr.Zero, text, null);
        }

        /// <summary>Finds window containing text in the caption field</summary>
        public static IntPtr FindWindowCaption(IntPtr parent, string text)
        {
            return FindWindowEx(parent, IntPtr.Zero, null, text);
        }

        private const int TV_FIRST = 0x1100;
        public enum TVM
        {
            TVM_GETNEXTITEM = (TV_FIRST + 10),
            TVM_GETITEMA = (TV_FIRST + 12),
            TVM_GETITEM = (TV_FIRST + 62),
            TVM_GETCOUNT = (TV_FIRST + 5),
            TVM_SELECTITEM = (TV_FIRST + 11),
            TVM_DELETEITEM = (TV_FIRST + 1),
            TVM_EXPAND = (TV_FIRST + 2),
            TVM_GETITEMRECT = (TV_FIRST + 4),
            TVM_GETINDENT = (TV_FIRST + 6),
            TVM_SETINDENT = (TV_FIRST + 7),
            TVM_GETIMAGELIST = (TV_FIRST + 8),
            TVM_SETIMAGELIST = (TV_FIRST + 9),
            TVM_GETISEARCHSTRING = (TV_FIRST + 64),
            TVM_HITTEST = (TV_FIRST + 17),
        }

        public enum TVGN
        {
            TVGN_ROOT = 0x0,
            TVGN_NEXT = 0x1,
            TVGN_PREVIOUS = 0x2,
            TVGN_PARENT = 0x3,
            TVGN_CHILD = 0x4,
            TVGN_FIRSTVISIBLE = 0x5,
            TVGN_NEXTVISIBLE = 0x6,
            TVGN_PREVIOUSVISIBLE = 0x7,
            TVGN_DROPHILITE = 0x8,
            TVGN_CARET = 0x9,
            TVGN_LASTVISIBLE = 0xA
        }

        [Flags]
        public enum TVIF
        {
            TVIF_TEXT = 1,
            TVIF_IMAGE = 2,
            TVIF_PARAM = 4,
            TVIF_STATE = 8,
            TVIF_HANDLE = 16,
            TVIF_SELECTEDIMAGE = 32,
            TVIF_CHILDREN = 64,
            TVIF_INTEGRAL = 0x0080,
            TVIF_DI_SETITEM = 0x1000
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TVITEMEX
        {
            public uint mask;
            public IntPtr hItem;
            public uint state;
            public uint stateMask;
            public IntPtr pszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
            public int iIntegral;
            public uint uStateEx;
            public IntPtr hwnd;
            public int iExpandedImage;
            public int iReserved;
        }

        // privileges
        public const int PROCESS_CREATE_THREAD = 0x0002;
        public const int PROCESS_QUERY_INFORMATION = 0x0400;
        public const int PROCESS_VM_OPERATION = 0x0008;
        public const int PROCESS_VM_WRITE = 0x0020;
        public const int PROCESS_VM_READ = 0x0010;

        // used for memory allocation
        public const uint MEM_COMMIT = 0x00001000;
        public const int MEM_DECOMMIT = 0x4000;
        public const uint MEM_RESERVE = 0x00002000;
        public const uint PAGE_READWRITE = 4;
    }
}
