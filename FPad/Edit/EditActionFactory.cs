using FPad.Encodings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Edit;

public static class EditActionFactory
{
    /// <summary>
    /// Creates manual input related edit action by observing how the text has changed.
    /// Returns null if the text hasn't changed.
    /// <paramref name="selectionBefore"/> must be valid (within range) for <paramref name="textBefore"/>,
    /// and <paramref name="positionAfterEdit"/> must be valid (within range) for <paramref name="textAfter"/> -
    /// this is not checked here and must be ensured outside.
    /// </summary>
    public static IEditAction DetectByTextChange(ReadOnlySpan<char> textBefore,
        Selection selectionBefore, ReadOnlySpan<char> textAfter, int positionAfterEdit)
    {
        int charsToEndBefore = textBefore.Length - selectionBefore.End;
        int charsToEndAfter = textAfter.Length - positionAfterEdit;

        int commonPrefixLength = StringUtils.GetCommonPrefixLength(textBefore, textAfter);
        if ((textBefore.Length == textAfter.Length)
            && (commonPrefixLength == textBefore.Length))
        {
            return null;
        }

        int? verifiedCommonSuffixLength = null;

        // This arrangement looks cryptic, but this is what you get if you group and minimize checks
        // to avoid repeatly checking the same stuff.
        if (charsToEndBefore == charsToEndAfter)
        {
            if (positionAfterEdit == selectionBefore.Start)
            {
                // Pattern: Clear the selection
                if ((selectionBefore.Length > 0)
                    && (commonPrefixLength >= selectionBefore.Start))
                {
                    verifiedCommonSuffixLength = StringUtils.GetCommonSuffixLength(textBefore, textAfter, charsToEndAfter);
                    if (verifiedCommonSuffixLength.Value >= charsToEndAfter) // ==
                    {
                        return new SelectionEraseEditAction(selectionBefore.Start, charsToEndBefore,
                            textBefore[(Range)selectionBefore].ToString());
                    }
                }
            }
            else if (positionAfterEdit > selectionBefore.Start)
            {
                // Pattern: Typing some characters (include type over selection)
                if (commonPrefixLength >= selectionBefore.Start)
                {
                    verifiedCommonSuffixLength = StringUtils.GetCommonSuffixLength(textBefore, textAfter, charsToEndAfter);
                    if (verifiedCommonSuffixLength.Value >= charsToEndAfter) // ==
                    {
                        return new SingleSymbolTypeEditAction(selectionBefore.Start, charsToEndBefore,
                            selectionBefore.Length > 0 ? textBefore[(Range)selectionBefore].ToString() : null,
                            textAfter[selectionBefore.Start..positionAfterEdit].ToString(),
                            positionAfterEdit);
                    }
                }
            }
            else // positionAfterEdit < selectionBefore.Start
            {
                // Pattern: Backspace (no selection prior)
                if ((selectionBefore.Length == 0)
                    && (commonPrefixLength >= positionAfterEdit))
                {
                    verifiedCommonSuffixLength = StringUtils.GetCommonSuffixLength(textBefore, textAfter, charsToEndAfter);
                    if (verifiedCommonSuffixLength.Value >= charsToEndAfter) // ==
                    {
                        return new SingleSymbolEraseEditAction(positionAfterEdit, charsToEndAfter,
                            textBefore[positionAfterEdit..selectionBefore.Start].ToString(),
                            selectionBefore.Start);
                    }
                }
            }
        }
        else if (charsToEndBefore > charsToEndAfter)
        {
            // Pattern: Delete (no selection prior)
            if ((positionAfterEdit == selectionBefore.Start)
                && (selectionBefore.Length == 0)
                && (commonPrefixLength >= selectionBefore.Start))
            {
                verifiedCommonSuffixLength = StringUtils.GetCommonSuffixLength(textBefore, textAfter, charsToEndAfter);
                if (verifiedCommonSuffixLength.Value >= charsToEndAfter) // ==
                {
                    return new SingleSymbolEraseEditAction(positionAfterEdit, charsToEndAfter,
                        textBefore[selectionBefore.Start..^charsToEndAfter].ToString(),
                        selectionBefore.Start);
                }
            }
        }
        // And there are no actions which would increase charsToEnd.


        // Unrecognized pattern: most likely means that function-specific action on text
        // was intercepted by text change handler, instead of being handled separately.
        // This should not happen if I'm not missing anything.
        // We will support undo-redo in this scenario exactly how we've detected it,
        // but this is not the "right" way.

        // Suffix should not overlap prefix. Cut equal part to prevent overlap.
        ReadOnlySpan<char> tailBefore = textBefore[commonPrefixLength..];
        ReadOnlySpan<char> tailAfter = textAfter[commonPrefixLength..];
        int commonSuffixLength;
        if (verifiedCommonSuffixLength.HasValue)
        {
            int shortestTailLength = Math.Min(tailBefore.Length, tailAfter.Length);
            if (verifiedCommonSuffixLength.Value >= shortestTailLength)
            {
                commonSuffixLength = shortestTailLength;
            }
            else
            {
                if (verifiedCommonSuffixLength.Value == charsToEndAfter)
                {
                    // This code is never executed!
                    // See above: as soon as verifiedCommonSuffixLength becomes equal to charsToEndAfter
                    // there is immediate return, always.
                    // If it wasn't for return, then here we would've had to handle what todo if
                    // the suffix was capped during initial comparison and it is longer in reality.

                    // The next line returns correct value just for the case,
                    // it just doesn't reuse previous comparison result and therefore
                    // is not as effective.
                    commonSuffixLength = StringUtils.GetCommonSuffixLength(tailBefore, tailAfter);
                }
                else // only "<" is possible
                {
                    commonSuffixLength = verifiedCommonSuffixLength.Value;
                }
            }
        }
        else
        {
            commonSuffixLength = StringUtils.GetCommonSuffixLength(tailBefore, tailAfter);
        }

        return new GenericEditAction(commonPrefixLength, commonSuffixLength,
            textBefore[commonPrefixLength..^commonSuffixLength].ToString(),
            textAfter[commonPrefixLength..^commonSuffixLength].ToString(),
            selectionBefore, new Selection(positionAfterEdit, 0));
    }

    public static IEditAction CreateCut(string text, Selection selection)
    {
        return new CutEditAction(selection.Start, text.Length - selection.End,
            text.SubString(selection));
    }

    public static IEditAction CreateDecode(string textBefore, EncodingVm encodingBefore,
        Selection selectionBefore, string textAfter, EncodingVm encodingAfter)
    {
        return new DecodeEditAction(textBefore, encodingBefore, selectionBefore, textAfter,
            encodingAfter);
    }

    public static IEditAction CreateEncodingForSave(EncodingVm encodingBefore,
        EncodingVm encodingAfter, bool raiseModifiedFlag)
    {
        return new EncodingForSaveEditAction(encodingBefore, encodingAfter, raiseModifiedFlag);
    }

    public static IEditAction CreatePaste(string text, Selection selection, string subStrToPaste)
    {
        return new PasteEditAction(selection.Start, text.Length - selection.End,
            text.SubString(selection), subStrToPaste);
    }

    public static IEditAction CreateReplace(string text, Selection selection, string replaceWith)
    {
        return new ReplaceEditAction(selection.Start, text.Length - selection.End,
            text.SubString(selection), replaceWith);
    }

    public static IEditAction CreateReplaceAll(string text, List<int> nonIntersectingMatches,
        int findSubStrLength, string replaceWith, Selection selectionBefore, Selection selectionAfter)
    {
        List<ReplaceAllEditAction.Match> matches = nonIntersectingMatches.Select(x =>
            new ReplaceAllEditAction.Match(x, text.Substring(x, findSubStrLength))
        ).ToList();
        return new ReplaceAllEditAction(matches, replaceWith, selectionBefore, selectionAfter);
    }
}
