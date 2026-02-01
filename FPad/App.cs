using FPad.Encodings;
using FPad.Interaction;
using FPad.Xml;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace FPad
{
    internal static class App
    {
        public static string TITLE = "F-Pad";
        public static string SETTINGS_FOLDER = "F-Pad";
        public static Settings Settings = Settings.Default();

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

        private static SettingsDto SettingsToDto(Settings settings)
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
                WindowPosition = settings.WindowPositionHasValue ? new WindowPositionDto()
                {
                    Maximized = settings.WindowMaximized ? "1" : null,
                    Top = settings.WindowTop,
                    Left = settings.WindowLeft,
                    Height = settings.WindowHeight,
                    Width = settings.WindowWidth
                } : null
            };

            return result;
        }

        private static void DtoToSettings(SettingsDto dto, Settings dest)
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

            dest.WindowPositionHasValue = false;
            if (dto.WindowPosition != null)
            {
                dest.WindowPositionHasValue = true;
                dest.WindowMaximized = dto.WindowPosition.Maximized == "1";
                dest.WindowTop = dto.WindowPosition.Top;
                dest.WindowLeft = dto.WindowPosition.Left;
                dest.WindowHeight = dto.WindowPosition.Height;
                dest.WindowWidth = dto.WindowPosition.Width;
            }
        }

        #endregion
    }
}