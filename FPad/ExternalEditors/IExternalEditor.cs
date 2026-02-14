using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPad.ExternalEditors;

public interface IExternalEditor
{
    string DisplayName { get; }
    Bitmap Icon { get; }
    string PathToExe { get; }
    /// <summary>
    /// Returns parameters for command line
    /// </summary>
    /// <param name="pathToFile">Path to file to open in external editor</param>
    /// <param name="lineIndex">Line to set cursor to</param>
    /// <param name="charIndex">Position within line to which to set cursor</param>
    /// <returns></returns>
    string CreateCommandLineArgs(string pathToFile, int lineIndex, int charIndex);

    /// <summary>
    /// Find installed editor on current PC. Returns true if successfully found.
    /// Initializes <see cref="Icon"/> and <see cref="PathToExe"/> if successfully found.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    bool Detect(CancellationToken ct);
}
