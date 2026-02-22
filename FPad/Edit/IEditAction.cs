using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

public interface IEditAction
{
    string DisplayName { get; }
    void Apply(IEditor editor);
    void Rollback(IEditor editor);
    /// <summary>
    /// Try to combine current action with the next one.
    /// </summary>
    /// <param name="nextAction"></param>
    /// <returns>If true, then <paramref name="nextAction"/> has been
    /// fully incorporated into current object, and can be discarded.</returns>
    bool Absorb(IEditAction nextAction);
}
