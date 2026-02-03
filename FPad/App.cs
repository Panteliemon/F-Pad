using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FPad.Encodings;
using FPad.Interaction;
using FPad.Settings;

namespace FPad
{
    internal static class App
    {
        public const string TITLE = "F-Pad";
        public const int BUFFERSIZE = 512 << 10;

        public static AppSettings Settings { get; private set; }
        public static string CmdLineFile { get; private set; }

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

        public static bool SaveSettings()
        {
            return SettingsManager.Modify(destSettings =>
            {
                destSettings.FontFamily = Settings.FontFamily;
                destSettings.FontSize = Settings.FontSize;
                destSettings.IsBold = Settings.IsBold;
                destSettings.IsItalic = Settings.IsItalic;
                destSettings.Wrap = Settings.Wrap;

                destSettings.WindowPosition ??= Settings.WindowPosition;
            });
        }

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
    }
}