using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using FPad.Settings.Print;
using FPad.Settings.Xml;

namespace FPad.Settings;

public static class SettingsManager
{
    public const string SETTINGS_FOLDER = "F-Pad";

    private static Mutex settingsMutex;

    private static XmlSerializer settingsSerializer;
    private static XmlWriterSettings xmlWriterSettings;
    private static XmlReaderSettings xmlReaderSettings;
    private static Dictionary<string, FileNameContent> fileNameContentCodes;
    private static Dictionary<string, HorizontalAlignment> horizontalAlignmentCodes;

    static SettingsManager()
    {
        settingsMutex = new Mutex(false, "FPAD_settings_mutex");
        xmlWriterSettings = new XmlWriterSettings()
        {
            CloseOutput = false,
            Encoding = Encoding.Unicode,
            Indent = true,
            IndentChars = "  ",
            WriteEndDocumentOnClose = true
        };
        xmlReaderSettings = new XmlReaderSettings()
        {
            CloseInput = false
        };

        fileNameContentCodes = new Dictionary<string, FileNameContent>
        {
            { "N", FileNameContent.Name },
            { "NE", FileNameContent.NameExt },
            { "FP", FileNameContent.FullPath }
        };

        horizontalAlignmentCodes = new Dictionary<string, HorizontalAlignment>
        {
            { "L", HorizontalAlignment.Left },
            { "C", HorizontalAlignment.Center },
            { "R", HorizontalAlignment.Right }
        };
    }

    public static AppSettings Read()
    {
        AppSettings result = AppSettings.Default();

        CriticalSection(() =>
        {
            string pathToFile = GetSettingsFilePath();
            try
            {
                if (File.Exists(pathToFile))
                {
                    settingsSerializer ??= new XmlSerializer(typeof(SettingsDto));
                    SettingsDto dto = null;

                    using (FileStream fs = new FileStream(pathToFile, FileMode.Open, FileAccess.Read, FileShare.Read, App.BUFFERSIZE))
                    {
                        XmlReader xmlReader = XmlReader.Create(fs, xmlReaderSettings);
                        dto = settingsSerializer.Deserialize(xmlReader) as SettingsDto;
                    }

                    DtoToSettings(dto, result);
                }
            }
            catch (Exception ex)
            {
                App.Discard(ex);
            }
        });

        return result;
    }

    public static bool Modify(Action<AppSettings> modifyAction)
    {
        bool success = false;
        CriticalSection(() =>
        {
            if (EnsureSettingsFolderExists())
            {
                settingsSerializer ??= new XmlSerializer(typeof(SettingsDto));
                AppSettings settings = AppSettings.Default();

                string pathToFile = GetSettingsFilePath();
                if (File.Exists(pathToFile))
                {
                    SettingsDto readDto = null;

                    using (FileStream fs = new FileStream(pathToFile, FileMode.Open, FileAccess.Read, FileShare.Read, App.BUFFERSIZE))
                    {
                        XmlReader xmlReader = XmlReader.Create(fs, xmlReaderSettings);
                        readDto = settingsSerializer.Deserialize(xmlReader) as SettingsDto;
                    }

                    DtoToSettings(readDto, settings);
                }

                modifyAction(settings);

                SettingsDto writeDto = SettingsToDto(settings);
                using (FileStream fs = new FileStream(pathToFile, FileMode.Create, FileAccess.Write, FileShare.None, App.BUFFERSIZE))
                {
                    XmlWriter xmlWriter = XmlWriter.Create(fs, xmlWriterSettings);
                    settingsSerializer.Serialize(xmlWriter, writeDto);
                }

                success = true;
            }
        });

        return success;
    }

    #region Private

    private static void CriticalSection(Action criticalAction)
    {
        settingsMutex.WaitOne();
        try
        {
            criticalAction.Invoke();
        }
        catch (Exception ex)
        {
            App.Discard(ex);
            settingsMutex.ReleaseMutex();
            throw;
        }

        settingsMutex.ReleaseMutex();
    }

    private static bool EnsureSettingsFolderExists()
    {
        string appDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string settingsFolderPath = Path.Combine(appDataLocal, SETTINGS_FOLDER);
        try
        {
            if (!Directory.Exists(settingsFolderPath))
            {
                Directory.CreateDirectory(settingsFolderPath);
            }

            return true;
        }
        catch (Exception ex)
        {
            App.Discard(ex);
            return false;
        }
    }

    private static string GetSettingsFilePath()
    {
        string appDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string settingsFolderPath = Path.Combine(appDataLocal, SETTINGS_FOLDER);
        return Path.Combine(settingsFolderPath, "settings.xml");
    }

    #region Dto Conversion

    private static SettingsDto SettingsToDto(AppSettings settings)
    {
        SettingsDto result = new()
        {
            General = new GeneralDto()
            {
                AutoReload = settings.AutoReload ? "1" : null,
                Encoding = settings.DefaultEncodingWebName
            },
            Font = FontSettingsToDto(settings.Font),
            Text = new TextDto()
            {
                Wrap = settings.Wrap ? "1" : null,
                FindMatchCase = settings.FindMatchCase ? "1" : null,
                FindWholeWords = settings.FindWholeWords ? "1" : null
            },
            WindowPosition = WindowPositionToDto(settings.WindowPosition),
            Print = PrintSettingsToDto(settings.PrintSettings)
        };

        if (settings.Files != null)
        {
            result.Files = settings.Files.Select(x => FileSettingsToDto(x)).ToList();
        }

        return result;
    }

    private static void DtoToSettings(SettingsDto dto, AppSettings dest)
    {
        if (dto == null)
            return;

        if (dto.General != null)
        {
            dest.AutoReload = dto.General.AutoReload == "1";
            dest.DefaultEncodingWebName = dto.General.Encoding;
        }

        DtoToFontSettings(dto.Font, dest.Font);

        if (dto.Text != null)
        {
            dest.Wrap = dto.Text.Wrap == "1";
            dest.FindMatchCase = dto.Text.FindMatchCase == "1";
            dest.FindWholeWords = dto.Text.FindWholeWords == "1";
        }

        WindowPositionSettings windowPosSettings = DtoToWindowPositionSettings(dto.WindowPosition);
        if (windowPosSettings != null)
        {
            dest.WindowPosition = windowPosSettings;
        }

        DtoToPrintSettings(dto.Print, dest.PrintSettings);

        if ((dto.Files != null) && (dto.Files.Count > 0))
        {
            dest.Files ??= new List<FileSettings>();
            foreach (FileDto fileDto in dto.Files)
            {
                FileSettings fileSettings = DtoToFileSettings(fileDto);
                if (fileSettings != null)
                {
                    int existingIndex = dest.Files.FindIndex(x => string.Equals(x.FullPathHash, fileSettings.FullPathHash, StringComparison.Ordinal));
                    if (existingIndex >= 0)
                        dest.Files[existingIndex] = fileSettings;
                    else
                        dest.Files.Add(fileSettings);
                }
            }

            dest.Files.Sort((x, y) => string.CompareOrdinal(x.FullPathHash, y.FullPathHash));
        }
    }

    private static WindowPositionDto WindowPositionToDto(WindowPositionSettings windowPosition)
    {
        if (windowPosition != null)
        {
            return new WindowPositionDto()
            {
                Top = windowPosition.Top,
                Left = windowPosition.Left,
                Height = windowPosition.Height,
                Width = windowPosition.Width,
                Maximized = windowPosition.IsMaximized ? "1" : null
            };
        }

        return null;
    }

    private static WindowPositionSettings DtoToWindowPositionSettings(WindowPositionDto dto)
    {
        if (dto != null)
        {
            return new WindowPositionSettings()
            {
                Top = dto.Top,
                Left = dto.Left,
                Height = dto.Height,
                Width = dto.Width,
                IsMaximized = dto.Maximized == "1"
            };
        }

        return null;
    }

    private static FileDto FileSettingsToDto(FileSettings fileSettings)
    {
        return new FileDto()
        {
            Path = fileSettings.FullPathHash,
            Date = fileSettings.LastChanged.DayNumber,
            WindowPosition = WindowPositionToDto(fileSettings.WindowPosition)
        };
    }

    private static FileSettings DtoToFileSettings(FileDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Path))
        {
            try
            {
                DateOnly lastChanged = DateOnly.FromDayNumber(dto.Date);
                return new FileSettings()
                {
                    FullPathHash = dto.Path,
                    LastChanged = lastChanged,
                    WindowPosition = DtoToWindowPositionSettings(dto.WindowPosition)
                };
            }
            catch (Exception ex)
            {
                App.Discard(ex);
            }
        }

        return null;
    }

    private static FontDto FontSettingsToDto(FontSettings fontSettings)
    {
        return new FontDto()
        {
            Name = fontSettings.Family,
            Size = fontSettings.Size,
            IsBold = fontSettings.IsBold ? "1" : null,
            IsItalic = fontSettings.IsItalic ? "1" : null
        };
    }

    private static void DtoToFontSettings(FontDto dto, FontSettings dest)
    {
        if (dto != null)
        {
            if (!string.IsNullOrEmpty(dto.Name))
                dest.Family = dto.Name;

            if ((dto.Size >= FontSettings.MIN_SIZE) && (dto.Size <= FontSettings.MAX_SIZE))
                dest.Size = dto.Size;

            dest.IsBold = dto.IsBold == "1";
            dest.IsItalic = dto.IsItalic == "1";
        }
    }

    private static PrintDto PrintSettingsToDto(PrintSettings printSettings)
    {
        return new PrintDto()
        {
            FileName = new PrintFileNameDto()
            {
                Include = printSettings.IncludeFileName ? "1" : null,
                Option = fileNameContentCodes.FirstOrDefault(kvp => kvp.Value == printSettings.FileNameContent).Key,
                Font = FontSettingsToDto(printSettings.FileNameFont)
            },
            PageNumber = new PrintPageNumberDto()
            {
                Include = printSettings.IncludePageNumber ? "1" : null,
                UseTemplate = printSettings.UsePageNumberTemplate ? "1" : null,
                Template = printSettings.PageNumberTemplate?.Trim(),
                Align = horizontalAlignmentCodes.FirstOrDefault(kvp => kvp.Value == printSettings.PageNumberAlignment).Key,
                Font = FontSettingsToDto(printSettings.PageNumberFont)               
            }
        };
    }

    private static void DtoToPrintSettings(PrintDto dto, PrintSettings dest)
    {
        if (dto != null)
        {
            if (dto.FileName != null)
            {
                dest.IncludeFileName = dto.FileName.Include == "1";

                if ((dto.FileName.Option != null)
                    && fileNameContentCodes.TryGetValue(dto.FileName.Option, out FileNameContent fnContent))
                {
                    dest.FileNameContent = fnContent;
                }

                DtoToFontSettings(dto.FileName.Font, dest.FileNameFont);
            }

            if (dto.PageNumber != null)
            {
                dest.IncludePageNumber = dto.PageNumber.Include == "1";
                dest.UsePageNumberTemplate = dto.PageNumber.UseTemplate == "1";
                if (dto.PageNumber.Template != null)
                    dest.PageNumberTemplate = dto.PageNumber.Template;

                if ((dto.PageNumber.Align != null)
                    && horizontalAlignmentCodes.TryGetValue(dto.PageNumber.Align, out HorizontalAlignment pnAlignment))
                {
                    dest.PageNumberAlignment = pnAlignment;
                }

                DtoToFontSettings(dto.PageNumber.Font, dest.PageNumberFont);
            }
        }
    }

    #endregion

    #endregion
}
