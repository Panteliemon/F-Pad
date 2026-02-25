using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

internal interface IModifyingEditAction
{
    /// <summary>
    /// True if this action raises "unsaved changes" flag (99% of actions do that)
    /// </summary>
    bool IsModifying { get; }
}
