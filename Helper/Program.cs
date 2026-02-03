using Panlingo.LanguageIdentification.Whatlang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Helper;

public record ScoreCalcStrings(string GoodChars, string BadChars);

public static class Program
{
    static Mutex mutex;
    static EventWaitHandle eventHandle;
    static object consoleSyncObj = new();

    public static void Main()
    {
        Console.WriteLine("3");

        //string encodingManagerLines = string.Join(Environment.NewLine, GenerateScoreCalculatorLines());

        //TestEvents();
        
        MakeIcon();
    }

    private static void MakeIcon()
    {
        string folder = @"D:\Bn\Src\FPad\Local";
        string[] srcFiles = ["fpad16_bilinear.png", "fpad32_.png", "fpad48_.png", "fpad64_.png"];
        int[] sizes = srcFiles.Select(x => int.Parse(new string(x.ToCharArray().Where(c => char.IsAsciiDigit(c)).ToArray()))).ToArray();

        List<byte[]> bytes = new();
        foreach(string fileName in srcFiles)
        {
            string path = Path.Combine(folder, fileName);
            bytes.Add(File.ReadAllBytes(path));
        }

        int[] offsets = new int[srcFiles.Length];
        offsets[0] = 6 + srcFiles.Length * 16;
        for (int i=1; i< srcFiles.Length; i++)
        {
            offsets[i] = offsets[i - 1] + bytes[i - 1].Length;
        }

        string outPath = Path.Combine(folder, "f-pad.ico");
        using (FileStream fs = new FileStream(outPath, FileMode.Create, FileAccess.Write, FileShare.None, 262144))
        {
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write((short)0);
                bw.Write((short)1);
                bw.Write((short)srcFiles.Length);

                for (int i = 0; i < srcFiles.Length; i++)
                {
                    bw.Write((byte)sizes[i]);
                    bw.Write((byte)sizes[i]);
                    bw.Write((byte)0);
                    bw.Write((byte)0);
                    bw.Write((short)0); // wPlanes - no idea
                    bw.Write((short)0); // bits per pixel - no idea
                    bw.Write(bytes[i].Length);
                    bw.Write(offsets[i]);
                }

                for (int i = 0; i < srcFiles.Length; i++)
                {
                    bw.Write(bytes[i]);
                }
            }
        }
    }

    private static void TestEvents()
    {
        mutex = new Mutex(false);
        eventHandle = new EventWaitHandle(false, EventResetMode.ManualReset, "test_named_event_1");

        for (int i = 0; i < 8; i++)
        {
            string threadName = $"T{i + 1}"; // capture to local variable so each thread uses unique string
            Thread thr = new Thread(() => ThreadProc(threadName));
            thr.Start();
        }

        while (true)
        {
            ConsoleWriteLine("type S to send the event, exit for exit");
            string input = Console.ReadLine().Trim().ToUpperInvariant();
            if (input == "S")
            {
                mutex.WaitOne();

                eventHandle.Set();
                eventHandle.Reset();
                Thread.Sleep(200);
                eventHandle.Set();
                eventHandle.Reset();

                // Result: 1. waiting threads always register an event
                // 2. some threads register 2 events, most of threads register 1 event

                Thread.Sleep(1000);

                mutex.ReleaseMutex();

                ConsoleWriteLine("sent");
            }
            else if (input == "EXIT")
            {
                isCanceled = true;
                eventHandle.Set();
                eventHandle.Reset();
                return;
            }
        }
    }

    static bool isCanceled = false;
    private static void ThreadProc(string threadName)
    {
        ConsoleWriteLine($"{threadName} started");

        while (true)
        {
            eventHandle.WaitOne();
            if (isCanceled)
                return;

            mutex.WaitOne();
            Thread.Sleep(50);

            ConsoleWriteLine($"{threadName} received the event");

            Thread.Sleep(50);
            mutex.ReleaseMutex();
        }
    }

    private static void ConsoleWriteLine(string str)
    {
        lock (consoleSyncObj)
        {
            Console.WriteLine(str);
        }
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
