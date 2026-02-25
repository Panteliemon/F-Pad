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
    /// Index which next added action will have (position between undos and redos)
    /// </summary>
    private int nextActionIndex;
    /// <summary>
    /// Position between actions which constitutes "No Changes" state.
    /// </summary>
    private int indexWhenSaved;

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
        if (action == null)
            return;

        // All redo is invalidated
        if (actions.Count > nextActionIndex)
        {
            actions.RemoveRange(nextActionIndex, actions.Count - nextActionIndex);

            // If "no unsaved changes" state was in future - lose it.
            if (indexWhenSaved > nextActionIndex)
                indexWhenSaved = -1;
        }

        // Try to glue to the topmost
        if ((nextActionIndex > 0)
            // Only if we didn't undo right before that
            && (nextActionIndex == actions.Count)
            && actions[nextActionIndex - 1].Absorb(action))
        {
            // If we were in "saved" state - then absorbing new action will
            // invalidate this state (no more possible to land to "no unsaved changes" via undo/redo)
            if (indexWhenSaved == nextActionIndex)
                indexWhenSaved = -1;

            return;
        }

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
    /// Signalize that the document has been just saved.
    /// This affects how functionality "reset modified flag when all Undo is exhausted" behaves.
    /// </summary>
    public void DocumentSaved()
    {
        // After the document has been saved - all non-modifying "select encoding for save" actions
        // must retroactively become modifying.
        foreach (IEditAction action in actions)
        {
            if (action is ISaveAwareEditAction x)
                x.DocumentSaved();
        }

        // Mark current undo-redo state as state in which the document was saved.
        // Position not at now, but after the latest "modifying" state
        // This way we won't lose the "saved" state if after that the user presses Ctrl+Z
        // and performs another non-modifying action.
        indexWhenSaved = nextActionIndex;
        while (indexWhenSaved > 0)
        {
            if (IsModifying(actions[indexWhenSaved - 1]))
                break;
            else
                indexWhenSaved--;
        } 
    }

    /// <summary>
    /// Tells if the current state is identical* to the one at which the document was saved
    /// (* well not exactly identical, but these margins are too narrow)
    /// </summary>
    /// <returns></returns>
    public bool IsInNoChangesState()
    {
        if (indexWhenSaved >= 0)
        {
            if (nextActionIndex >= indexWhenSaved)
            {
                // Saved in past (most natural case)
                for (int i = indexWhenSaved; i < nextActionIndex; i++)
                {
                    if (IsModifying(actions[i]))
                        return false;
                }
            }
            else
            {
                // Saved in future (someone slammed Ctrl-Z after saving)
                for (int i = nextActionIndex; i < indexWhenSaved; i++)
                {
                    if (IsModifying(actions[i]))
                        return false;
                }
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Reset all Undo
    /// </summary>
    public void Reset()
    {
        actions.Clear();
        nextActionIndex = 0;
        indexWhenSaved = 0;
    }

    private static bool IsModifying(IEditAction action)
    {
        if (action is IModifyingEditAction x)
            return x.IsModifying;
        else
            return true;
    }
}
