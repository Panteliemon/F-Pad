using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FPad;

internal static class WinApi
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern nint SendMessageW(
        nint hWnd,
        uint Msg,
        nint wParam,
        nint lParam);

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern uint ExtractIconExW(
        string lpszFile,
        int nIconIndex,
        nint[] phiconLarge,
        nint[] phiconSmall,
        uint nIcons
    );

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool DestroyIcon(nint hIcon);

    [Flags]
    public enum PrinterStatus : uint
    {
        Paused = 0x1,
        Error = 0x2,
        PendingDeletion = 0x4,
        PaperJam = 0x8,
        PaperOut = 0x10,
        ManualFeed = 0x20,
        PaperProblem = 0x40,
        Offline = 0x80,
        IOActive = 0x100,
        Busy = 0x200,
        Printing = 0x400,
        OutputBinFull = 0x800,
        NotAvailable = 0x1000,
        Waiting = 0x2000,
        Processing = 0x4000,
        Initializing = 0x8000,
        WarmingUp = 0x10000,
        TonerLow = 0x20000,
        NoToner = 0x40000,
        PagePunt = 0x80000,
        UserIntervention = 0x100000,
        OutOfMemory = 0x200000,
        DoorOpen = 0x400000,
        ServerUnknown = 0x800000,
        PowerSave = 0x1000000,
        ServerOffline = 0x2000000 
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct PRINTER_INFO_2
    {
        public string pServerName;
        public string pPrinterName;
        public string pShareName;
        public string pPortName;
        public string pDriverName;
        public string pComment;
        public string pLocation;
        public nint pDevMode;
        public string pSepFile;
        public string pPrintProcessor;
        public string pDatatype;
        public string pParameters;
        public nint pSecurityDescriptor;
        public uint Attributes;
        public uint Priority;
        public uint DefaultPriority;
        public uint StartTime;
        public uint UntilTime;
        public PrinterStatus Status;
        public uint cJobs;
        public uint AveragePPM;
    }

    [DllImport("winspool.drv", CharSet = CharSet.Unicode)]
    public static extern bool OpenPrinterW(
        string pPrinterName,
        out nint phPrinter,
        nint pDefault
    );

    [DllImport("winspool.drv", CharSet = CharSet.Unicode)]
    public static extern bool GetPrinter(
        nint hPrinter,
        uint Level,
        nint pPrinter,
        uint cbBuf,
        out uint pcbNeeded
    );

    [DllImport("winspool.drv", CharSet = CharSet.Unicode)]
    public static extern int DocumentPropertiesW(
        nint hwnd,
        nint hPrinter,
        string pDeviceName,
        nint pDevModeOutput,
        nint pDevModeInput,
        int fMode
    );

    [DllImport("winspool.drv")]
    public static extern bool ClosePrinter(nint hPrinter);
}
