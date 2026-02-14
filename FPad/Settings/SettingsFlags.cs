using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Settings;

[Flags]
public enum SettingsFlags
{
    General = 1,
    Font = 2,
    Wrap = 4,
    WindowPosition = 8,
    FileWindowPosition = 16,
    SearchSettings = 32
}
