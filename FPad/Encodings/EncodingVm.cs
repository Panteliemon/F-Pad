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

    public bool StartsWithPreamble(ReadOnlySpan<byte> bytes)
    {
        if ((Encoding.Preamble.Length > 0) && (bytes.Length >= Encoding.Preamble.Length))
        {
            for (int i = 0; i < Encoding.Preamble.Length; i++)
            {
                if (Encoding.Preamble[i] != bytes[i])
                    return false;
            }

            return true;
        }

        return false;
    }

    public string FileBytesToString(ReadOnlySpan<byte> allFileBytes)
    {
        int byteDecodeStart = 0;
        if (StartsWithPreamble(allFileBytes))
            byteDecodeStart = Encoding.Preamble.Length;
        return Encoding.GetString(allFileBytes[byteDecodeStart..]);
    }

    public byte[] StringToFileBytes(string allText)
    {
        // Implementation of Encoding sucks dick.
        // Internally it can only work with char arrays and cannot into spans.
        // Encoding.GetBytes(string): 2 waste extra char arrays created (1st for calculating
        // result length, 2nd for conversion itself). I wanna call GetByteCount and then GetBytes,
        // if I do it just like that - it uses 3 waste extra char arrays. What if my string is already 1 GB?
        // Therefore here is 1 waste extra char array so it doesn't shit more.
        char[] textAsArray = allText.ToCharArray();
        int textBytesCount = Encoding.GetByteCount(textAsArray);

        byte[] result = new byte[textBytesCount + Encoding.Preamble.Length];
        Span<byte> spanResult = result.AsSpan();
        Encoding.Preamble.CopyTo(spanResult);
        Encoding.GetBytes(textAsArray, spanResult[Encoding.Preamble.Length..]);

        return result;
    }
}
