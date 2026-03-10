using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad;

[Flags]
public enum LineBreaks
{
    None = 0,
    Windows = 1,
    Unix = 2,
    Mixed = 3
}
