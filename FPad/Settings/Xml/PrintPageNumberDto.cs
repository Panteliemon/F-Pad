using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FPad.Settings.Xml;

public class PrintPageNumberDto
{
    [XmlAttribute]
    public string Include { get; set; }
    [XmlAttribute]
    public string UseTemplate { get; set; }
    [XmlAttribute]
    public string Template { get; set; }
    [XmlAttribute]
    public string Align { get; set; }
    public FontDto Font { get; set; }
}
