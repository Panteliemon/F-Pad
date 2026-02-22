using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

internal class ReplaceEditAction : InsertEditAction, IEditAction
{
    internal ReplaceEditAction(int charsBeforeChange, int charsAfterChange,
        string erasedSubString, string insertedSubString)
        : base(charsBeforeChange, charsAfterChange, erasedSubString, insertedSubString)
    {
    }

    protected override void SelectAfterApply(IEditor editor)
    {
        editor.Selection = new Selection(charsBeforeChange, insertedSubString.Length);
    }

    #region IEditAction

    public string DisplayName => "Replace";

    public bool Absorb(IEditAction nextAction)
    {
        return false;
    }

    #endregion
}
