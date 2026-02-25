using FPad.Encodings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

internal class EncodingForSaveEditAction : IEditAction, ISaveAwareEditAction
{
    private EncodingVm encodingBefore;
    private EncodingVm encodingAfter;
    private bool raiseModifiedFlag;

    internal EncodingForSaveEditAction(EncodingVm encodingBefore, EncodingVm encodingAfter,
        bool raiseModifiedFlag)
    {
        this.encodingBefore = encodingBefore;
        this.encodingAfter = encodingAfter;
        this.raiseModifiedFlag = raiseModifiedFlag;
    }

    #region IEditAction

    public string DisplayName => "Encoding";
    public bool IsModifying => raiseModifiedFlag;

    public void Apply(IEditor editor)
    {
        editor.SetEncoding(encodingAfter, raiseModifiedFlag);
    }

    public void Rollback(IEditor editor)
    {
        editor.SetEncoding(encodingBefore, raiseModifiedFlag);
    }

    public bool Absorb(IEditAction nextAction)
    {
        if (nextAction is EncodingForSaveEditAction next
            && next.raiseModifiedFlag == raiseModifiedFlag
            && next.encodingAfter != encodingBefore)
        {
            encodingAfter = next.encodingAfter;
            return true;
        }

        return false;
    }

    #endregion

    public void DocumentSaved()
    {
        raiseModifiedFlag = true;
    }
}
