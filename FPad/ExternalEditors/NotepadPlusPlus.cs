using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPad.ExternalEditors;

internal class NotepadPlusPlus : ExternalEditorBase, IExternalEditor
{
    private static string[] registryPaths = [
        @"SOFTWARE\Notepad++",
        @"SOFTWARE\WOW6432Node\Notepad++" // exists but empty value on my machine
    ];

    public string DisplayName => "Notepad++";

    public string CreateCommandLineArgs(string pathToFile, int lineIndex, int charIndex)
    {
        StringBuilder sb = new();
        if (lineIndex >= 0)
        {
            sb.Append("-n");
            sb.Append((lineIndex + 1).ToString());
            sb.Append(' ');
        }
        
        if (charIndex >= 0)
        {
            sb.Append("-c");
            sb.Append((charIndex + 1).ToString());
            sb.Append(' ');
        }

        sb.Append(WrapIntoQuotesIfNeeded(pathToFile));
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
        foreach (string regPath in registryPaths)
        {
            try
            {
                using (RegistryKey regFolder = regRoot.OpenSubKey(regPath))
                {
                    if (regFolder != null)
                    {
                        string pathToFolder = regFolder.GetValue(string.Empty) as string;
                        if (!string.IsNullOrEmpty(pathToFolder))
                        {
                            string path = Path.Combine(pathToFolder, "notepad++.exe");
                            if (File.Exists(path))
                            {
                                PathToExe = path;
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            ct.ThrowIfCancellationRequested();
        }

        return false;
    }

    private bool DetectFromProgramFiles(string programFilesPath)
    {
        try
        {
            string path = Path.Combine(programFilesPath, "Notepad++", "notepad++.exe");
            if (File.Exists(path))
            {
                PathToExe = path;
                return true;
            }
        }
        catch (Exception ex)
        {
        }

        return false;
    }
}
