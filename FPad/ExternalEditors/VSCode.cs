using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPad.ExternalEditors;

internal class VSCode : ExternalEditorBase, IExternalEditor
{
    private static string[] uninstallPaths = [
        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
        @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
    ];

    public string DisplayName => "Visual Studio Code";

    public string CreateCommandLineArgs(string pathToFile, int lineIndex, int charIndex)
    {
        if ((lineIndex >= 0) || (charIndex >= 0))
        {
            StringBuilder sb = new();
            sb.Append("--goto ");
            sb.Append(WrapIntoQuotesIfNeeded(pathToFile));
            sb.Append(':');
            sb.Append(lineIndex >= 0 ? (lineIndex + 1).ToString() : "1");
            if (charIndex >= 0)
            {
                sb.Append(':');
                sb.Append((charIndex + 1).ToString());
            }

            return sb.ToString();
        }
        else
        {
            return WrapIntoQuotesIfNeeded(pathToFile);
        }
    }

    public bool Detect(CancellationToken ct)
    {
        bool detected = true;
        if (!DetectFromRegistry(Registry.CurrentUser, ct))
        {
            if (!DetectFromRegistry(Registry.LocalMachine, ct))
            {
                if (!DetectFromProgramFiles(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)))
                {
                    if (!DetectFromProgramFiles(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Programs")))
                    {
                        detected = false;
                    }
                }
            }
        }

        ct.ThrowIfCancellationRequested();
        if (detected)
        {
            Icon = ExtractIcon(PathToExe);
        }

        return detected;
    }

    private bool DetectFromRegistry(RegistryKey regRoot, CancellationToken ct)
    {
        foreach (string regPath in uninstallPaths)
        {
            try
            {
                using (RegistryKey uninstallRootKey = regRoot.OpenSubKey(regPath))
                {
                    if (uninstallRootKey != null)
                    {
                        string[] uninstallSubKeys = uninstallRootKey.GetSubKeyNames();
                        ct.ThrowIfCancellationRequested();

                        foreach (string uninstallSubKeyName in uninstallSubKeys)
                        {
                            try
                            {
                                using (RegistryKey currentSubKey = uninstallRootKey.OpenSubKey(uninstallSubKeyName))
                                {
                                    string displayName = currentSubKey?.GetValue("DisplayName") as string;
                                    if ((displayName != null) && displayName.Contains("Visual Studio Code", StringComparison.InvariantCulture))
                                    {
                                        string installLocation = currentSubKey.GetValue("InstallLocation") as string;
                                        if (!string.IsNullOrEmpty(installLocation))
                                        {
                                            string pathToExe = Path.Combine(installLocation, "Code.exe");
                                            if (File.Exists(pathToExe))
                                            {
                                                PathToExe = pathToExe;
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                App.Discard(ex);
                            }

                            ct.ThrowIfCancellationRequested();
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                App.Discard(ex);
            }
        }

        return false;
    }

    private bool DetectFromProgramFiles(string programFilesPath)
    {
        try
        {
            string path = Path.Combine(programFilesPath, "Microsoft VS Code", "Code.exe");
            if (File.Exists(path))
            {
                PathToExe = path;
                return true;
            }
        }
        catch (Exception ex)
        {
            App.Discard(ex);
        }

        return false;
    }
}
