using Panlingo.LanguageIdentification.Whatlang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ude;

namespace FPad.Encodings;

public static class EncodingManager
{
    private static Encoding encWin1251;
    private static Encoding encOem866;
    private static Encoding encKoi8R;
    private static Encoding encWin1252;
    private static Encoding encWin1250;
    private static Encoding encOem852;
    private static Encoding encWin1257;
    private static Encoding encOem775;

    private static EncodingVm vmUnicode;
    private static EncodingVm vmDefaultAnsi;

    private static CharsetDetector charsetDetector;
    private static WhatlangDetector langDetector;
    private static Dictionary<WhatlangLanguage, LangScoreCalculator> scoreCalculators;

    private static List<EncodingDetectionData> detectionData;

    public static IReadOnlyList<Alphabet> Alphabets { get; private set; }
    /// <summary>
    /// Supported encodings (flattened list from <see cref="Alphabets"/> + those which don't belong to an alphabet)
    /// </summary>
    public static IReadOnlyList<EncodingVm> Encodings { get; private set; }

    public static void Init()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        encWin1251 = Encoding.GetEncoding(1251);
        encOem866 = Encoding.GetEncoding(866);
        encKoi8R = Encoding.GetEncoding("koi8-r");
        encWin1252 = Encoding.GetEncoding(1252);
        encWin1250 = Encoding.GetEncoding(1250);
        encOem852 = Encoding.GetEncoding(852);
        encWin1257 = Encoding.GetEncoding(1257);
        encOem775 = Encoding.GetEncoding(775);

        charsetDetector = new CharsetDetector();
        // Reliably detected by Ude:
        // UTF-8
        // UTF-16BE
        // UTF-16LE
        // ASCII
        // windows-1251
        // IBM866
        // KOI8-R

        // Not detected by Ude:
        // 1257, 775 - baltics // Ude detects 1252 < .5
        // 1250 - central european // Ude detects 1252 < .5
        // 1252 - western european // Ude detects 1252 < .5
        langDetector = new WhatlangDetector();

        // Root-level (multibyte, "loseless") encodings
        vmUnicode = new EncodingVm(Encoding.Unicode, "Unicode", true, true);
        EncodingVm vmUtf8 = new EncodingVm(Encoding.UTF8, "UTF-8", true, false);
        EncodingVm vmUtf16Be = new EncodingVm(Encoding.BigEndianUnicode, "UTF 16 Big Endian", true, true);
        EncodingVm vmAscii = new EncodingVm(Encoding.ASCII, "ASCII", false, false);

        Alphabet cyrillic = new("Cyrillic");
        EncodingVm vmWin1251 = new EncodingVm(cyrillic, encWin1251, "Шindows 1251");
        EncodingVm vmOem866 = new EncodingVm(cyrillic, encOem866, "OEM 866 (DOS)");
        EncodingVm vmKoi8R = new EncodingVm(cyrillic, encKoi8R, "KOI8-R");

        Alphabet latin = new("Latin");
        vmDefaultAnsi = new EncodingVm(latin, encWin1252, "Western European (Windows 1252)");
        EncodingVm vmWin1250 = new EncodingVm(latin, encWin1250, "Central European (Windows 1250)");
        EncodingVm vmOem852 = new EncodingVm(latin, encOem852, "Central European OEM 852 (DOS)");
        EncodingVm vmWin1257 = new EncodingVm(latin, encWin1257, "Baltics (Windows 1257)");
        EncodingVm vmOem775 = new EncodingVm(latin, encOem775, "Baltics OEM 775 (DOS)");

        Alphabets = new List<Alphabet>() { cyrillic, latin };
        List<EncodingVm> encodings = new();
        encodings.Add(vmUnicode);
        encodings.Add(vmUtf8);
        encodings.Add(vmUtf16Be);
        encodings.Add(vmAscii);
        encodings.AddRange(Alphabets.SelectMany(x => x.Encodings));
        Encodings = encodings;

        detectionData = new List<EncodingDetectionData>()
        {
            new EncodingDetectionData(vmUnicode, "UTF-16LE"),
            new EncodingDetectionData(vmUtf8, "UTF-8"),
            new EncodingDetectionData(vmUtf16Be, "UTF-16BE"),
            new EncodingDetectionData(vmAscii, "ASCII"),
            new EncodingDetectionData(vmWin1251, "windows-1251"),
            new EncodingDetectionData(vmOem866, "IBM866"),
            new EncodingDetectionData(vmKoi8R, "KOI8-R"),
            new EncodingDetectionData(vmDefaultAnsi,
                [WhatlangLanguage.Afr, WhatlangLanguage.Cat, WhatlangLanguage.Eng, WhatlangLanguage.Ind,
                 WhatlangLanguage.Ita, WhatlangLanguage.Nob, WhatlangLanguage.Por,
                 WhatlangLanguage.Spa, WhatlangLanguage.Swe, WhatlangLanguage.Tgl,
                 WhatlangLanguage.Dan, WhatlangLanguage.Nld, WhatlangLanguage.Fra, WhatlangLanguage.Deu,
                 WhatlangLanguage.Fin]),
            new EncodingDetectionData(vmWin1250,
                [WhatlangLanguage.Ces, WhatlangLanguage.Pol, WhatlangLanguage.Slk,
                 WhatlangLanguage.Slv, WhatlangLanguage.Hun, WhatlangLanguage.Srp, WhatlangLanguage.Hrv,
                 WhatlangLanguage.Ron, WhatlangLanguage.Tuk]),
            new EncodingDetectionData(vmOem852,
                [WhatlangLanguage.Ces, WhatlangLanguage.Pol, WhatlangLanguage.Slk,
                 WhatlangLanguage.Slv, WhatlangLanguage.Hun, WhatlangLanguage.Srp, WhatlangLanguage.Hrv,
                 WhatlangLanguage.Ron, WhatlangLanguage.Tuk]),
            new EncodingDetectionData(vmWin1257,
                [WhatlangLanguage.Est, WhatlangLanguage.Lav, WhatlangLanguage.Lit]),
            new EncodingDetectionData(vmOem775,
                [WhatlangLanguage.Est, WhatlangLanguage.Lav, WhatlangLanguage.Lit]),
        };

        scoreCalculators = new Dictionary<WhatlangLanguage, LangScoreCalculator>();
        // Generated (Helper csproj)
        scoreCalculators.Add(WhatlangLanguage.Est, new LangScoreCalculator("äõöüÄÕÖÜ", "§÷³─šų▄„”ˇå™"));
        scoreCalculators.Add(WhatlangLanguage.Lav, new LangScoreCalculator("āčēģīķļņšūžĀČĒĢĪĶĻŅŠŪŽ", "Ōń’“­¹■┬╚╠╬═ęą█▐Ń‰…éėÕ×Ų ¶•ź¾"));
        scoreCalculators.Add(WhatlangLanguage.Lit, new LangScoreCalculator("ąčėęįšūųžĄČĖĘĮŠŪŲŽ", "ÓĶļµß­¹°■└╚╦┴█▐ŃŅŌÕ×Ö¶ø·½¾ĒĻ"));
        scoreCalculators.Add(WhatlangLanguage.Ces, new LangScoreCalculator("áéíóúýčďěňřšťůžÁÉÍÓÚÝČĎĚŇŘŠŤŮŽ", "ßˇ˙Ŕ´˛°ÜŁ¨×┴╔═Ë┌Ţ╚¤╠ŐŹ┘Ä ‚˘źÔĺçś…§µÖŕ¬·üć›¦"));
        scoreCalculators.Add(WhatlangLanguage.Pol, new LangScoreCalculator("óąćęłńśźżÓĄĆĘŁŃŚŹŻ", "ˇ╣Šŕ│˝ťč┐ËĂ╩úĐî»˘†©ä«ľ¤¨ă—Ť"));
        scoreCalculators.Add(WhatlangLanguage.Slk, new LangScoreCalculator("áäéíóôúýčďĺľňŕšťžÁÄÉÍÓÔÚÝČĎĹĽŇŔŠŤŽ", "ßńˇ˘˙ř´ż˛ÜŁ×┴─╔═Ë┌Ţ╚¤┼╝└ŐŹ „‚“ěź’–ęçś§µÖâ¬‘•ć›¦"));
        scoreCalculators.Add(WhatlangLanguage.Slv, new LangScoreCalculator("čšžČŠŽ", "ŔÜ×╚ŐÄźç§¬ć¦"));
        scoreCalculators.Add(WhatlangLanguage.Hun, new LangScoreCalculator("áéíóöúüőűÁÉÍÓÖÚÜŐŰ", "ßÝˇ÷˙Ř§┴╔═Ë┌▄Ň█ ‚˘”Ł‹µŕ™šŠë"));
        scoreCalculators.Add(WhatlangLanguage.Srp, new LangScoreCalculator("ćčđšžĆČĐŠŽ", "Ŕ­Ü×Ă╚ŐÄ†źç§Ź¬Ń¦"));
        scoreCalculators.Add(WhatlangLanguage.Hrv, new LangScoreCalculator("ćčđšžĆČĐŠŽ", "Ŕ­Ü×Ă╚ŐÄ†źç§Ź¬Ń¦"));
        scoreCalculators.Add(WhatlangLanguage.Ron, new LangScoreCalculator("âîășțÂÎĂȘȚ", "ÔţŃ?┬╬├ŚÇ¶×Ć"));
        scoreCalculators.Add(WhatlangLanguage.Tuk, new LangScoreCalculator("äçöüýňşÄÇÖÜÝŇŞ", "ńš÷Řř˛║─ăÍ▄ŢĎ¬„‡”ěĺ­Ž€™íŐ¸"));
    }

    public static EncodingVm DetectEncoding(byte[] textFileBytes)
    {
        // Empty file is Unicode because I said so.
        if (textFileBytes.Length == 0)
            return DefaultEncoding;

        // Ude, if detects preamble, returns encoding immediately with 100% certainty,
        // but for some reason it only works if file size is >= 4 bytes.
        // Empty UTF-16 files, empty UTF-8 files containing preamble only -
        // returns incorrect encoding with 0.5 confidence.
        // Therefore we will ourselves detect preambles.
        foreach (EncodingVm encoding in Encodings)
        {
            if (encoding.StartsWithPreamble(textFileBytes))
            {
                return encoding;
            }
        }

        charsetDetector.Reset();
        charsetDetector.Feed(textFileBytes, 0, textFileBytes.Length);
        charsetDetector.DataEnd();

        // example_1251.txt: 0.52 for ANSI 1251 (correct)
        // empty file with Preamble only: 0.5 for ANSI 1252 (incorrect)
        // Keep threshold at 0.5 for now, and strict inequality
        if (charsetDetector.Confidence > 0.5)
        {
            EncodingDetectionData result = detectionData.FirstOrDefault(x => x.UdeCode == charsetDetector.Charset);
            return (result != null) ? result.Vm : vmDefaultAnsi;
        }
        else
        {
            if (textFileBytes.Length == 0)
                return vmUnicode;
            ReadOnlySpan<byte> first1000 = textFileBytes.AsSpan();
            if (first1000.Length > 1000)
                first1000 = first1000[..1000];

            // Detect by language
            EncodingDetectionData bestCandidate = null;
            int bestScore = 0; // for Baltics
            foreach (EncodingDetectionData detectionData in detectionData.Where(x => x.Languages != null))
            {
                string strTest = detectionData.Vm.Encoding.GetString(first1000);
                WhatlangPrediction prediction = langDetector.PredictLanguage(strTest);
                if (prediction != null)
                {
                    if (prediction.IsReliable
                        // Recognizes languages with 100% confidence
                        // no matter how bastardized by incorrect encoding, cannot rely solely on recognition.
                        && detectionData.Languages.Contains(prediction.Language))
                    {
                        int score = 0;
                        if (scoreCalculators.TryGetValue(prediction.Language, out LangScoreCalculator scoreCalculator))
                            score = scoreCalculator.Calculate(strTest);
                        // We again assume that all iterations will return the same language,
                        // so we don't check for that, we only compare how neat the writing is.
                        if ((bestCandidate == null)
                            || (score > bestScore)) // if score equal then first wrapper wins
                        {
                            bestCandidate = detectionData;
                            bestScore = score;
                        }
                    }
                }
            }

            return (bestCandidate != null) ? bestCandidate.Vm : vmDefaultAnsi;
        }
    }

    public static EncodingVm DefaultEncoding => vmUnicode;
}
