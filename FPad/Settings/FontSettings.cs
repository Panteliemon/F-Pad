using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Settings;

public class FontSettings
{
    public const int MIN_SIZE = 5;
    public const int MAX_SIZE = 128;

    public string Family { get; set; }
    public int Size { get; set; }
    public bool IsBold { get; set; }
    public bool IsItalic { get; set; }

    public FontSettings Clone()
    {
        return (FontSettings)MemberwiseClone();
    }
}
