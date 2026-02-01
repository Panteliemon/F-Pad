using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Encodings;

public class EncodingVm
{
    /// <summary>
    /// Can be null
    /// </summary>
    public Alphabet Alphabet { get; }
    /// <summary>
    /// Underlying object
    /// </summary>
    public Encoding Encoding { get; }
    public string DisplayName { get; }

    public bool IsLossless { get; }

    internal EncodingVm(Encoding encoding, string displayName, bool isLossless)
    {
        Encoding = encoding;
        DisplayName = displayName;
        IsLossless = isLossless;
    }

    internal EncodingVm(Alphabet alphabet, Encoding encoding, string displayName)
    {
        ArgumentNullException.ThrowIfNull(alphabet);

        Alphabet = alphabet;
        Encoding = encoding;
        DisplayName = displayName;
        IsLossless = false;

        Alphabet.Encodings.Add(this);
    }
}
