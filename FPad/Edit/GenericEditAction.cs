using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

/// <summary>
/// Used if we failed to determine what exactly has happened.
/// </summary>
internal class GenericEditAction : SubStringChangeEditAction, IEditAction
{
    private string erasedSubString;
    private string insertedSubString;
    private Selection selectionBefore;
    private Selection selectionAfter;

    internal GenericEditAction(int charsBeforeChange, int charsAfterChange,
        string erasedSubString, string insertedSubString,
        Selection selectionBefore, Selection selectionAfter)
        : base(charsBeforeChange, charsAfterChange)
    {
        this.erasedSubString = erasedSubString;
        this.insertedSubString = insertedSubString;
        this.selectionBefore = selectionBefore;
        this.selectionAfter = selectionAfter;
    }

    #region IEditAction

    public string DisplayName => "Edit";

    public void Apply(IEditor editor)
    {
        ReadOnlySpan<char> txtBefore = editor.Text;

        StringBuilder sb = new();
        sb.Append(txtBefore[0..charsBeforeChange]);
        sb.Append(insertedSubString);
        sb.Append(txtBefore[^charsAfterChange..]);

        editor.SetTextNoUndo(sb.ToString());
        editor.Selection = selectionAfter;
    }

    public void Rollback(IEditor editor)
    {
        ReadOnlySpan<char> txtAfter = editor.Text;

        StringBuilder sb = new();
        sb.Append(txtAfter[0..charsBeforeChange]);
        sb.Append(erasedSubString);
        sb.Append(txtAfter[^charsAfterChange..]);

        editor.SetTextNoUndo(sb.ToString());
        editor.Selection = selectionBefore;
    }

    public bool Absorb(IEditAction nextAction)
    {
        return false;
    }

    #endregion
}
