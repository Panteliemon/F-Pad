using FPad.Encodings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

/// <summary>
/// Editor's actions for Undo/Redo functionality
/// </summary>
public interface IEditor
{
    string Text { get; }
    /// <summary>
    /// Sets text without generating new Undo entries
    /// </summary>
    void SetTextNoUndo(string value, bool raiseModifiedFlag = true);
    Selection Selection { get; set; }
    EncodingVm Encoding { get; set; }
}
