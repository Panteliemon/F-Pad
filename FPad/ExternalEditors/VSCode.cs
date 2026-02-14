using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPad.ExternalEditors;

internal class VSCode : ExternalEditorBase, IExternalEditor
{
    public string DisplayName => "Visual Studio Code";

    public string CreateCommandLineArgs(string pathToFile, int lineIndex, int charIndex)
    {
        if ((lineIndex >= 0) || (charIndex >= 0))
        {
            StringBuilder sb = new();
            sb.Append("--goto ");
            sb.Append(WrapIntoQuotesIfNeeded(pathToFile));
            sb.Append(':');
            sb.Append(lineIndex >= 0 ? (lineIndex + 1).ToString() : "1");
            if (charIndex >= 0)
            {
                sb.Append(':');
                sb.Append((charIndex + 1).ToString());
            }

            return sb.ToString();
        }
        else
        {
            return WrapIntoQuotesIfNeeded(pathToFile);
        }
    }

    public bool Detect(CancellationToken ct)
    {
        return false;
    }
}
