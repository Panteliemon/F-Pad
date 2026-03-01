using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad.Controls;

public class UacIconBehavior : IDisposable
{
    private const int BCM_FIRST = 0x1600;
    private const int BCM_SETSHIELD = BCM_FIRST + 0x000C;

    private Button button;

    public UacIconBehavior(Button button)
    {
        this.button = button;
        WinApi.SendMessageW(button.Handle, BCM_SETSHIELD, 0, 1);
    }

    public void Dispose()
    {
        if (button != null)
        {
            WinApi.SendMessageW(button.Handle, BCM_SETSHIELD, 0, 0);
            button = null;
        }
    }
}
