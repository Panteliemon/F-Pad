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
        for (int i = startFromIndex; i < targetPosition; i++)
        {
            char c = str[i];
            if (c == 10)
            {
                lineIndex++;
                charIndex = 0;
            }
            else
            {
                charIndex++;
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
        for (int i = 0; i < str.Length; i++)
        {
            if (currentLineIndex == lineIndex)
            {
                if (currentCharIndex == charIndex)
                {
                    return (i, currentLineIndex, currentCharIndex);
                }
                else
                {
                    char c = str[i];
                    if (c == 10)
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
            else
            {
                char c = str[i];
                if (c == 10)
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
        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            if (c == 10)
            {
                lineIndex++;
            }
        }

        return lineIndex + 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsPartOfWord(char c)
    {
        return (c == '_') || char.IsLetterOrDigit(c);
    }
}
