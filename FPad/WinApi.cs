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

    [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern void SHChangeNotify(
        uint wEventId,
        uint uFlags,
        nint dwItem1,
        nint dwItem2);
}
