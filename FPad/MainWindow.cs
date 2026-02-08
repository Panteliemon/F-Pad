using FPad.Encodings;
using FPad.Interaction;
using FPad.Settings;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
        /// <summary>
        /// Valid until first change
        /// </summary>
        byte[] currentDocumentBytes;
        EncodingVm currentEncoding = null;

        bool enableSizingHandlers = false;
        bool enableTextChangeHandler = true;

        FormWindowState prevWindowState = FormWindowState.Normal;

        public MainWindow()
        {
            InitializeComponent();
            Icon = App.Icon;

            SelectionChangedBehavior textSelectionChanged = new(text);
            textSelectionChanged.SelectionChanged += Text_SelectionChanged;

            ApplySettings();
            ConstructEncodingMenu();

            if (!string.IsNullOrEmpty(App.CmdLineFile))
            {
                if (!LoadFile(App.CmdLineFile, false))
                    PrepareNew(false);
            }
            else
            {
                PrepareNew(false);
            }

            Interactor.Activate = Interactor_ActivateReceived;
        }

        #region Cross-Window communication

        public (int Start, int Length) GetTextSelection()
        {
            return (text.SelectionStart, text.SelectionLength);
        }

        public void SetTextSelection(int selectionStart, int selectionLength)
        {
            Activate();
            text.Focus();
            text.SelectionStart = selectionStart;
            text.SelectionLength = selectionLength;
            text.ScrollToCaret();
        }

        public string GetText()
        {
            return text.Text;
        }

        public void ChangeSearchSettings(bool matchCase, bool wholeWords)
        {
            bool changed = (matchCase != App.Settings.FindMatchCase)
                || (wholeWords != App.Settings.FindWholeWords);
            if (changed)
            {
                App.Settings.FindMatchCase = matchCase;
                App.Settings.FindWholeWords = wholeWords;
                if (App.SaveSettings(SettingsFlags.SearchSettings))
                    StatusBarShowSecondOrderSuccessMessage("Settings Saved");
                else
                    StatusBarShowSecondOrderErrorMessage("Error when saving settings. Settings not saved.");
            }
        }

        #endregion

        #region Event Handlers

        private void Interactor_ActivateReceived()
        {
            Invoke(() =>
            {
                if (WindowState == FormWindowState.Minimized)
                    WindowState = prevWindowState;
                Activate();
            });
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.WindowsShutDown)
            {
                if (!HandleUnsavedChanges())
                {
                    e.Cancel = true;
                }
            }

            if (!e.Cancel)
            {
                RememberWindowPosition();
                SettingsFlags settingsToSave = SettingsFlags.WindowPosition;
                if (!isNew)
                    settingsToSave |= SettingsFlags.FileWindowPosition;
                App.SaveSettings(settingsToSave, currentDocumentFullPath);
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            WindowPositionSettings windowPositionToRestore = App.Settings.WindowPosition;
            if (!string.IsNullOrEmpty(App.CmdLineFile) && !isNew
                && (App.Settings.Files != null))
            {
                // If successfully loaded some file from cmd line - apply this file's personal position
                string hash = StringUtils.GetPathHash(currentDocumentFullPath);
                WindowPositionSettings fileWindowPosition = App.Settings.Files
                    .Where(x => string.Equals(x.FullPathHash, hash, StringComparison.Ordinal))
                    .Select(x => x.WindowPosition)
                    .FirstOrDefault();
                if (fileWindowPosition != null)
                    windowPositionToRestore = fileWindowPosition;
            }

            ApplyWindowPosition(windowPositionToRestore);

            enableSizingHandlers = true;
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            if (enableSizingHandlers)
            {
                RememberNormalSize();
                if (WindowState != FormWindowState.Minimized)
                    prevWindowState = WindowState;
            }
        }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            if (enableSizingHandlers)
                RememberNormalSize();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (enableTextChangeHandler)
            {
                hasUnsavedChanges = true;
                currentDocumentBytes = null;
                UpdateTitle();
                UpdateStatusBar();
            }
        }

        private void Text_SelectionChanged(object sender, EventArgs e)
        {
            UpdateStatusBar();
        }

        #region Menu: File

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!HandleUnsavedChanges())
                return;

            PrepareNew(true);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!HandleUnsavedChanges())
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

                LoadFile(ofd.FileName, true);
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

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplaceForm.HideIfShown();
            FPad.FindForm.Show(this, GetTopRightForFindReplace());
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FPad.FindForm.HideIfShown();
            ReplaceForm.Show(this, GetTopRightForFindReplace());
        }

        private void wrapLinesMenuItem_Click(object sender, EventArgs e)
        {
            App.Settings.Wrap = !App.Settings.Wrap;

            if (App.SaveSettings(SettingsFlags.Wrap))
                StatusBarShowSecondOrderSuccessMessage("Settings Saved");
            else
                StatusBarShowSecondOrderErrorMessage("Error when saving settings. Settings not saved.");

            ApplySettings();
            UpdateStatusBar();
        }

        private void encodingMenuItemSelected(EncodingVm encodingVm)
        {
            if ((encodingVm != currentEncoding) && (currentEncoding != null))
            {
                if (!isNew)
                {
                    EncodingSwitchMethod? switchMethod = EncodingSwitchDialog.ShowDialog(this, encodingVm);
                    if (!switchMethod.HasValue) // Canceled
                        return;

                    if (switchMethod == EncodingSwitchMethod.Reinterpret)
                    {
                        byte[] bytes = currentDocumentBytes ?? currentEncoding.Encoding.GetBytes(text.Text);
                        enableTextChangeHandler = false;
                        text.Text = encodingVm.Encoding.GetString(bytes);
                        enableTextChangeHandler = true;

                        // Keep previous hasUnsavedChanges (reinterpretation != edit)
                        // Keep previous currentDocumentBytes (reinterpreted, not changed)

                        ResetSelection();
                    }
                    else // Use during save
                    {
                        // Consider this an edit, although the text doesn't change
                        currentDocumentBytes = null;
                        hasUnsavedChanges = true;
                    }
                }

                currentEncoding = encodingVm;
                UpdateEncodingMenuCheckboxes();
                UpdateTitle();
                UpdateStatusBar();
            }
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SettingsDialog.ShowADialog())
            {
                if (App.SaveSettings(SettingsFlags.Font))
                    StatusBarShowSecondOrderSuccessMessage("Settings Saved");
                else
                    StatusBarShowSecondOrderErrorMessage("Error when saving settings. Settings not saved.");

                ApplySettings();
            }
        }

        #endregion

        #endregion

        #region New, Load, Save

        private void PrepareNew(bool savePreviousFileSettings)
        {
            if (savePreviousFileSettings && !isNew)
            {
                RememberWindowPosition();
                App.SaveSettings(SettingsFlags.FileWindowPosition, currentDocumentFullPath);
            }

            text.Text = string.Empty;

            currentDocumentFileName = "new.txt";
            currentDocumentFullPath = string.IsNullOrEmpty(lastPathToFolder)
                ? Path.Combine(Environment.CurrentDirectory, currentDocumentFileName)
                : Path.Combine(lastPathToFolder, currentDocumentFileName);
            isNew = true;
            hasUnsavedChanges = false;
            currentDocumentBytes = null;
            UpdateTitle();

            currentEncoding = EncodingManager.DefaultEncoding;
            UpdateEncodingMenuCheckboxes();
            UpdateStatusBar();

            Interactor.UpdateCurrentDocumentFullPath(currentDocumentFullPath);
        }

        private bool LoadFile(string fileName, bool savePreviousFileSettings)
        {
            try
            {
                string fullPath = Path.GetFullPath(fileName);
                byte[] allBytes = File.ReadAllBytes(fullPath);

                if (savePreviousFileSettings && !isNew)
                {
                    RememberWindowPosition();
                    App.SaveSettings(SettingsFlags.FileWindowPosition, currentDocumentFullPath);
                }

                currentDocumentFullPath = fullPath;
                currentDocumentFileName = Path.GetFileName(fullPath);
                currentEncoding = EncodingManager.DetectEncoding(allBytes);
                UpdateEncodingMenuCheckboxes();

                text.Text = currentEncoding.Encoding.GetString(allBytes);

                currentDocumentBytes = allBytes; // after text change
                hasUnsavedChanges = false; // after text change
                isNew = false;
                ResetSelection();
                UpdateTitle();
                UpdateStatusBar();

                Interactor.UpdateCurrentDocumentFullPath(currentDocumentFullPath);

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

                (bool proceed, byte[] encodedBytes) = EncodeForSave(text.Text);
                if (!proceed)
                    return null;

                bool saveResult = UnsafeSave(destPath, encodedBytes);
                if (saveResult)
                {
                    if (!isNew)
                    {
                        RememberWindowPosition();
                        App.SaveSettings(SettingsFlags.FileWindowPosition, currentDocumentFullPath);
                    }

                    currentDocumentFullPath = destPath;
                    currentDocumentFileName = Path.GetFileName(destPath);
                    isNew = false;
                    hasUnsavedChanges = false;
                    currentDocumentBytes = encodedBytes;
                    UpdateTitle();
                    UpdateStatusBar();

                    Interactor.UpdateCurrentDocumentFullPath(currentDocumentFullPath);

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

        /// <summary>
        /// </summary>
        /// <returns>True - saved, False - not saved, Null - canceled</returns>
        private bool? ExecuteSave()
        {
            (bool proceed, byte[] encodedBytes) = EncodeForSave(text.Text);
            if (!proceed)
                return null;

            bool saveResult = UnsafeSave(currentDocumentFullPath, encodedBytes);
            if (saveResult)
            {
                isNew = false;
                hasUnsavedChanges = false;
                currentDocumentBytes = encodedBytes;
            }

            UpdateTitle();
            UpdateStatusBar();
            return saveResult;
        }

        private (bool proceed, byte[] encodedBytes) EncodeForSave(string allText)
        {
            // Implementation of Encoding sucks dick.
            // Internally it can only work with char arrays and cannot into spans.
            // Encoding.GetBytes(string): 2 waste extra char arrays created (1st for calculating
            // result length, 2nd for conversion itself. I wanna call GetByteCount and then GetBytes,
            // if I do it just like that - it uses 3 waste extra char arrays. What if my string is already 1 GB?
            // Therefore here is 1 waste extra char array so it doesn't shit more.
            char[] textAsArray = allText.ToCharArray();
            int bytesCount = currentEncoding.Encoding.GetByteCount(textAsArray);

            byte[] result = new byte[bytesCount + currentEncoding.Encoding.Preamble.Length];
            Span<byte> spanResult = result.AsSpan();
            currentEncoding.Encoding.Preamble.CopyTo(spanResult);
            currentEncoding.Encoding.GetBytes(textAsArray, spanResult[currentEncoding.Encoding.Preamble.Length..]);

            if (!currentEncoding.IsLossless)
            {
                string decoded = currentEncoding.Encoding.GetString(spanResult);
                if (decoded != allText)
                {
                    bool lossConfirmation = App.WarningQuestion($"Encoding {currentEncoding.DisplayName}"
                        + Environment.NewLine + "Some characters will be lost."
                        + Environment.NewLine + "Save anyway?");
                    if (!lossConfirmation)
                        return (false, null);
                }
            }

            return (true, result);
        }

        private bool UnsafeSave(string destPath, byte[] bytes)
        {
            try
            {
                File.WriteAllBytes(destPath, bytes);
                StatusBarShowSuccessMessage("SAVED");
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
        /// <returns>True - saved / not saved and we allow to proceed; Null - canceled</returns>
        private bool HandleUnsavedChanges()
        {
            if (hasUnsavedChanges)
            {
                bool? doSave = App.Question($"Save changes to {currentDocumentFileName}?");
                if (doSave == true)
                {
                    if (isNew)
                    {
                        if (ExecuteSaveAs() != true)
                            return false;
                    }
                    else
                    {
                        if (ExecuteSave() != true)
                            return false;
                    }
                }
                else if (!doSave.HasValue)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        private void ApplySettings()
        {
            text.Font = FontUtils.GetFontBySettings(App.Settings);

            text.WordWrap = App.Settings.Wrap;
            wrapLinesMenuItem.Checked = App.Settings.Wrap;
        }

        private void ApplyWindowPosition(WindowPositionSettings windowPosSettings)
        {
            if (windowPosSettings != null)
            {
                Top = windowPosSettings.Top;
                Left = windowPosSettings.Left;
                Height = windowPosSettings.Height;
                Width = windowPosSettings.Width;
                if (windowPosSettings.IsMaximized)
                {
                    WindowState = FormWindowState.Maximized;
                }
            }
        }

        private void ResetSelection()
        {
            text.SelectionStart = 0;
            text.SelectionLength = 0;
        }

        private void UpdateTitle()
        {
            if (hasUnsavedChanges)
                Text = currentDocumentFileName + "* – " + App.TITLE; // Em dash
            else
                Text = currentDocumentFileName + " – " + App.TITLE;
        }

        private void UpdateStatusBar()
        {
            encodingLabel.Text = currentEncoding?.DisplayName ?? string.Empty;
            wrapLabel.Visible = App.Settings.Wrap;
            modifiedLabel.Visible = hasUnsavedChanges;

            if (text.SelectionLength > 0)
            {
                (int lineStart, int charStart) = StringUtils.GetLineAndCol(text.Text, text.SelectionStart);
                (int lineEnd, int charEnd) = StringUtils.GetLineAndCol(text.Text, text.SelectionStart, lineStart, charStart, text.SelectionStart + text.SelectionLength);
                if (lineStart == lineEnd)
                    lineAndColLabel.Text = $"Line {lineStart + 1}, Col {charStart + 1} - Col {charEnd + 1}";
                else
                    lineAndColLabel.Text = $"Line {lineStart + 1}, Col {charStart + 1} - Line {lineEnd + 1}, Col {charEnd + 1}";
                labelSelection.Visible = true;
                labelSelection.Text = $"Sel {text.SelectionLength}";
            }
            else
            {
                (int lineIndex, int charIndex) = StringUtils.GetLineAndCol(text.Text, text.SelectionStart);
                lineAndColLabel.Text = $"Line {lineIndex + 1}, Col {charIndex + 1}";
                labelSelection.Visible = false;
            }
        }

        private void RememberWindowPosition()
        {
            App.Settings.WindowPosition ??= new WindowPositionSettings();
            RememberNormalSize();
            App.Settings.WindowPosition.IsMaximized = WindowState == FormWindowState.Maximized;
        }

        private void RememberNormalSize()
        {
            if (WindowState == FormWindowState.Normal)
            {
                App.Settings.WindowPosition ??= new WindowPositionSettings();
                App.Settings.WindowPosition.Top = Top;
                App.Settings.WindowPosition.Left = Left;
                App.Settings.WindowPosition.Height = Height;
                App.Settings.WindowPosition.Width = Width;
            }
        }

        private void ConstructEncodingMenu()
        {
            foreach (EncodingVm encodingVm in EncodingManager.Encodings)
            {
                if (encodingVm.Alphabet == null)
                {
                    ToolStripMenuItem menuItem = new(encodingVm.DisplayName, null,
                        (_, _) => encodingMenuItemSelected(encodingVm));
                    menuItem.Tag = encodingVm;
                    encodingToolStripMenuItem.DropDownItems.Add(menuItem);
                }
            }

            foreach (Alphabet alphabet in EncodingManager.Alphabets)
            {
                ToolStripMenuItem alphabetItem = new("ANSI " + alphabet.DisplayName);
                encodingToolStripMenuItem.DropDownItems.Add(alphabetItem);

                foreach (EncodingVm encodingVm in alphabet.Encodings)
                {
                    ToolStripMenuItem menuItem = new(encodingVm.DisplayName, null,
                        (_, _) => encodingMenuItemSelected(encodingVm));
                    menuItem.Tag = encodingVm;
                    alphabetItem.DropDownItems.Add(menuItem);
                }
            }
        }

        private bool UpdateEncodingMenuCheckboxes(ToolStripMenuItem parentMenuItem = null)
        {
            parentMenuItem ??= encodingToolStripMenuItem;

            bool anyChecked = false;
            foreach (ToolStripMenuItem item in parentMenuItem.DropDownItems)
            {
                bool subItemsChecked = UpdateEncodingMenuCheckboxes(item);
                item.Checked = subItemsChecked
                    || ((item.Tag == currentEncoding) && (currentEncoding != null));
                anyChecked = anyChecked || item.Checked;
            }

            return anyChecked;
        }

        #region Status Bar Messages

        int statusBarMessageId = 0;
        static readonly Padding statusLabelMarginForBorders = new Padding(2, 3, 0, 2);
        static readonly Padding statusLabelMarginBorderless = new Padding(2, 3, 2, 4);

        private void StatusBarShowSuccessMessage(string msg)
        {
            msgLabel.ForeColor = Color.White;
            msgLabel.BackColor = Color.FromArgb(0, 160, 0);
            msgLabel.Font = msgLabel.Font.ToBold();
            msgLabel.BorderSides = ToolStripStatusLabelBorderSides.None;
            msgLabel.Margin = statusLabelMarginBorderless;
            StatusBarShowMessage(msg);
        }

        private void StatusBarShowSecondOrderSuccessMessage(string msg)
        {
            msgLabel.ForeColor = Color.FromArgb(0, 160, 0);
            msgLabel.BackColor = SystemColors.Control;
            msgLabel.Font = msgLabel.Font.ToBold();
            msgLabel.BorderSides = ToolStripStatusLabelBorderSides.All;
            msgLabel.Margin = statusLabelMarginForBorders;
            StatusBarShowMessage(msg);
        }

        private void StatusBarShowSecondOrderErrorMessage(string msg)
        {
            msgLabel.ForeColor = Color.Red;
            msgLabel.BackColor = SystemColors.Control;
            msgLabel.Font = msgLabel.Font.ToBold();
            msgLabel.BorderSides = ToolStripStatusLabelBorderSides.All;
            msgLabel.Margin = statusLabelMarginForBorders;
            StatusBarShowMessage(msg);
        }

        private void StatusBarShowMessage(string msg)
        {
            msgLabel.Text = msg;
            int currentMessageId = ++statusBarMessageId;

            Task.Run(() =>
            {
                Task.Delay(3000).Wait();
                BeginInvoke(() =>
                {
                    if (statusBarMessageId == currentMessageId)
                    {
                        msgLabel.Text = string.Empty;
                        msgLabel.ForeColor = SystemColors.ControlText;
                        msgLabel.BackColor = SystemColors.Control;
                        // msgLabel.Font = msgLabel.Font.Unbold(); Uncomment if we will ever use regular font
                        msgLabel.BorderSides = ToolStripStatusLabelBorderSides.All;
                        msgLabel.Margin = statusLabelMarginForBorders;
                    }
                });
            });
        }

        #endregion

        private Point GetTopRightForFindReplace()
        {
            return text.PointToScreen(new Point(text.Width - 10, 0));
        }
    }
}
