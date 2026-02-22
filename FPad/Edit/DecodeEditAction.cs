using FPad.Encodings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

internal class DecodeEditAction : IEditAction
{
    private string textBefore;
    private string textAfter;
    private Selection selectionBefore;
    private EncodingVm encodingBefore;
    private EncodingVm encodingAfter;

    public DecodeEditAction(string textBefore, EncodingVm encodingBefore, Selection selectionBefore,
        string textAfter, EncodingVm encodingAfter)
    {
        this.textBefore = textBefore;
        this.textAfter = textAfter;
        this.selectionBefore = selectionBefore;
        this.encodingBefore = encodingBefore;
        this.encodingAfter = encodingAfter;
    }

    #region IEditAction

    public string DisplayName => "Decode";

    public void Apply(IEditor editor)
    {
        editor.TextNoUndo = textAfter;
        editor.Selection = new Selection(0, 0);
        editor.Encoding = encodingAfter;
    }

    public void Rollback(IEditor editor)
    {
        editor.TextNoUndo = textBefore;
        editor.Selection = selectionBefore;
        editor.Encoding = encodingBefore;
    }

    public bool Absorb(IEditAction nextAction)
    {
        return false;
    }

    #endregion
}
