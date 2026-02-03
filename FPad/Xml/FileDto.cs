using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FPad.Xml;

public class FileDto
{
    [XmlAttribute]
    public string Path { get; set; }
    [XmlAttribute]
    public int Date { get; set; }

    public WindowPositionDto WindowPosition { get; set; }
}
