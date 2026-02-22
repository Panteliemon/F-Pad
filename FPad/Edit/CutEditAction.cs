using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

internal class CutEditAction : SelectionEraseEditAction
{
    internal CutEditAction(int charsBeforeChange, int charsAfterChange,
        string erasedSubString)
        : base(charsBeforeChange, charsAfterChange, erasedSubString)
    {
    }

    public override string DisplayName => "Cut";

    // When run "Redo" Cut - don't change contents of the clipboard
}
