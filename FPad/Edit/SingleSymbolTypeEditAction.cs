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
    private int nonGluableCharsAtBeginning;

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
        if (nextAction is SingleSymbolTypeEditAction next)
        {
            if ((next.insertedCharsType == insertedCharsType)
                && string.IsNullOrEmpty(next.erasedSubString)
                && (next.charsBeforeChange >= charsBeforeChange + nonGluableCharsAtBeginning)
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

            // Here is the deal: Ctrl+Z which removes one symbol at a time - SUCKS!
            // Let's handle special case: if we type text strictly forward,
            // and we use single space between words like normal people do,
            // then glue this single space to the upcoming word, so they are rolled back together.
            if (string.IsNullOrEmpty(erasedSubString)
                && (insertedSubString.Length == 1) // only one, so line breaks are not glued to words
                && (insertedSubString[0] == 32) // PERSONALLY space
                && (positionAfterEdit == charsBeforeChange + 1)
                && string.IsNullOrEmpty(next.erasedSubString)
                && (next.insertedSubString.Length == 1)
                && (next.insertedCharsType != ConseqCharType.Space)
                && (next.charsBeforeChange == positionAfterEdit)
                && (next.positionAfterEdit == next.charsBeforeChange + 1))
            {
                insertedSubString = insertedSubString + next.insertedSubString;
                positionAfterEdit = next.positionAfterEdit;
                insertedCharsType = next.insertedCharsType; // change flavor
                nonGluableCharsAtBeginning = 1; // so other letters are not glued before this space
                
                return true;
            }
        }

        return false;
    }

    #endregion
}
