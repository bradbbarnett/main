using System.Runtime.InteropServices;

namespace AutomationAPI
{
    public static class MazakLibrary
    {
        #region Error Code Return Values

        public const short MAZERR_OK = 0;
        public const short MAZERR_SOCK = -10;
        public const short MAZERR_HNDL = -20;
        public const short MAZERR_CLIMAX = -21;
        public const short MAZERR_SERVERMAX = -22;
        public const short MAZERR_VER = -30;
        public const short MAZERR_BUSY = -40;
        public const short MAZERR_RUNNING = -50;
        public const short MAZERR_OVER = -51;
        public const short MAZERR_NONE = -52;
        public const short MAZERR_TYPE = -53;
        public const short MAZERR_EDIT = -54;
        public const short MAZERR_PROSIZE = -55;
        public const short MAZERR_PRONUM = -56;
        public const short MAZERR_RESTARTSEACH = -57;
        public const short MAZERR_RUNMODE = -58;
        public const short MAZERR_DISPLAY = -59;
        public const short MAZERR_ARG = -60;
        public const short MAZERR_VALUE = -61;
        public const short MAZERR_OPTION = -62;
        public const short MAZERR_SET_TDATA = -64;
        public const short MAZERR_SYS = -70;
        public const short MAZERR_FUNC = -80;
        public const short MAZERR_TIMEOUT = -90;
        public const short MAZERR_AXIS = -100;

        #endregion

        #region DLL Import Functions

        [DllImport("NTIFDLL.dll", EntryPoint = "MazConnect")]
        public static extern int MazConnect(out ushort hndl, string ipaddress, ushort port, ushort timeout);

        [DllImport("NTIFDLL.dll", EntryPoint = "MazDisconnect")]
        public static extern int MazDisconnect(ushort hndl);

        [DllImport("NTIFDLL.dll", EntryPoint = "MazGetAlarm")]
        public static extern int MazGetAlarm(ushort hndl, out MAZ_ALARMALL alarm);

        [DllImport("NTIFDLL.dll", EntryPoint = "MazGetToolLife")]
        public static extern int MazGetToolLife(ushort hndl, ushort head, int tno, out MAZ_TLIFEALL data);

        [DllImport("NTIFDLL.dll", EntryPoint = "MazGetCurrentTool")]
        public static extern int MazGetCurrentTool(ushort hndl, ushort head, out MAZ_TOOLINFO tool);

        [DllImport("NTIFDLL.dll", EntryPoint = "MazGetMagazineToolNum")]
        public static extern int MazGetMagazineToolNum(ushort hndl, ushort head, out int num);

        #endregion
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MAZ_ALARMALL
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public MAZ_ALARM[] alarm;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MAZ_ALARM
    {
        public short eno;       //Error no.

        public byte sts;        //Status

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        string dummy;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string pa1;      //Detailed code 1

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string pa2;      //Detailed code 2

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string pa3;      //Detailed code 3

        public byte mon;        //Month

        public byte day;        //Day

        public byte hor;        //Hour

        public byte min;        //Minute

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        string dummy2;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string extmes;   //Interfering part

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string head;     //Display of system

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        string dummy3;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string msg;      //Alarm message
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MAZ_TLIFEALL
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public MAZ_TLIFE[] tLife;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MAZ_TLIFE
    {
        public MAZ_TOOLINFO info;       //Tool info
        public int sts;                 //Status
        public int lif;                 //Lifetime
        public int use;                 //Use time
        public int clif;                //Lifetime number
        public int cuse;                //Use number

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        string dummy;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MAZ_TOOLINFO
    {
        public ushort tno;     //Tool number
        public byte suf;       //Suffix
        public byte sufatr;    //Suffix attribute
        public byte name;      //Tool name
        public byte part;      //Part

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string sts;     //Nominal valid flag

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        string dummy;

        public int yob;        //Nominal size (turning) or nominal diameter (milling)

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        string dummy2;
    }
}