using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FPad.Settings.Xml;

public class PrintPageNumberDto
{
    public FontDto Font { get; set; }
    [XmlAttribute]
    public int Option { get; set; }
    [XmlAttribute]
    public string Template { get; set; }
    [XmlAttribute]
    public int Align { get; set; }
}
