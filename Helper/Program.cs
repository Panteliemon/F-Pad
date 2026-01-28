using Panlingo.LanguageIdentification.Whatlang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper;

public record ScoreCalcStrings(string GoodChars, string BadChars);

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("1");

        string encodingManagerLines = string.Join(Environment.NewLine, GenerateScoreCalculatorLines());

    }

    private static List<string> GenerateScoreCalculatorLines()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding encWin1257 = Encoding.GetEncoding(1257);
        Encoding encWin1250 = Encoding.GetEncoding(1250);
        Encoding encOem775 = Encoding.GetEncoding(775);
        Encoding encOem852 = Encoding.GetEncoding(852);

        List<string> result = new();
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Est, "ÕÄÖÜ", [encWin1257, encOem775]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Lat, "āčēģīķļņšūž", [encWin1257, encOem775]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Lit, "ĄČĘĖĮŠŲŪŽ", [encWin1257, encOem775]));

        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Ces, "áčďéěíňóřšťúůýž", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Pol, "ąćęłńóśźż", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Slk, "áäčďéíĺľňóôŕšťúýž", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Slv, "čšž", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Hun, "áéíóöőúüű", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Srp, "čćđšž", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Hrv, "čćđšž", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Ron, "ăâîșț", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Tuk, "çäňöşüý", [encWin1250, encOem852]));

        return result;
    }

    /// <summary>
    /// Generate code line for EncodingManager.
    /// </summary>
    /// <param name="lang"></param>
    /// <param name="symbolsWithDiacritics">symbols with diacritics which the language has, in any case. no spaces.</param>
    /// <param name="encodings">ANSI encodings suitable for this language (at least 2, or there is no point)</param>
    /// <returns></returns>
    private static string GenerateScoreCalculatorLine(WhatlangLanguage lang, string symbolsWithDiacritics,
        ICollection<Encoding> encodings)
    {
        ScoreCalcStrings scoreCalcStrings = GetScoreCalcStrings(lang, symbolsWithDiacritics, encodings);
        string result = $"scoreCalculators.Add(WhatlangLanguage.{lang}, new LangScoreCalculator(\"{scoreCalcStrings.GoodChars}\", \"{scoreCalcStrings.BadChars}\"));";
        return result;
    }

    private static ScoreCalcStrings GetScoreCalcStrings(WhatlangLanguage lang, string symbolsWithDiacritics,
        ICollection<Encoding> encodings)
    {
        string goodSymbols = GetBothCasesSymbolsWithDiacritics(symbolsWithDiacritics);
        HashSet<char> badSymbolsSet = new();
        StringBuilder badSymbolsSb = new();

        foreach (Encoding enc in encodings)
        {
            byte[] encodedBytes = enc.GetBytes(goodSymbols);
            // Try to decode using wrong page, and add whatever we see to "bad symbols"
            foreach (Encoding wrongEnc in encodings.Where(x => x != enc))
            {
                string badResult = wrongEnc.GetString(encodedBytes);
                for (int i = 0; i < badResult.Length; i++)
                {
                    char c = badResult[i];
                    if ((c > 32) // control symbols are handled differently, skip.
                        && !goodSymbols.Contains(c)
                        && !badSymbolsSet.Contains(c))
                    {
                        badSymbolsSet.Add(c);
                        badSymbolsSb.Append(c);
                    }
                }
            }
        }

        return new ScoreCalcStrings(goodSymbols, badSymbolsSb.ToString());
    }

    private static string GetBothCasesSymbolsWithDiacritics(string symbolsWithDiacriticsAnyCase)
    {
        string symbolsWithDiacriticsLower = symbolsWithDiacriticsAnyCase.Normalize().ToLowerInvariant();
        HashSet<char> uniqueSymbols = new();
        for (int i = 0; i < symbolsWithDiacriticsLower.Length; i++)
        {
            char c = symbolsWithDiacriticsLower[i];
            if (!uniqueSymbols.Contains(c))
                uniqueSymbols.Add(c);
        }

        StringBuilder sb = new();
        foreach (char c in uniqueSymbols.OrderBy(c => c))
        {
            sb.Append(c);
        }

        sb.Append(sb.ToString().ToUpperInvariant());
        return sb.ToString();
    }
}
