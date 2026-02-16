using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using FPad.Encodings;
using FPad;

namespace Tests.FPad;

public class EncodingTests : IClassFixture<EncodingTestsFixture>
{
    private EncodingTestsFixture fixture;

    public EncodingTests(EncodingTestsFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void EncodingDetectionTest()
    {
        FileInfo[] txtFiles = fixture.TxtDir.GetFiles("*.txt");
        foreach (FileInfo fi in txtFiles.Where(x => !x.Name.StartsWith("_")))
        {
            EncodingVm expected = GetExpectedEncoding(fi.Name);

            byte[] bytes = File.ReadAllBytes(fi.FullName);
            EncodingVm detected = EncodingManager.DetectEncoding(bytes);

            if (detected != expected)
            {
                Assert.Fail($"Incorrect encoding detection {fi.Name}:{Environment.NewLine}Expected {expected.DisplayName}{Environment.NewLine}Detected {detected.DisplayName}");
            }
        }
    }

    private EncodingVm GetExpectedEncoding(string fileName)
    {
        EncodingTestsFixture.EncodingWrapper candidate = null;
        foreach (EncodingTestsFixture.EncodingWrapper wrapper in fixture.Wrappers)
        {
            if (wrapper.Search.FindFirstMatch(fileName) >= 0)
            {
                if (candidate != null)
                {
                    // If beginning the same - the longest wins
                    if (wrapper.Vm.Encoding.WebName.StartsWith(candidate.Vm.Encoding.WebName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        candidate = wrapper;
                    }
                    else if (candidate.Vm.Encoding.WebName.StartsWith(wrapper.Vm.Encoding.WebName))
                    {
                        // ok
                    }
                    else
                    {
                        Assert.Fail($"File name {fileName} contains identifiers of two encodings: {candidate.Search.SubStringToSearch} and {wrapper.Search.SubStringToSearch}. Only one is allowed.");
                    }
                }

                candidate = wrapper;
            }
        }

        if (candidate == null)
        {
            Assert.Fail($"File name {fileName} doesn't contain identifiers of any supported encodings{Environment.NewLine}See _codes.txt in output dir for available options");
        }

        return candidate?.Vm;
    }
}
