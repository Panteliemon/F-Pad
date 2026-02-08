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
using FPad.Xml;

namespace FPad.Settings;

public static class SettingsManager
{
    public const string SETTINGS_FOLDER = "F-Pad";

    private static Mutex settingsMutex;

    private static XmlSerializer settingsSerializer;
    private static XmlWriterSettings xmlWriterSettings;
    private static XmlReaderSettings xmlReaderSettings;

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

                    string settingsFilePath = GetSettingsFilePath();
                    using (FileStream fs = new FileStream(settingsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, App.BUFFERSIZE))
                    {
                        XmlReader xmlReader = XmlReader.Create(fs, xmlReaderSettings);
                        dto = settingsSerializer.Deserialize(xmlReader) as SettingsDto;
                    }

                    DtoToSettings(dto, result);
                }
            }
            catch (Exception ex)
            {
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
            Font = new FontDto()
            {
                Name = settings.FontFamily,
                Size = settings.FontSize,
                IsBold = settings.IsBold ? "1" : null,
                IsItalic = settings.IsItalic ? "1" : null
            },
            Text = new TextDto()
            {
                Wrap = settings.Wrap ? "1" : null,
                FindMatchCase = settings.FindMatchCase ? "1" : null,
                FindWholeWords = settings.FindWholeWords ? "1" : null
            },
            WindowPosition = WindowPositionToDto(settings.WindowPosition)
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

        if (dto.Font != null)
        {
            if (!string.IsNullOrEmpty(dto.Font.Name))
                dest.FontFamily = dto.Font.Name;

            if ((dto.Font.Size >= 5) && (dto.Font.Size <= 128))
                dest.FontSize = dto.Font.Size;

            dest.IsBold = dto.Font.IsBold == "1";
            dest.IsItalic = dto.Font.IsItalic == "1";
        }

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
            }
        }

        return null;
    }

    #endregion

    #endregion
}
