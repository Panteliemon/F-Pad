using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.ExternalEditors;

internal class ExternalEditorBase
{
    public Icon Icon { get; protected set; }
    public string PathToExe { get; protected set; }

    protected static string WrapIntoQuotesIfNeeded(string str)
    {
        if (string.IsNullOrEmpty(str))
            return "\"\"";

        if (ContainsSpaces(str))
            return StringUtils.WrapIntoQuotes(str);
        else
            return str;
    }

    private static bool ContainsSpaces(string str)
    {
        if (str != null)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] <= 32)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
