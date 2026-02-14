using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Interaction;

internal enum MessageType : byte
{
    Activate = 1,
    ActivateSetCaret = 2
}
