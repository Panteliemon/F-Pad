using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Settings;

public class FontSettings
{
    public string Family { get; set; }
    public int Size { get; set; }
    public bool IsBold { get; set; }
    public bool IsItalic { get; set; }

    public static FontSettings Default()
    {
        return new FontSettings()
        {
            Family = string.Empty,
            Size = 12
        };
    }
}
