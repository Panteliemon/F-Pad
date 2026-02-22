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
    /// <summary>
    /// Setter of this property should not generate Undo entries
    /// </summary>
    string TextNoUndo { get; set; }
    Selection Selection { get; set; }
}
