using FPad.Encodings;
using FPad.Interaction;
using FPad.Settings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace FPad
{
    internal static class App
    {
        public const string TITLE = "F-Pad";
        public const int BUFFERSIZE = 512 << 10;

        // We first collect all info into here,
        // but when we save we only save those parts of settings which we consider actual.
        public static AppSettings Settings { get; private set; }
        public static string CmdLineFile { get; private set; }

        public static string LastSearchStr { get; set; }
        public static string LastReplaceToStr { get; set; }

        public static Icon Icon { get; private set; }
        public static Version Version { get; set; }
        public static DateTime BuildDate { get; set; }

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

        public static bool SaveSettings(SettingsFlags flags, string currentFileFullPath = null)
        {
            return SettingsManager.Modify(destSettings =>
            {
                if ((flags & SettingsFlags.Font) != 0)
                {
                    destSettings.FontFamily = Settings.FontFamily;
                    destSettings.FontSize = Settings.FontSize;
                    destSettings.IsBold = Settings.IsBold;
                    destSettings.IsItalic = Settings.IsItalic;
                }

                if ((flags & SettingsFlags.Wrap) != 0)
                {
                    destSettings.Wrap = Settings.Wrap;
                }

                if (((flags & SettingsFlags.WindowPosition) != 0) && (Settings.WindowPosition != null))
                    destSettings.WindowPosition = Settings.WindowPosition.Clone();

                if (((flags & SettingsFlags.FileWindowPosition) != 0)
                    && !string.IsNullOrEmpty(currentFileFullPath))
                {
                    string hash = StringUtils.GetPathHash(currentFileFullPath);

                    // Add/update this file with our root WindowPosition,
                    // because we don't actualize per-file records that we hold.
                    destSettings.Files ??= new List<FileSettings>();
                    FileSettings fileSettings = destSettings.Files.FirstOrDefault(x => x.FullPathHash == hash);
                    if (fileSettings == null)
                    {
                        fileSettings = new FileSettings()
                        {
                            FullPathHash = hash
                        };
                        destSettings.Files.Add(fileSettings);
                    }

                    fileSettings.LastChanged = DateOnly.FromDateTime(DateTime.Today);
                    fileSettings.WindowPosition = Settings.WindowPosition?.Clone();
                }

                if ((flags & SettingsFlags.SearchSettings) != 0)
                {
                    destSettings.FindMatchCase = Settings.FindMatchCase;
                    destSettings.FindWholeWords = Settings.FindWholeWords;
                }
            });
        }

        [STAThread]
        static void Main()
        {
            Interactor.Startup();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            Version = Assembly.GetExecutingAssembly().GetName().Version;
            BuildDateAttribute buildDateAttr = Assembly.GetExecutingAssembly().GetCustomAttribute<BuildDateAttribute>();
            if (buildDateAttr != null)
                BuildDate = buildDateAttr.LocalValue;

            
            Icon = LoadIcon("FPad.Resources.f-pad.ico");

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

            Settings = SettingsManager.Read();

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

        private static Icon LoadIcon(string identifier)
        {
            using (Stream iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FPad.Resources.f-pad.ico"))
            {
                if (iconStream != null)
                {
                    return new Icon(iconStream);
                }
            }

            return null;
        }

        public static Image LoadImage(string imgName)
        {
            using (Stream imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"FPad.Resources.{imgName}"))
            {
                if (imageStream != null)
                {
                    return Image.FromStream(imageStream);
                }
            }

            return null;
        }
    }
}