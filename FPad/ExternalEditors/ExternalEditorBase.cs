using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FPad.ExternalEditors;

internal class ExternalEditorBase
{
    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    private static extern uint ExtractIconExW(
        string lpszFile,
        int nIconIndex,
        nint[] phiconLarge,
        nint[] phiconSmall,
        uint nIcons
    );

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool DestroyIcon(nint hIcon);

    public Bitmap Icon { get; protected set; }
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

    protected static Bitmap ExtractIcon(string pathToExe)
    {
        // This API kind of sort of only extracts 16x16 and 32x32 - I don't get it
        // Index [0] is what allegedly used by explorer for EXE.
        // Notepad++ for example has 302 icons if we ask for count, so,
        // other than inventing any heuristics - simply take [0].

        // There seems to be no API for extracting all sizes other than loading file as a library
        // and manually parsing its embedded resources. However we are ok with 16x16.

        Bitmap result = null;
        nint[] handles = new nint[1];
        int nIcons = (int)ExtractIconExW(pathToExe, 0, null, handles, 1);
        if ((nIcons == 1) && (handles[0] != 0))
        {
            try
            {
                using (Icon extracted = System.Drawing.Icon.FromHandle(handles[0]))
                {
                    result = extracted.ToBitmap();
                }
            }
            catch (Exception ex)
            {
            }

            DestroyIcon(handles[0]);
        }

        return result;
    }
}
