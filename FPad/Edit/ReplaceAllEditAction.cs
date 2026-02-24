using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FPad.Edit;

/// <summary>
/// It's "Replace All", not "replace all edit".
/// </summary>
internal class ReplaceAllEditAction : IEditAction
{
    /// <summary>
    /// Because of case-insensitive search we have to store each replaced fragment
    /// exactly as it was found.
    /// </summary>
    internal record Match(int Start, string SubStr);

    /// <summary>
    /// Indexes here are in text before applying "replace all" to it.
    /// </summary>
    private IReadOnlyList<Match> foundMatches;
    private string replacedWith;
    private Selection selectionBefore;
    private Selection selectionAfter;

    internal ReplaceAllEditAction(IReadOnlyList<Match> foundMatches, string replacedWith,
        Selection selectionBefore, Selection selectionAfter)
    {
        this.foundMatches = foundMatches;
        this.replacedWith = replacedWith;
        this.selectionBefore = selectionBefore;
        this.selectionAfter = selectionAfter;
    }

    #region IEditAction

    public string DisplayName => "Replace All";

    public void Apply(IEditor editor)
    {
        ReadOnlySpan<char> textBefore = editor.TextNoUndo;

        StringBuilder sb = new();
        int currentFragmentStart = 0;
        foreach (Match match in foundMatches)
        {
            sb.Append(textBefore[currentFragmentStart..match.Start]);
            sb.Append(replacedWith);
            currentFragmentStart = match.Start + match.SubStr.Length;
        }

        if (currentFragmentStart < textBefore.Length)
            sb.Append(textBefore[currentFragmentStart..]);

        editor.TextNoUndo = sb.ToString();
        editor.Selection = selectionAfter;
    }

    public void Rollback(IEditor editor)
    {
        ReadOnlySpan<char> textAfter = editor.TextNoUndo;

        StringBuilder sb = new();
        int currentFragmentStart = 0;
        int translationShift = 0;
        foreach (Match match in foundMatches)
        {
            int translatedMatchStart = match.Start + translationShift;
            sb.Append(textAfter[currentFragmentStart..translatedMatchStart]);
            sb.Append(match.SubStr);
            currentFragmentStart = translatedMatchStart + replacedWith.Length;
            translationShift += replacedWith.Length - match.SubStr.Length;
        }

        if (currentFragmentStart < textAfter.Length)
            sb.Append(textAfter[currentFragmentStart..]);

        editor.TextNoUndo = sb.ToString();
        editor.Selection = selectionBefore;
    }

    public bool Absorb(IEditAction nextAction)
    {
        return false;
    }

    #endregion
}
