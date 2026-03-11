using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FPad;

public static class StringUtils
{
    private static SHA256 sha256;

    public static string GetPathHash(string fullPath)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(fullPath.ToUpperInvariant().Normalize());
        sha256 ??= SHA256.Create();
        byte[] hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public static (int lineIndex, int charIndex) GetLineAndCol(string str, int position)
    {
        return GetLineAndCol(str, 0, 0, 0, position);
    }

    public static (int lineIndex, int charIndex) GetLineAndCol(string str,
        int startFromIndex, int startLineIndex, int startCharIndex,
        int targetPosition)
    {
        if ((str == null) || (targetPosition > str.Length)
            || (startFromIndex < 0) || (startFromIndex > targetPosition))
        {
            return (startLineIndex, startCharIndex);
        }

        int lineIndex = startLineIndex;
        int charIndex = startCharIndex;
        bool isAfter13 = false;
        for (int i = startFromIndex; i < targetPosition; i++)
        {
            char c = str[i];
            if (isAfter13)
            {
                if (c == 13)
                {
                    lineIndex++;
                    charIndex = 0;
                    // Stay in current state
                }
                else if (c == 10)
                {
                    // charIndex stays 0
                    isAfter13 = false;
                }
                else
                {
                    charIndex++;
                    isAfter13 = false;
                }
            }
            else
            {
                if (c == 13)
                {
                    lineIndex++;
                    charIndex = 0;
                    isAfter13 = true;
                }
                else if (c == 10)
                {
                    lineIndex++;
                    charIndex = 0;
                }
                else
                {
                    charIndex++;
                }
            }
        }

        return (lineIndex, charIndex);
    }

    public static (int position, int positionLineIndex, int positionCharIndex) GetPositionAdaptive(string str, int lineIndex, int charIndex)
    {
        if ((str == null) || (lineIndex < 0) || (charIndex < 0))
            return (0, 0, 0);

        int currentLineIndex = 0;
        int currentCharIndex = 0;
        bool isAfter13 = false;
        for (int i = 0; i < str.Length; i++)
        {
            if (currentLineIndex == lineIndex)
            {
                if (isAfter13)
                {
                    char c = str[i];
                    if (c == 10)
                    {
                        // We haven't entered the current line yet. Don't detect.
                    }
                    else if (c == 13)
                    {
                        // Target line has zero length
                        return (i, currentLineIndex, currentCharIndex);
                    }
                    else
                    {
                        if (currentCharIndex == charIndex)
                        {
                            return (i, currentLineIndex, currentCharIndex);
                        }
                        else
                        {
                            currentCharIndex++;
                        }
                    }

                    isAfter13 = false;
                }
                else
                {
                    if (currentCharIndex == charIndex)
                    {
                        return (i, currentLineIndex, currentCharIndex);
                    }
                    else
                    {
                        char c = str[i];
                        if ((c == 10) || (c == 13))
                        {
                            // Target line has ended and we haven't reached required col
                            return (i, currentLineIndex, currentCharIndex);
                        }
                        else
                        {
                            currentCharIndex++;
                        }
                    }
                }
            }
            else
            {
                char c = str[i];
                if (isAfter13)
                {
                    if (c == 13)
                    {
                        currentLineIndex++;
                        currentCharIndex = 0;
                    }
                    else if (c == 10)
                    {
                        isAfter13 = false;
                    }
                    else
                    {
                        currentCharIndex++;
                        isAfter13 = false;
                    }
                }
                else
                {
                    if (c == 13)
                    {
                        currentLineIndex++;
                        currentCharIndex = 0;
                        isAfter13 = true;
                    }
                    else if (c == 10)
                    {
                        currentLineIndex++;
                        currentCharIndex = 0;
                    }
                    else
                    {
                        currentCharIndex++;
                    }
                }
            }
        }

        // Text has ended and we haven't reached required line / required col within line
        return (str.Length, currentLineIndex, currentCharIndex);
    }

    /// <summary>
    /// </summary>
    /// <remarks>For empty strings returns 1</remarks>
    public static int GetLinesCount(string str)
    {
        if (str == null)
            return 0;

        int lineIndex = 0;
        bool isAfter13 = false;
        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            if (isAfter13)
            {
                if (c == 13)
                {
                    lineIndex++;
                }
                else
                {
                    isAfter13 = false;
                }
            }
            else
            {
                if (c == 13)
                {
                    lineIndex++;
                    isAfter13 = true;
                }
                else if (c == 10)
                {
                    lineIndex++;
                }
            }
        }

        return lineIndex + 1;
    }

    public static string WrapIntoQuotes(string str)
    {
        if (string.IsNullOrEmpty(str))
            return "\"\"";

        if (str.Length == 1)
            return $"\"{str}\"";

        if (str[0] == '\"')
        {
            if (str[^1] == '\"')
                return str;
            else
                return str + "\"";
        }
        else
        {
            if (str[^1] == '\"')
                return $"\"{str}";
            else
                return $"\"{str}\"";
        }
    }

    public static int GetCommonPrefixLength(ReadOnlySpan<char> str1, ReadOnlySpan<char> str2)
    {
        if (str1.Length <= str2.Length)
        {
            for (int i = 0; i < str1.Length; i++)
            {
                if (str1[i] != str2[i])
                    return i;
            }

            return str1.Length;
        }
        else
        {
            for (int i = 0; i < str2.Length; i++)
            {
                if (str1[i] != str2[i])
                    return i;
            }

            return str2.Length;
        }
    }

    public static int GetCommonSuffixLength(ReadOnlySpan<char> str1, ReadOnlySpan<char> str2)
    {
        int pos1 = str1.Length;
        int pos2 = str2.Length;
        int result = 0;
        if (str1.Length <= str2.Length)
        {
            while (pos1 > 0)
            {
                pos1--;
                pos2--;
                if (str1[pos1] != str2[pos2])
                    return result;
                result++;
            }

            return str1.Length;
        }
        else
        {
            while (pos2 > 0)
            {
                pos1--;
                pos2--;
                if (str1[pos1] != str2[pos2])
                    return result;
                result++;
            }

            return str2.Length;
        }
    }

    public static int GetCommonSuffixLength(ReadOnlySpan<char> str1, ReadOnlySpan<char> str2, int suffixLengthLimit)
    {
        int pos1 = str1.Length;
        int pos2 = str2.Length;
        int result = 0;
        if (str1.Length <= str2.Length)
        {
            while ((pos1 > 0) && (result < suffixLengthLimit))
            {
                pos1--;
                pos2--;
                if (str1[pos1] != str2[pos2])
                    return result;
                result++;
            }

            return str1.Length;
        }
        else
        {
            while ((pos2 > 0) && (result < suffixLengthLimit))
            {
                pos1--;
                pos2--;
                if (str1[pos1] != str2[pos2])
                    return result;
                result++;
            }

            return str2.Length;
        }
    }

    /// <summary>
    /// Callback for iteration over text split to lines
    /// </summary>
    /// <param name="line">Current line</param>
    /// <param name="lineIndex">Index of current line in the text</param>
    /// <param name="lineStartPosition">Index of the first char in the line in the initial text.</param>
    /// <param name="nextLineStartPosition">Index of first char of the next line in initial text.
    /// If current line is last - this value is length of initial text.</param>
    /// <param name="isLastLine">True if current line is the last one.</param>
    /// <returns>Return false to break outer cycle by lines.
    /// If returned true - iteration continues until end of text is reached.</returns>
    public delegate bool ProcessLineFunc(ReadOnlySpan<char> line, int lineIndex,
        int lineStartPosition, int nextLineStartPosition, bool isLastLine);

    /// <summary>
    /// Split string to lines without reallocating memory
    /// </summary>
    /// <param name="allText">Initial text</param>
    /// <param name="callback">Called for each line</param>
    public static void IterateOverSplitByLines(ReadOnlySpan<char> allText, ProcessLineFunc callback)
    {
        int currentLineIndex = 0;
        int currentLineStart = 0;
        int currentLineLength = 0;
        bool isAfter13 = false;

        for (int i = 0; i < allText.Length; i++)
        {
            char c = allText[i];
            if (isAfter13)
            {
                if (c == 10)
                {
                    bool continueIteration = callback(
                        allText.Slice(currentLineStart, currentLineLength),
                        currentLineIndex, currentLineStart, i + 1, false
                    );
                    if (!continueIteration)
                        return;

                    currentLineIndex++;
                    currentLineStart = i + 1;
                    currentLineLength = 0;
                    isAfter13 = false;
                }
                else if (c == 13)
                {
                    // Prev 13 goes into the current line, current 13 - pending.
                    currentLineLength++;
                }
                else
                {
                    // No line break, just stray 13
                    currentLineLength += 2;
                    isAfter13 = false;
                }
            }
            else
            {
                if (c == 10)
                {
                    bool continueIteration = callback(
                        allText.Slice(currentLineStart, currentLineLength),
                        currentLineIndex, currentLineStart, i + 1, false
                    );
                    if (!continueIteration)
                        return;

                    currentLineIndex++;
                    currentLineStart = i + 1;
                    currentLineLength = 0;
                }
                else if (c == 13)
                {
                    isAfter13 = true;
                }
                else
                {
                    currentLineLength++;
                }
            }
        }

        callback(allText[currentLineStart..], currentLineIndex, currentLineStart, allText.Length, true);
    }

    /// <summary>
    /// Cut line on the nearest space for Word Wrap
    /// </summary>
    /// <param name="line"></param>
    /// <param name="maxLength"></param>
    /// <param name="nextPartStart"></param>
    /// <returns></returns>
    public static ReadOnlySpan<char> CutOnSpaceToFit(ReadOnlySpan<char> line, int maxLength, out int nextPartStart)
    {
        if (line.Length <= maxLength)
        {
            nextPartStart = line.Length;
            return line.TrimEnd();
        }

        if (GetCharType(line[maxLength]) == ConseqCharType.Space)
        {
            // Find next non-space
            for (int i = maxLength + 1; i < line.Length; i++)
            {
                if (GetCharType(line[i]) != ConseqCharType.Space)
                {
                    nextPartStart = i;
                    return line[0..maxLength].TrimEnd();
                }
            }

            // Only spaces after: report as if there is nothing left in current string after cut
            nextPartStart = line.Length;
            return line.TrimEnd();
        }
        else
        {
            // Find space before [maxLength]
            for (int i = maxLength - 1; i >= 0; i--)
            {
                if (GetCharType(line[i]) == ConseqCharType.Space)
                {
                    nextPartStart = i + 1;
                    return line[0..i].TrimEnd();
                }
            }

            // No spaces: force cut at max length
            nextPartStart = maxLength;
            return line[0..maxLength];
        }    
    }

    public static LineBreaks DetectLineBreaks(ReadOnlySpan<char> str)
    {
        LineBreaks result = LineBreaks.None;

        bool isAfter13 = false;
        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            if (isAfter13)
            {
                if (c == 10)
                {
                    result |= LineBreaks.Windows;
                    isAfter13 = false;
                }
                else if (c == 13)
                {
                    // Nothing changes
                }
                else
                {
                    isAfter13 = false;
                }
            }
            else
            {
                if (c == 13)
                {
                    isAfter13 = true;
                }
                else if (c == 10)
                {
                    result |= LineBreaks.Unix;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Returns new string in which all line breaks of the initial string are replaced to the specified value.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="targetLineBreaks">Only pure flags are accepted, no combinations.</param>
    /// <returns></returns>
    public static string NormalizeLineBreaks(ReadOnlySpan<char> str, LineBreaks targetLineBreaks)
    {
        StringBuilder sb = new();
        if (targetLineBreaks == LineBreaks.Windows)
        {
            IterateOverSplitByLines(str, (lineSpan, _, _, _, isLastLine) =>
            {
                sb.Append(lineSpan);
                if (!isLastLine)
                {
                    sb.Append((char)13);
                    sb.Append((char)10);
                }

                return true;
            });
        }
        else if (targetLineBreaks == LineBreaks.Unix)
        {
            IterateOverSplitByLines(str, (lineSpan, _, _, _, isLastLine) =>
            {
                sb.Append(lineSpan);
                if (!isLastLine)
                {
                    sb.Append((char)10);
                }

                return true;
            });
        }
        else if (targetLineBreaks == LineBreaks.None)
        {
            throw new ArgumentException($"Need to specify a value for {nameof(targetLineBreaks)} parameter.");
        }
        else if ((int)targetLineBreaks >= 4)
        {
            throw new NotSupportedException();
        }
        else
        {
            throw new ArgumentException($"Flag combinations are not allowed for {nameof(NormalizeLineBreaks)}");
        }

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ConseqCharType GetCharType(char c)
    {
        if (c <= 32)
            return ConseqCharType.Space;
        else
            return IsPartOfWord(c) ? ConseqCharType.Word : ConseqCharType.NonWord;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsPartOfWord(char c)
    {
        return (c == '_') || char.IsLetterOrDigit(c);
    }
}
