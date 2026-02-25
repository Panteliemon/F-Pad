using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad;

/// <summary>
/// Chars that we group together for purposes
/// </summary>
public enum ConseqCharType
{
    // Order is important
    Space = 0,
    NonWord = 1,
    Word = 2
}
