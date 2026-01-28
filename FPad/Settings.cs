using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad;

public class Settings
{
    public string FontFamily { get; set; }
    public int FontSize { get; set; }
    public bool IsBold { get; set; }
    public bool IsItalic { get; set; }
    public bool Wrap { get; set; }

    public bool WindowPositionHasValue { get; set; }
    public int WindowTop { get; set; }
    public int WindowLeft { get; set; }
    public int WindowHeight { get; set; }
    public int WindowWidth { get; set; }
    public bool WindowMaximized { get; set; }

    public static Settings Default()
    {
        return new Settings()
        {
            FontFamily = string.Empty,
            FontSize = 12
        };
    }
}
