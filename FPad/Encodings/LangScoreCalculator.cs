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
    private string goodChars;
    private string badChars;

    public LangScoreCalculator(string goodChars, string badChars)
    {
        this.goodChars = goodChars.Normalize();
        this.badChars = badChars.Normalize();
    }

    public int Calculate(ReadOnlySpan<char> text)
    {
        int score = 0;
        for (int i=0; i<text.Length; i++)
        {
            char c = text[i];
            if (goodChars.Contains(c))
            {
                score += 1;
            }
            else if (badChars.Contains(c))
            {
                score -= 2;
            }
            else if ((c < 32) && (c != 13) && (c != 10) && (c != '\t'))
            {
                score -= 3;
            }
        }

        return score;
    }
}
