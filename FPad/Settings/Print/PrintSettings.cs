using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Settings.Print;

public class PrintSettings
{
    public bool IncludeFileName { get; set; }
    public FileNameContent FileNameContent { get; set; }
    public FontSettings FileNameFont { get; set; }

    public bool IncludePageNumber { get; set; }
    public bool UsePageNumberTemplate { get; set; }
    public string PageNumberTemplate { get; set; }
    public HorizontalAlignment PageNumberAlignment { get; set; }
    public FontSettings PageNumberFont { get; set; }
    
    public static PrintSettings Default()
    {
        return new PrintSettings()
        {
            FileNameContent = FileNameContent.NameExt,
            FileNameFont = new FontSettings()
            {
                Family = "Georgia",
                Size = 12
            },
            PageNumberTemplate = "{page} / {total}",
            PageNumberAlignment = HorizontalAlignment.Center,
            PageNumberFont = new FontSettings()
            {
                Family = "Georgia",
                Size = 12
            }
        };
    }
}
