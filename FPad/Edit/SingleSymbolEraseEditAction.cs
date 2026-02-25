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

    internal SingleSymbolEraseEditAction(int charsBeforeChange, int charsAfterChange,
        string erasedSubString, int positionBeforeEdit)
        : base(charsBeforeChange, charsAfterChange, erasedSubString)
    {
        this.positionBeforeEdit = positionBeforeEdit;
        this.erasedCharsType = StringUtils.GetCharType(erasedSubString[0]);
    }

    protected override void RestoreSelection(IEditor editor)
    {
        editor.Selection = new Selection(positionBeforeEdit, 0);
    }

    #region IEditAction

    public string DisplayName => "Erase Text";

    public bool Absorb(IEditAction nextAction)
    {
        if ((nextAction is SingleSymbolEraseEditAction next)
            && (next.erasedCharsType == erasedCharsType))
        {
            if (next.charsBeforeChange == charsBeforeChange)
            {
                erasedSubString += next.erasedSubString;
                charsAfterChange = next.charsAfterChange;
                return true;
            }
            else if (next.charsAfterChange == charsAfterChange)
            {
                erasedSubString = next.erasedSubString + erasedSubString;
                charsBeforeChange = next.charsBeforeChange;
                return true;
            }
        }

        return false;
    }

    #endregion
}
