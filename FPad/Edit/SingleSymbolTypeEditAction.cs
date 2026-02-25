using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

internal class SingleSymbolTypeEditAction : InsertEditAction, IEditAction
{
    private ConseqCharType insertedCharsType;
    private int positionAfterEdit;

    internal SingleSymbolTypeEditAction(int charsBeforeChange, int charsAfterChange,
        string erasedSubString, string insertedSubString, int positionAfterEdit)
        : base(charsBeforeChange, charsAfterChange, erasedSubString, insertedSubString)
    {
        insertedCharsType = StringUtils.GetCharType(insertedSubString[0]);
        this.positionAfterEdit = positionAfterEdit;
    }

    protected override void SelectAfterApply(IEditor editor)
    {
        editor.Selection = new Selection(positionAfterEdit, 0);
    }

    #region IEditAction

    public string DisplayName => "Type";

    public bool Absorb(IEditAction nextAction)
    {
        if ((nextAction is SingleSymbolTypeEditAction next)
            && (next.insertedCharsType == insertedCharsType)
            && string.IsNullOrEmpty(next.erasedSubString)
            && (next.charsBeforeChange >= charsBeforeChange)
            && (next.charsBeforeChange <= charsBeforeChange + insertedSubString.Length))
        {
            int relativePos = next.charsBeforeChange - charsBeforeChange;
            StringBuilder sb = new();
            sb.Append(insertedSubString[0..relativePos]);
            sb.Append(next.insertedSubString);
            sb.Append(insertedSubString[relativePos..]);
            insertedSubString = sb.ToString();
            positionAfterEdit = next.positionAfterEdit;

            return true;
        }

        return false;
    }

    #endregion
}
