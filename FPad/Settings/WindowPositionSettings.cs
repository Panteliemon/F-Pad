using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Settings;

public class WindowPositionSettings
{
    public int Top { get; set; }
    public int Left { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public bool IsMaximized { get; set; }

    public WindowPositionSettings Clone()
    {
        return (WindowPositionSettings)MemberwiseClone();
    }
}
