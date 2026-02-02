using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Settings;

public class AppSettings
{
    public string FontFamily { get; set; }
    public int FontSize { get; set; }
    public bool IsBold { get; set; }
    public bool IsItalic { get; set; }
    public bool Wrap { get; set; }

    public WindowPositionSettings WindowPosition { get; set; }

    public static AppSettings Default()
    {
        return new AppSettings()
        {
            FontFamily = string.Empty,
            FontSize = 12
        };
    }
}
