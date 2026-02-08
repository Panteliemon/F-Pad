using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Settings;

[Flags]
public enum SettingsFlags
{
    Font = 1,
    Wrap = 2,
    WindowPosition = 4,
    FileWindowPosition = 8,
    SearchSettings = 16
}
