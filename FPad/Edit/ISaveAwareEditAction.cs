using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

internal interface ISaveAwareEditAction
{
    void DocumentSaved();
}
