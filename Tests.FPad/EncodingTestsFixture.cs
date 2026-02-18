using FPad;
using FPad.Encodings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Tests.FPad.EncodingTests;

namespace Tests.FPad;

public class EncodingTestsFixture
{
    public record EncodingWrapper(EncodingVm Vm, StringSearch Search);

    public DirectoryInfo TxtDir { get; }
    public List<EncodingWrapper> Wrappers { get; }

    public EncodingTestsFixture()
    {
        string pathToCurrentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        TxtDir = new DirectoryInfo(Path.Combine(pathToCurrentDir, "Txt"));

        EncodingManager.Init();
        Wrappers = EncodingManager.Encodings.Select(x => new EncodingWrapper(
            x, new StringSearch(x.Encoding.WebName, false)
        )).ToList();

        string[] codes = EncodingManager.Encodings.Select(x => x.Encoding.WebName).ToArray();
        string codesLines = $"Generated {DateTime.UtcNow} UTC{Environment.NewLine}Supported encoding identifiers are:"
            + Environment.NewLine + string.Join(Environment.NewLine, codes);
        File.WriteAllText(Path.Combine(TxtDir.FullName, "_codes.txt"), codesLines);
    }
}
