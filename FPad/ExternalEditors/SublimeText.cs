using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPad.ExternalEditors;

internal class SublimeText : ExternalEditorBase, IExternalEditor
{
    private static string[] uninstallPaths = [
        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
        @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
    ];

    public string DisplayName => "Sublime Text";

    public string CreateCommandLineArgs(string pathToFile, int lineIndex, int charIndex)
    {
        StringBuilder sb = new();
        sb.Append(WrapIntoQuotesIfNeeded(pathToFile));
        if (lineIndex >= 0)
        {
            sb.Append(':');
            sb.Append((lineIndex + 1).ToString());
            if (charIndex >= 0)
            {
                sb.Append(':');
                sb.Append((charIndex + 1).ToString());
            }
        }

        return sb.ToString();
    }

    public bool Detect(CancellationToken ct)
    {
        bool detected = true;
        if (!DetectFromRegistry(Registry.LocalMachine, ct))
        {
            if (!DetectFromRegistry(Registry.CurrentUser, ct))
            {
                if (!DetectFromProgramFiles(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)))
                {
                    if (!DetectFromProgramFiles(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)))
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
                                    if ((displayName != null) && displayName.Contains("Sublime Text", StringComparison.InvariantCulture))
                                    {
                                        string installLocation = currentSubKey.GetValue("InstallLocation") as string;
                                        if (!string.IsNullOrEmpty(installLocation))
                                        {
                                            string pathToExe = Path.Combine(installLocation, "sublime_text.exe");
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
            string path = Path.Combine(programFilesPath, "Sublime Text", "sublime_text.exe");
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
