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
    /// Raised by <see cref="Load"/> on the thread on which <see cref="Load"/> executes
    /// when another editor gets detected.
    /// </summary>
    public event EventHandler<IReadOnlyList<IExternalEditor>> ExternalEditorsChanged;

    /// <summary>
    /// Detect all supported external editors. Raises <see cref="ExternalEditorsChanged"/> as it goes.
    /// </summary>
    /// <param name="ct"></param>
    /// <exception cref="OperationCanceledException">Thrown if canceled</exception>
    public void Load(CancellationToken ct)
    {
        TryAdd(new NotepadPlusPlus(), ct);
        TryAdd(new VSCode(), ct);
    }

    private void TryAdd(IExternalEditor editor, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (editor.Detect(ct))
        {
            externalEditors.Add(editor);
            ct.ThrowIfCancellationRequested();

            if (ExternalEditorsChanged != null)
            {
                List<IExternalEditor> listCopy = externalEditors.ToList();
                ExternalEditorsChanged.Invoke(this, listCopy);
            }
        }
    }
}
