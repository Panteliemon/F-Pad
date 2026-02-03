using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Settings;

public class FileSettings
{
    /// <summary>
    /// <see cref="StringUtils.GetPathHash(string)"/> of full path to the file
    /// </summary>
    // We don't store in open way list of files which the user has edited!
    public string FullPathHash { get; set; }
    /// <summary>
    /// When these settings last changed (for removing outdated data)
    /// </summary>
    public DateOnly LastChanged { get; set; }

    /// <summary>
    /// Window position when edited the file
    /// </summary>
    public WindowPositionSettings WindowPosition { get; set; }
}
