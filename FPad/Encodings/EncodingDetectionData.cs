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
    /// Not null if detectable by language
    /// </summary>
    public HashSet<WhatlangLanguage> Languages { get; }

    internal EncodingDetectionData(EncodingVm vm, string udeCode)
    {
        Vm = vm;
        UdeCode = udeCode;
    }

    internal EncodingDetectionData(EncodingVm vm, IEnumerable<WhatlangLanguage> languages)
    {
        Vm = vm;
        if (languages != null)
        {
            Languages = [.. languages];
        }
    }
}
