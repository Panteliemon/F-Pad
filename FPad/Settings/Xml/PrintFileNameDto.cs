using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FPad.Settings.Xml;

public class PrintFileNameDto
{
    [XmlAttribute]
    public string Include { get; set; }
    [XmlAttribute]
    public string Option { get; set; }
    public FontDto Font { get; set; }
}
