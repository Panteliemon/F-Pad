using Panlingo.LanguageIdentification.Whatlang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Encodings;

internal class EncodingDetectionData
{
    public EncodingVm Vm { get; }

    /// <summary>
    /// Not null if detectable by Ude
    /// </summary>
    public string UdeCode { get; }
    /// <summary>
    /// Not empty if encoding has BOM
    /// </summary>
    public ReadOnlyMemory<byte> BOM { get; }
    /// <summary>
    /// Not null if detectable by language
    /// </summary>
    public HashSet<WhatlangLanguage> Languages { get; }

    internal EncodingDetectionData(EncodingVm vm, string udeCode, byte[] bom = null)
    {
        Vm = vm;
        UdeCode = udeCode;
        BOM = bom;
    }

    internal EncodingDetectionData(EncodingVm vm, IEnumerable<WhatlangLanguage> languages)
    {
        Vm = vm;
        if (languages != null)
        {
            Languages = [.. languages];
        }
    }

    public bool IsBOM(ReadOnlySpan<byte> bytes)
    {
        if ((BOM.Length > 0) && (bytes.Length == BOM.Length))
        {
            for (int i=0; i<BOM.Length; i++)
            {
                if (BOM.Span[i] != bytes[i])
                    return false;
            }

            return true;
        }

        return false;
    }
}
