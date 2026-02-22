using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

/// <summary>
/// Selection erased by backspace or delete
/// </summary>
internal class SelectionEraseEditAction : EraseEditAction, IEditAction
{
    internal SelectionEraseEditAction(int charsBeforeChange, int charsAfterChange,
        string erasedSubString) : base(charsBeforeChange, charsAfterChange, erasedSubString)
    {
    }

    protected override void RestoreSelection(IEditor editor)
    {
        editor.Selection = new Selection(charsBeforeChange, erasedSubString.Length);
    }

    #region IEditAction

    public virtual string DisplayName => "Erase Selection";

    public bool Absorb(IEditAction nextAction)
    {
        return false;
    }

    #endregion
}
