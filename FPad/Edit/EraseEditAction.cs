using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

internal abstract class EraseEditAction : SubStringChangeEditAction
{
    protected string erasedSubString;

    internal EraseEditAction(int charsBeforeChange, int charsAfterChange,
        string erasedSubString) : base(charsBeforeChange, charsAfterChange)
    {
        if (string.IsNullOrEmpty(erasedSubString))
            throw new ArgumentException();
        this.erasedSubString = erasedSubString;
    }

    public void Apply(IEditor editor)
    {
        ReadOnlySpan<char> txtBefore = editor.TextNoUndo;

        StringBuilder sb = new();
        sb.Append(txtBefore[0..charsBeforeChange]);
        sb.Append(txtBefore[^charsAfterChange..]);

        editor.TextNoUndo = sb.ToString();
        editor.Selection = new Selection(charsBeforeChange, 0);
    }

    public void Rollback(IEditor editor)
    {
        ReadOnlySpan<char> txtAfter = editor.TextNoUndo;
        StringBuilder sb = new();
        sb.Append(txtAfter[0..charsBeforeChange]);
        sb.Append(erasedSubString);
        sb.Append(txtAfter[^charsAfterChange..]);

        editor.TextNoUndo = sb.ToString();
        RestoreSelection(editor);
    }

    protected abstract void RestoreSelection(IEditor editor);
}
