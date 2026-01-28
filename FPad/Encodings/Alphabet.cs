using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Encodings;

/// <summary>
/// Group of encodings for ANSI
/// </summary>
public class Alphabet
{
    public string DisplayName { get; }
    public List<EncodingVm> Encodings { get; } = new();

    public Alphabet(string displayName)
    {
        DisplayName = displayName;
    }
}
