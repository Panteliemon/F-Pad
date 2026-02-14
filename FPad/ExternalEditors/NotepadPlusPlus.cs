using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPad.ExternalEditors;

internal class NotepadPlusPlus : ExternalEditorBase, IExternalEditor
{
    public string DisplayName => "Notepad++";

    public string CreateCommandLineArgs(string pathToFile, int lineIndex, int charIndex)
    {
        StringBuilder sb = new();
        if (lineIndex >= 0)
        {
            sb.Append("-n");
            sb.Append((lineIndex + 1).ToString());
            sb.Append(' ');
        }
        
        if (charIndex >= 0)
        {
            sb.Append("-c");
            sb.Append((charIndex + 1).ToString());
            sb.Append(' ');
        }

        sb.Append(WrapIntoQuotesIfNeeded(pathToFile));
        return sb.ToString();
    }

    public bool Detect(CancellationToken ct)
    {
        return false;
    }
}
