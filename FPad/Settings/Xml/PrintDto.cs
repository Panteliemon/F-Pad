using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Settings.Xml;

public class PrintDto
{
    public PrintFileNameDto FileName { get; set; }
    public PrintPageNumberDto PageNumber { get; set; }
}
