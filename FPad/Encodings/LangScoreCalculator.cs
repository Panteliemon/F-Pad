using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Encodings;

/// <summary>
/// Calculates abstract number for comparing how good is alphabet obeyed for the language
/// </summary>
internal class LangScoreCalculator
{
    private sbyte[] scoreArray;

    public LangScoreCalculator(string goodChars, string badChars)
    {
        scoreArray = new sbyte[0x10000]; // 65536.
        
        // Control characters: -3
        for (int i = 0; i < 32; i++)
            scoreArray[i] = -3;
        scoreArray[13] = 0;
        scoreArray[10] = 0;
        scoreArray[9] = 0;
        scoreArray[127] = -3;

        // Good chars: +1
        goodChars = goodChars.Normalize();
        for (int i = 0; i < goodChars.Length; i++)
        {
            scoreArray[goodChars[i]] = 1;
        }

        // Bad chars: -2
        badChars = badChars.Normalize();
        for (int i = 0; i < badChars.Length; i++)
        {
            scoreArray[badChars[i]] = -2;
        }
    }

    public int Calculate(ReadOnlySpan<char> text)
    {
        int score = 0;
        for (int i = 0; i < text.Length; i++)
        {
            score += (int)scoreArray[text[i]];
        }

        return score;
    }
}
