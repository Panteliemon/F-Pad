using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FPad.Xml;

[XmlRoot("Settings")]
public class SettingsDto
{
    public FontDto Font { get; set; }
    public TextDto Text { get; set; }
    public WindowPositionDto WindowPosition { get; set; }
    [XmlArrayItem("File")]
    public List<FileDto> Files { get; set; }
}
