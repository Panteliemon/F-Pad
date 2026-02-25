using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

/// <summary>
/// Represents all changes which can be described as "some part of the text has changed"
/// </summary>
internal abstract class SubStringChangeEditAction
{
    /// <summary>
    /// Length of common prefix of previous text and new text
    /// </summary>
    protected int charsBeforeChange;
    /// <summary>
    /// Length of common suffix of previous text and new text
    /// </summary>
    protected int charsAfterChange;

    internal SubStringChangeEditAction(int charsBeforeChange, int charsAfterChange)
    {
        this.charsBeforeChange = charsBeforeChange;
        this.charsAfterChange = charsAfterChange;
    }
}
