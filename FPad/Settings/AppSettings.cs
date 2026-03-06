using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Settings;

public class AppSettings
{
    public FontSettings Font { get; set; }
    public bool Wrap { get; set; }
    public bool FindMatchCase { get; set; }
    public bool FindWholeWords { get; set; }
    public bool AutoReload { get; set; }
    public string DefaultEncodingWebName { get; set; }

    /// <summary>
    /// "Default" window position
    /// </summary>
    public WindowPositionSettings WindowPosition { get; set; }

    public List<FileSettings> Files { get; set; }

    public static AppSettings Default()
    {
        return new AppSettings()
        {
            Font = FontSettings.Default(),
            AutoReload = true
        };
    }
}
