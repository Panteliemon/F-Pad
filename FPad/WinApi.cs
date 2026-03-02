using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FPad;

public static class WinApi
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

    [DllImport("winspool.drv", CharSet = CharSet.Unicode)]
    public static extern bool OpenPrinterW(
        string pPrinterName,
        out nint phPrinter,
        nint pDefault
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
