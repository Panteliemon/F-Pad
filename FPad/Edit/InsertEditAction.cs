using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

internal abstract class InsertEditAction : SubStringChangeEditAction
{
    /// <summary>
    /// Null if nothing has been erased
    /// </summary>
    protected string erasedSubString;
    protected string insertedSubString;

    internal InsertEditAction(int charsBeforeChange, int charsAfterChange,
        string erasedSubString, string insertedSubString)
        : base(charsBeforeChange, charsAfterChange)
    {
        this.erasedSubString = erasedSubString;

        if (string.IsNullOrEmpty(insertedSubString))
            throw new ArgumentException();
        this.insertedSubString = insertedSubString;
    }

    public void Apply(IEditor editor)
    {
        ReadOnlySpan<char> txtBefore = editor.TextNoUndo;

        StringBuilder sb = new();
        sb.Append(txtBefore[0..charsBeforeChange]);
        sb.Append(insertedSubString);
        sb.Append(txtBefore[^charsAfterChange..]);

        editor.TextNoUndo = sb.ToString();
        SelectAfterApply(editor);
    }

    public void Rollback(IEditor editor)
    {
        ReadOnlySpan<char> txtAfter = editor.TextNoUndo;

        StringBuilder sb = new();
        sb.Append(txtAfter[0..charsBeforeChange]);
        if (erasedSubString != null)
            sb.Append(erasedSubString);
        sb.Append(txtAfter[^charsAfterChange..]);

        editor.TextNoUndo = sb.ToString();
        if (erasedSubString == null)
            editor.Selection = new Selection(charsBeforeChange, 0);
        else
            editor.Selection = new Selection(charsBeforeChange, erasedSubString.Length);
    }

    protected abstract void SelectAfterApply(IEditor editor);
}
