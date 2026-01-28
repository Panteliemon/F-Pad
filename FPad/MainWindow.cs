using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace FPad
{
    public partial class MainWindow : Form
    {
        bool isNew = false;
        bool hasUnsavedChanges = false;

        string currentDocumentFullPath = string.Empty;
        string currentDocumentFileName = string.Empty;
        string lastPathToFolder = string.Empty;

        bool enableSizingHandlers = false;

        public MainWindow()
        {
            InitializeComponent();

            ApplySettings();

            if (!string.IsNullOrEmpty(App.CmdLineFile))
            {
                if (!LoadFile(App.CmdLineFile))
                    newToolStripMenuItem_Click(this, EventArgs.Empty);
            }
            else
            {
                newToolStripMenuItem_Click(this, EventArgs.Empty);
            }
        }

        #region Event Handlers

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.WindowsShutDown)
            {
                if (HandleUnsavedChanges() != true)
                {
                    e.Cancel = true;
                }
            }

            if (!e.Cancel)
            {
                App.Settings.WindowMaximized = WindowState == FormWindowState.Maximized;
                RememberNormalSize();
                App.Settings.WindowPositionHasValue = true;
                App.SaveSettings();
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (App.Settings.WindowPositionHasValue)
            {
                Top = App.Settings.WindowTop;
                Left = App.Settings.WindowLeft;
                Height = App.Settings.WindowHeight;
                Width = App.Settings.WindowWidth;
                if (App.Settings.WindowMaximized)
                {
                    WindowState = FormWindowState.Maximized;
                }
            }

            enableSizingHandlers = true;
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            if (enableSizingHandlers)
                RememberNormalSize();
        }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            if (enableSizingHandlers)
                RememberNormalSize();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            hasUnsavedChanges = true;
        }

        #region Menu: File

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (HandleUnsavedChanges() != true)
                return;

            text.Text = string.Empty;
            currentDocumentFileName = "new.txt";
            currentDocumentFullPath = string.IsNullOrEmpty(lastPathToFolder)
                ? Path.Combine(Environment.CurrentDirectory, currentDocumentFileName)
                : Path.Combine(lastPathToFolder, currentDocumentFileName);
            isNew = true;
            hasUnsavedChanges = false;
            SetTitle();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (HandleUnsavedChanges() != true)
                return;

            OpenFileDialog ofd = new();
            ofd.AddExtension = true;
            ofd.AddToRecent = false;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.DereferenceLinks = false;
            ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = 0;
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.SelectReadOnly = true;
            ofd.ShowHiddenFiles = true;
            ofd.ShowPreview = false;
            ofd.ShowReadOnly = false;
            ofd.Title = "Open File - " + App.TITLE;
            ofd.ValidateNames = true;
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                lastPathToFolder = Path.GetDirectoryName(ofd.FileName);

                LoadFile(ofd.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isNew)
                ExecuteSaveAs();
            else
                ExecuteSave();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteSaveAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Menu: Edit

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (text.SelectionLength > 0)
            {
                Clipboard.SetText(text.Text.Substring(text.SelectionStart, text.SelectionLength));

                int newCursorPosition = text.SelectionStart;

                StringBuilder sb = new();
                sb.Append(text.Text.Substring(0, text.SelectionStart));
                sb.Append(text.Text.Substring(text.SelectionStart + text.SelectionLength));

                text.Text = sb.ToString();
                text.SelectionStart = newCursorPosition;
                text.SelectionLength = 0;
                text.ScrollToCaret();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (text.SelectionLength > 0)
            {
                Clipboard.SetText(text.Text.Substring(text.SelectionStart, text.SelectionLength));
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText() && (text.SelectionStart >= 0))
            {
                string clipboardText = Clipboard.GetText();
                int newCursorPosition = text.SelectionStart + clipboardText.Length;

                StringBuilder sb = new();
                sb.Append(text.Text.Substring(0, text.SelectionStart));
                sb.Append(Clipboard.GetText());
                sb.Append(text.Text.Substring(text.SelectionStart + text.SelectionLength));

                text.Text = sb.ToString();
                text.SelectionStart = newCursorPosition;
                text.SelectionLength = 0;
                text.ScrollToCaret();
            }
        }

        private void wrapLinesMenuItem_Click(object sender, EventArgs e)
        {
            App.Settings.Wrap = !App.Settings.Wrap;
            App.SaveSettings();
            ApplySettings();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SettingsDialog.ShowDialog(App.Settings))
            {
                App.SaveSettings();

                ApplySettings();
            }
        }

        #endregion

        #endregion

        #region Load and Save

        private bool LoadFile(string fileName)
        {
            try
            {
                string fullPath = Path.GetFullPath(fileName);
                string allText = File.ReadAllText(fullPath);

                currentDocumentFullPath = fullPath;
                currentDocumentFileName = Path.GetFileName(fullPath);
                SetTitle();

                text.Text = allText;
                hasUnsavedChanges = false; // after text is set
                isNew = false;
                ResetSelection();

                return true;
            }
            catch (Exception ex)
            {
                App.ShowError(ex);
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>True - saved, False - not saved, Null - canceled</returns>
        private bool? ExecuteSaveAs()
        {
            SaveFileDialog sfd = new();
            sfd.AddExtension = false;
            sfd.AddToRecent = false;
            sfd.CheckFileExists = false;
            sfd.CheckPathExists = true;
            sfd.DefaultExt = ".txt";
            sfd.DereferenceLinks = false;
            sfd.ExpandedMode = false;
            sfd.FileName = currentDocumentFileName;
            sfd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.FilterIndex = 0;
            sfd.InitialDirectory = Path.GetDirectoryName(currentDocumentFullPath);
            sfd.RestoreDirectory = false;
            sfd.ShowHiddenFiles = true;
            sfd.Title = "Save File - " + App.TITLE;
            sfd.ValidateNames = true;
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                lastPathToFolder = Path.GetDirectoryName(sfd.FileName);

                string destPath = Path.GetFullPath(sfd.FileName);
                if (File.Exists(destPath))
                {
                    bool overwrite = App.YNQuestion($"File {Path.GetFileName(destPath)} exists. Overwrite?");
                    if (!overwrite)
                        return null;
                }

                if (UnsafeSave(destPath))
                {
                    currentDocumentFullPath = destPath;
                    currentDocumentFileName = Path.GetFileName(destPath);
                    isNew = false;
                    hasUnsavedChanges = false;
                    SetTitle();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return null;
            }
        }

        private bool ExecuteSave()
        {
            bool result = UnsafeSave(currentDocumentFullPath);
            if (result)
            {
                isNew = false;
                hasUnsavedChanges = false;
            }

            return result;
        }

        private bool UnsafeSave(string destPath)
        {
            try
            {
                File.WriteAllText(destPath, text.Text);
                return true;
            }
            catch (Exception ex)
            {
                App.ShowError(ex);
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>True - saved, False - not saved, Null - canceled</returns>
        private bool? HandleUnsavedChanges()
        {
            if (hasUnsavedChanges)
            {
                bool? doSave = App.Question($"Save changes to {currentDocumentFileName}?");
                if (doSave == true)
                {
                    if (isNew)
                        return ExecuteSaveAs();
                    else
                        return ExecuteSave();
                }
                else if (!doSave.HasValue)
                {
                    return null;
                }
            }

            // No changes - "saved"
            // Answered "don't save" - "saved"
            return true;
        }

        #endregion

        private void ApplySettings()
        {
            text.Font = FontUtils.GetFontBySettings(App.Settings);

            text.WordWrap = App.Settings.Wrap;
            wrapLinesMenuItem.Checked = App.Settings.Wrap;
        }

        private void ResetSelection()
        {
            text.SelectionStart = 0;
            text.SelectionLength = 0;
        }

        private void SetTitle()
        {
            Text = currentDocumentFileName + " - " + App.TITLE;
        }

        private void RememberNormalSize()
        {
            if (WindowState == FormWindowState.Normal)
            {
                App.Settings.WindowTop = Top;
                App.Settings.WindowLeft = Left;
                App.Settings.WindowHeight = Height;
                App.Settings.WindowWidth = Width;
            }
        }
    }
}
