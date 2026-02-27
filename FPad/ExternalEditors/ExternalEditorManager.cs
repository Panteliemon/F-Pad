using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPad.ExternalEditors;

internal class ExternalEditorManager
{
    /// <summary>
    /// Detect all supported external editors. Raises <see cref="ExternalEditorsChanged"/> as it goes.
    /// </summary>
    /// <param name="callback">Called when another editor gets detected</param>
    /// <param name="ct"></param>
    /// <exception cref="OperationCanceledException">Thrown if canceled</exception>
    public void Load(Action<IExternalEditor> callback, CancellationToken ct)
    {
        List<IExternalEditor> editorsToDetect = [new NotepadPlusPlus(), new VSCode(), new SublimeText()];
        // Order by alphabetical order in user's current culture
        foreach (IExternalEditor editor in editorsToDetect.OrderBy(x => x.DisplayName))
        {
            TryAdd(editor, callback, ct);
        }
    }

    private void TryAdd(IExternalEditor editor, Action<IExternalEditor> callback, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (editor.Detect(ct))
        {
            ct.ThrowIfCancellationRequested();

            if (callback != null)
            {
                callback(editor);
            }
        }
    }
}
