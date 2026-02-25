using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

/// <summary>
/// Delete or backspace with no previous selection
/// </summary>
internal class SingleSymbolEraseEditAction : EraseEditAction, IEditAction
{
    private ConseqCharType erasedCharsType;
    private int positionBeforeEdit;
    private bool hasSpaceBefore;
    private bool hasSpaceAfter;
    // If explicitly cannot be glued from either of sides:
    private bool sealedFromBeginning;
    private bool sealedFromEnd;

    internal SingleSymbolEraseEditAction(int charsBeforeChange, int charsAfterChange,
        ErasedSubString erasedSubString, int positionBeforeEdit)
        : base(charsBeforeChange, charsAfterChange, erasedSubString.Value)
    {
        this.positionBeforeEdit = positionBeforeEdit;
        this.erasedCharsType = StringUtils.GetCharType(erasedSubString.Value[0]);
        hasSpaceBefore = erasedSubString.HasSpaceBefore;
        hasSpaceAfter = erasedSubString.HasSpaceAfter;
    }

    protected override void RestoreSelection(IEditor editor)
    {
        editor.Selection = new Selection(positionBeforeEdit, 0);
    }

    #region IEditAction

    public string DisplayName => "Erase Text";

    public bool Absorb(IEditAction nextAction)
    {
        if (nextAction is SingleSymbolEraseEditAction next)
        {
            if (next.erasedCharsType == erasedCharsType)
            {
                // Delete
                if ((next.charsBeforeChange == charsBeforeChange)
                    && !sealedFromEnd)
                {
                    erasedSubString += next.erasedSubString;
                    charsAfterChange = next.charsAfterChange;
                    return true;
                }
                // Backspace
                else if ((next.charsAfterChange == charsAfterChange)
                         && !sealedFromBeginning)
                {
                    erasedSubString = next.erasedSubString + erasedSubString;
                    charsBeforeChange = next.charsBeforeChange;
                    return true;
                }
            }
            // If our erasedCharsType is not space, and the next edit is single erased space
            else if ((erasedCharsType != ConseqCharType.Space) // this check is redundant here, but this way it's safer
                     && (next.erasedSubString.Length == 1)
                     && (next.erasedSubString[0] == 32) // should be space PERSONALLY
                     && !next.sealedFromBeginning && !next.sealedFromEnd) // again redundant (cannot be sealed if just 1 character in length)
            {
                // Delete
                if ((next.charsBeforeChange == charsBeforeChange)
                    && !sealedFromEnd
                    && !next.hasSpaceAfter) // should be the only space
                {
                    erasedSubString += next.erasedSubString;
                    charsAfterChange = next.charsAfterChange;
                    sealedFromEnd = true; // No more appending to end, since the end now contains different character type
                    return true;
                }
                // Backspace
                else if ((next.charsAfterChange == charsAfterChange)
                         && !sealedFromBeginning
                         && !next.hasSpaceBefore)
                {
                    erasedSubString = next.erasedSubString + erasedSubString;
                    charsBeforeChange = next.charsBeforeChange;
                    sealedFromBeginning = true;
                    return true;
                }
            }
        }

        return false;
    }

    #endregion
}
