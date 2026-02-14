using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPad.ExternalEditors;

internal class ExternalEditorManager
{
    private List<IExternalEditor> externalEditors = new();

    /// <summary>
    /// Detect all supported external editors. Raises <see cref="ExternalEditorsChanged"/> as it goes.
    /// </summary>
    /// <param name="callback">Called when another editor gets detected</param>
    /// <param name="ct"></param>
    /// <exception cref="OperationCanceledException">Thrown if canceled</exception>
    public void Load(Action<IExternalEditor> callback, CancellationToken ct)
    {
        TryAdd(new NotepadPlusPlus(), callback, ct);
        TryAdd(new VSCode(), callback, ct);
    }

    private void TryAdd(IExternalEditor editor, Action<IExternalEditor> callback, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (editor.Detect(ct))
        {
            externalEditors.Add(editor);
            ct.ThrowIfCancellationRequested();

            if (callback != null)
            {
                callback(editor);
            }
        }
    }
}
