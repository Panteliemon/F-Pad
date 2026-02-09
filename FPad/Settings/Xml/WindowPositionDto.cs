using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FPad.Settings.Xml;

public class WindowPositionDto
{
    [XmlAttribute]
    public int Top { get; set; }
    [XmlAttribute]
    public int Left { get; set; }
    [XmlAttribute]
    public int Height { get; set; }
    [XmlAttribute]
    public int Width { get; set; }
    [XmlAttribute]
    public string Maximized { get; set; }

}
