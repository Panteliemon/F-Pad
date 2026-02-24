using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

public class UndoManager
{
    /// <summary>
    /// From earliest to the newest
    /// </summary>
    private List<IEditAction> actions = new();
    /// <summary>
    /// Index which next added action will have
    /// </summary>
    private int nextActionIndex;

    public UndoManager()
    {
    }

    // Current state
    public bool CanUndo => nextActionIndex > 0;
    public string UndoActionName => (nextActionIndex > 0) ? actions[nextActionIndex - 1].DisplayName : null;
    public bool CanRedo => nextActionIndex < actions.Count;
    public string RedoActionName => (nextActionIndex < actions.Count) ? actions[nextActionIndex].DisplayName : null;

    public void TakeNewAction(IEditAction action)
    {
        // Try glue to the topmost
        if ((nextActionIndex > 0)
            // Only if we didn't undo right before that
            && (nextActionIndex == actions.Count)
            && actions[nextActionIndex - 1].Absorb(action))
        {
            return;
        }

        // All redo is invalidated
        if (actions.Count > nextActionIndex)
            actions.RemoveRange(nextActionIndex, actions.Count - nextActionIndex);

        actions.Add(action);
        nextActionIndex = actions.Count;
    }

    public void Undo(IEditor editor)
    {
        if (nextActionIndex > 0)
        {
            actions[nextActionIndex - 1].Rollback(editor);
            nextActionIndex--;
        }
    }

    public void Redo(IEditor editor)
    {
        if (nextActionIndex < actions.Count)
        {
            actions[nextActionIndex].Apply(editor);
            nextActionIndex++;
        }
    }

    /// <summary>
    /// Reset all Undo
    /// </summary>
    public void Reset()
    {
        actions.Clear();
        nextActionIndex = 0;
    }
}
