using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FPad.Settings.Xml;

public class GeneralDto
{
    [XmlAttribute]
    public string AutoReload { get; set; }
}
