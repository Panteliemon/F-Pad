using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FPad.Xml;

public class TextDto
{
    [XmlAttribute]
    public string Wrap { get; set; }
}
