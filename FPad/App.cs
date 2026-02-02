using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using FPad.Encodings;
using FPad.Interaction;
using FPad.Settings;
using FPad.Xml;

namespace FPad
{
    internal static class App
    {
        public static string TITLE = "F-Pad";
        public static string SETTINGS_FOLDER = "F-Pad";
        public static AppSettings Settings = AppSettings.Default();

        public static int BUFFERSIZE = 512 << 10;

        public static string CmdLineFile { get; private set; }

        #region Private Fields

        private static XmlSerializer settingsSerializer;
        private static XmlWriterSettings xmlWriterSettings = new()
        {
            CloseOutput = false,
            Encoding = Encoding.Unicode,
            Indent = true,
            IndentChars = "  ",
            WriteEndDocumentOnClose = true
        };
        private static XmlReaderSettings xmlReaderSettings = new()
        {
            CloseInput = false
        };

        #endregion

        #region Messageboxes

        public static bool? Question(string msg)
        {
            DialogResult dlgResult = MessageBox.Show(msg, TITLE, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            return (dlgResult == DialogResult.Yes) ? true : (dlgResult == DialogResult.No ? false : null);
        }

        public static bool WarningQuestion(string msg)
        {
            DialogResult dlgResult = MessageBox.Show(msg, TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            return (dlgResult == DialogResult.Yes);
        }

        public static bool YNQuestion(string msg)
        {
            DialogResult dlgResult = MessageBox.Show(msg, TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return (dlgResult == DialogResult.Yes);
        }

        public static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error - " + TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region Settings

        public static bool SaveSettings()
        {
            if (!EnsureSettingsFolderExists())
                return false;

            try
            {
                SettingsDto dto = SettingsToDto(Settings);
                settingsSerializer ??= new XmlSerializer(typeof(SettingsDto));

                string settingsFilePath = GetSettingsFilePath();
                using (FileStream fs = new FileStream(settingsFilePath, FileMode.Create, FileAccess.Write, FileShare.None, BUFFERSIZE))
                {
                    XmlWriter xmlWriter = XmlWriter.Create(fs, xmlWriterSettings);
                    settingsSerializer.Serialize(xmlWriter, dto);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool LoadSettings()
        {
            string pathToFile = GetSettingsFilePath();
            try
            {
                if (File.Exists(pathToFile))
                {
                    settingsSerializer ??= new XmlSerializer(typeof(SettingsDto));
                    SettingsDto dto = null;

                    string settingsFilePath = GetSettingsFilePath();
                    using (FileStream fs = new FileStream(settingsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, BUFFERSIZE))
                    {
                        XmlReader xmlReader = XmlReader.Create(fs, xmlReaderSettings);
                        dto = settingsSerializer.Deserialize(xmlReader) as SettingsDto;
                    }

                    DtoToSettings(dto, Settings);
                    return true;
                }
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        #endregion

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Interactor.Startup();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            string[] cmdLineArgs = Environment.GetCommandLineArgs();
            if (cmdLineArgs.Length > 1) // The first is just full path to the app
            {
                CmdLineFile = cmdLineArgs[^1];

                if (!string.IsNullOrWhiteSpace(CmdLineFile))
                {
                    if (Interactor.FindAndActivateByCurrentDocumentPath(CmdLineFile))
                    {
                        Interactor.Shutdown();
                        return;
                    }
                }
            }

            LoadSettings();

            try
            {
                EncodingManager.Init();
            }
            catch (Exception ex)
            {
                ShowError(ex);
                Interactor.Shutdown();
                return;
            }

            Application.ApplicationExit += OnApplicationExit;
            Application.Run(new MainWindow());
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            if (Interactor.IsInitialized)
                Interactor.Shutdown();
        }

        #region Private: Settings

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
                    Wrap = settings.Wrap ? "1" : null
                },
                WindowPosition = WindowPositionToDto(settings.WindowPosition)
            };

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
            }

            WindowPositionSettings windowPosSettings = DtoToWindowPositionSettings(dto.WindowPosition);
            if (windowPosSettings != null)
            {
                dest.WindowPosition = windowPosSettings;
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

        #endregion
    }
}