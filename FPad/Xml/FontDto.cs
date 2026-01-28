using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FPad.Xml;

public class FontDto
{
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public int Size { get; set; }
    [XmlAttribute]
    public string IsBold { get; set; }
    [XmlAttribute]
    public string IsItalic { get; set; }
}
