using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Settings.Print;

public class PrintSettings
{
    public FontSettings FileNameFont { get; set; }
    public FileNameContent FileNameContent { get; set; }

    public FontSettings PageNumberFont { get; set; }
    public PageNumberContent PageNumberContent { get; set; }
    public HorizontalAlignment PageNumberAlignment { get; set; }
    public string PageNumberTemplate { get; set; }

    public static PrintSettings Default()
    {
        return new PrintSettings()
        {
            FileNameFont = new FontSettings()
            {
                Family = "Georgia",
                Size = 12
            },
            FileNameContent = FileNameContent.None,
            PageNumberFont = new FontSettings()
            {
                Family = "Georgia",
                Size = 12
            },
            PageNumberContent = PageNumberContent.None,
            PageNumberTemplate = "{page} / {total}",
            PageNumberAlignment = HorizontalAlignment.Center
        };
    }
}
