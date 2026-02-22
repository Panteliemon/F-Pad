using FPad.Edit;
using FPad.Encodings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.FPad;

internal class MockEditor : IEditor
{
    private string text = string.Empty;
    private Selection selection;
    private EncodingVm encoding;

    public MockEditor(EncodingTestsFixture fixture)
    {
        // Param "fixture" not used itself, but it is required to make sure that
        // encoding manager has been initialized.
        encoding = EncodingManager.DefaultEncoding;
    }

    public string TextNoUndo
    {
        get => text;
        set
        {
            text = value ?? string.Empty;
            selection = NormalizeSelection(selection);
        }
    }

    public Selection Selection
    {
        get => selection;
        set
        {
            selection = NormalizeSelection(value);
        }
    }

    public EncodingVm Encoding
    {
        get => encoding;
        set
        {
            encoding = value;
        }
    }

    #region Private

    private Selection NormalizeSelection(Selection sel)
    {
        return Selection.FromStartEnd(NormalizePosition(sel.Start), NormalizePosition(sel.End));
    }

    private int NormalizePosition(int pos)
    {
        if (pos <= 0)
            return 0;
        else if (pos > text.Length)
            return text.Length;
        else
            return pos;
    }

    #endregion
}
