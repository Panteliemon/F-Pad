using Panlingo.LanguageIdentification.Whatlang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Encodings;

public record LangKey(WhatlangLanguage Lang, WhatlangScript Alphabet);
