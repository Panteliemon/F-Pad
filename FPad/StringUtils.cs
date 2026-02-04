using System;
using System.Collections.Generic;
using System.Linq;
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
}
