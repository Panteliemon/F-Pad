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
    private static Dictionary<LangKey, LangScoreCalculator> scoreCalculators;

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
            new EncodingDetectionData(vmWin1251, "windows-1251",
                [WhatlangLanguage.Rus, WhatlangLanguage.Bel, WhatlangLanguage.Ukr,
                 WhatlangLanguage.Srp, WhatlangLanguage.Bul, WhatlangLanguage.Mkd]),
            new EncodingDetectionData(vmOem866, "IBM866",
                [WhatlangLanguage.Rus, WhatlangLanguage.Bel, WhatlangLanguage.Ukr,
                 WhatlangLanguage.Bul]),
            new EncodingDetectionData(vmKoi8R, "KOI8-R", [WhatlangLanguage.Rus]),
            new EncodingDetectionData(vmDefaultAnsi,
                [WhatlangLanguage.Afr, WhatlangLanguage.Cat, WhatlangLanguage.Ind,
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

        scoreCalculators = new Dictionary<LangKey, LangScoreCalculator>();
        // Generated (Helper csproj)
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Est, WhatlangScript.Latn), new LangScoreCalculator("äõöüÄÕÖÜ", "§÷³─šų▄„”ˇå™Ć¤µ¶¼•–├żĄČ╝Ģ¢£"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Lav, WhatlangScript.Latn), new LangScoreCalculator("āčēģīķļņšūžĀČĒĢĪĶĻŅŠŪŽ", "Ōń’“­¹■┬╚╠╬═ęą█▐Ń‰…éėÕ×Ų ¶•ź¾Ä¨£«·¼Å†€¢Ŗ»½─üŹōŻĘ╝┼åĆÆó¬╗Į"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Lit, WhatlangScript.Latn), new LangScoreCalculator("ąčėęįšūųžĄČĖĘĮŠŪŲŽ", "ÓĶļµß­¹°■└╚╦┴█▐ŃŅŌÕ×Ö¶ø·½¾ĒĻÄ…¨—™ÆÅ«³„–® Ŗ²─ģŹŚ»┼Ī│äī¢śĀ¬▓"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Ces, WhatlangScript.Latn), new LangScoreCalculator("áéíóúýčďěňřšťůžÁÉÍÓÚÝČĎĚŇŘŠŤŮŽ", "ßˇ˙Ŕ´˛°ÜŁ¨×┴╔═Ë┌Ţ╚¤╠ŐŹ┘Ä ‚˘źÔĺçś…§µÖŕ¬·üć›¦Ă©­łş˝Ĺ™ĄŻľ‰“Ś‡®├ę│║─Ć┼ą»żëôî«"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Pol, WhatlangScript.Latn), new LangScoreCalculator("óąćęłńśźżÓĄĆĘŁŃŚŹŻ", "ˇ╣Šŕ│˝ťč┐ËĂ╩úĐî»˘†©ä«ľ¤¨ă—ŤÄ…‡™Ĺ‚„›şĽ“š├─ůçÖ┼é║╝ôüâÜ╗"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Slk, WhatlangScript.Latn), new LangScoreCalculator("áäéíóôúýčďĺľňŕšťžÁÄÉÍÓÔÚÝČĎĹĽŇŔŠŤŽ", "ßńˇ˘˙ř´ż˛ÜŁ×┴─╔═Ë┌Ţ╚¤┼╝└ŐŹ „‚“ěź’–ęçś§µÖâ¬‘•ć›¦Ă©­łş˝Ą‰”Śą‡├│┤║ŻĆüëöî╣"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Slv, WhatlangScript.Latn), new LangScoreCalculator("čšžČŠŽ", "ŔÜ×╚ŐÄźç§¬ć¦ŤĹˇľŚ ˝─Ź┼íżîáŻ"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Hun, WhatlangScript.Latn), new LangScoreCalculator("áéíóöúüőűÁÉÍÓÖÚÜŐŰ", "ßÝˇ÷˙Ř§┴╔═Ë┌▄Ň█ ‚˘”Ł‹µŕ™šŠëĂ©­ł¶şĽĹ‘±‰Ť“–ś°├ę│Â║╝┼▒Źôľť░"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Srp, WhatlangScript.Latn), new LangScoreCalculator("ćčđšžĆČĐŠŽ", "Ŕ­Ü×Ă╚ŐÄ†źç§Ź¬Ń¦‡Ť‘ĹˇľŚ ˝─┼íżîÉáŻ"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Srp, WhatlangScript.Cyrl), new LangScoreCalculator("абвгдежзиклмнопрстуфхцчшђјљњћџАБВГДЕЖЗИКЛМНОПРСТУФХЦЧШЂЈЉЊЋЏ", "°±Ііґµ¶·ёє»ЅѕїЃ‚ѓ„…†‡€’™›‘“”•–—ќ Ўў¤Ґ¦§Ё‰‹"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Hrv, WhatlangScript.Latn), new LangScoreCalculator("ćčđšžĆČĐŠŽ", "Ŕ­Ü×Ă╚ŐÄ†źç§Ź¬Ń¦‡Ť‘ĹˇľŚ ˝─┼íżîÉáŻ"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Ron, WhatlangScript.Latn), new LangScoreCalculator("âîășțÂÎĂȘȚ", "ÔţŃ┬╬├ŚÇ¶×Ć˘®ÄČ™›‚Žšó«─╚ÖŤéśÜ"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Tuk, WhatlangScript.Latn), new LangScoreCalculator("äçöüýňşÄÇÖÜÝŇŞ", "ńš÷Řř˛║─ăÍ▄ŢĎ¬„‡”ěĺ­Ž€™íŐ¸Ă¤§¶Ľ˝Ĺź–śťž├ĄÂ╝Ż┼łčľŁ×"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Rus, WhatlangScript.Cyrl), new LangScoreCalculator("абвгдежзийклмнопрстуфхцчшщъыьэюяёАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯЁ", "ЄєЇїЎў°∙·√№¤■ ╕└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀╗ЈҐ¦§©«¬­®ЂЃ‚ѓ„…†‡€‰Љ‹ЊЌЋЏђ‘’“”•–—™љ›њќћџ║╖╛╜╝│┐┤░▒▓⌠≈≤≥⌡²÷і±Іґµ¶»јЅѕ╡╢╣"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Bel, WhatlangScript.Cyrl), new LangScoreCalculator("абвгдежзйклмнопрстуфхцчшыьэюяёіўАБВГДЕЖЗЙКЛМНОПРСТУФХЦЧШЫЬЭЮЯЁІЎ", "щъЄєЇї°√№¤■ ╕│└┴┬├─┼╞╟╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪█▄▌▐▀и▓ЈҐ¦§©«¬­®ЂЃ‚ѓ„…†‡‰Љ‹ЊЌЋЏђ‘’“”•–—›њќћџ±ґµ¶·»јЅѕ€™љ░▒┤╡╢╖╣║╗╝╜╛┐ИЩЪ"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Ukr, WhatlangScript.Cyrl), new LangScoreCalculator("абвгдежзийклмнопрстуфхцчшщьюяєіїґАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЮЯЄІЇҐ", "ъыэЁёЎў°∙№■ ║│┐┤└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘▄▐▀▓Ј¤¦§©«¬­®ЂЃ‚ѓ„…†‡€‰Љ‹ЊЌЋЏђ‘’“”•–—™њћџ±µ¶·»јЅѕљ›ќ░▒╡╢╖╕╣╗╝╜╛ЪЫЭ"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Bul, WhatlangScript.Cyrl), new LangScoreCalculator("абвгдежзийклмнопрстуфхцчшщъьюяАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЬЮЯ", "ыэЁёЄєЇїЎў°∙·№■ └┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌▄▐▀Ј¤Ґ¦§©«¬­®ЂЃ‚ѓ„…†‡€‰Љ‹ЊЌЋЏђ‘’“”•–—™љњћџ±Ііґµ¶»јЅѕ›ќ░▒▓│┤╡╢╖╕╣║╗╝╜╛┐ЫЭ"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Mkd, WhatlangScript.Cyrl), new LangScoreCalculator("абвгдежзиклмнопрстуфхцчшѓѕјљњќџАБВГДЕЖЗИКЛМНОПРСТУФХЦЧШЃЅЈЉЊЌЏ", "°±Ііґµ¶·ёє»їЂ‚„…†‡€“•™ђ‘’”–—›ћ Ўў¤Ґ¦§Ё‰"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Afr, WhatlangScript.Latn), new LangScoreCalculator("áäèéêëíîïóôöúûüýŉÁÄÈÉÊËÍÎÏÓÔÖÚÛÜÝŉ", "Ã¡¤¨©ª«­®¯³´¶º»¼½Å‰„ˆŠ‹Ž“”–š›œ"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Cat, WhatlangScript.Latn), new LangScoreCalculator("àçèéíïòóúüÀÇÈÉÍÏÒÓÚÜ", "Ã §¨©­¯²³º¼€‡ˆ‰’“šœ"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Ita, WhatlangScript.Latn), new LangScoreCalculator("àèéìîòùÀÈÉÌÎÒÙ", "Ã ¨©¬®²¹€ˆ‰ŒŽ’™"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Nob, WhatlangScript.Latn), new LangScoreCalculator("åæéòóôøÅÆÉÒÓÔØ", "Ã¥¦©²³´¸…†‰’“”˜"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Por, WhatlangScript.Latn), new LangScoreCalculator("àáâãçéêíóôõúÀÁÂÃÇÉÊÍÓÔÕÚ", " ¡¢£§©ª­³´µº€‚ƒ‡‰Š“”•š"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Spa, WhatlangScript.Latn), new LangScoreCalculator("¡¿áéíñóú¡¿ÁÉÍÑÓÚ", "ÂÃ©­±³º‰‘“š"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Swe, WhatlangScript.Latn), new LangScoreCalculator("äåéöÄÅÉÖ", "Ã¤¥©¶„…‰–"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Dan, WhatlangScript.Latn), new LangScoreCalculator("åæéøÅÆÉØ", "Ã¥¦©¸…†‰˜"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Nld, WhatlangScript.Latn), new LangScoreCalculator("áèéëíïóöúüĳÁÈÉËÍÏÓÖÚÜĲ", "Ã¡¨©«­¯³¶º¼Äˆ‰‹“–šœ²"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Fra, WhatlangScript.Latn), new LangScoreCalculator("àâæçèéêëîïôùûüÿœÀÂÆÇÈÉÊËÎÏÔÙÛÜŸŒ", "Ã ¢¦§¨©ª«®¯´¹»¼¿Å“€‚†‡ˆ‰Š‹Ž”™›¸’"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Deu, WhatlangScript.Latn), new LangScoreCalculator("ßäöüßÄÖÜ", "ÃŸ¤¶¼„–œ"));
        scoreCalculators.Add(new LangKey(WhatlangLanguage.Fin, WhatlangScript.Latn), new LangScoreCalculator("äåöÄÅÖ", "Ã¤¥¶„…–"));
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
                        LangKey key = new(prediction.Language, prediction.Script);
                        if (scoreCalculators.TryGetValue(key, out LangScoreCalculator scoreCalculator))
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
