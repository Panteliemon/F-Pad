using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

internal class LineBreaksEditAction : IEditAction, IModifyingEditAction, ISaveAwareEditAction
{
    private LineBreaks lineBreaksBefore;
    private LineBreaks lineBreaksAfter;
    private bool raiseModifiedFlag;

    public LineBreaksEditAction(LineBreaks lineBreaksBefore, LineBreaks lineBreaksAfter,
        bool raiseModifiedFlag)
    {
        this.lineBreaksBefore = lineBreaksBefore;
        this.lineBreaksAfter = lineBreaksAfter;
        this.raiseModifiedFlag = raiseModifiedFlag;
    }

    public bool IsModifying => raiseModifiedFlag;

    #region IEditAction

    public string DisplayName => "Line Breaks";

    public void Apply(IEditor editor)
    {
        editor.SetLineBreaks(lineBreaksAfter, raiseModifiedFlag);
    }

    public void Rollback(IEditor editor)
    {
        editor.SetLineBreaks(lineBreaksBefore, raiseModifiedFlag);
    }

    public bool Absorb(IEditAction nextAction)
    {
        // Don't glue. Because I said so.
        return false;
    }

    #endregion

    public void DocumentSaved()
    {
        raiseModifiedFlag = true;
    }
}
