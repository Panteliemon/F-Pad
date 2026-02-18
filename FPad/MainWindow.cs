using FPad.Encodings;
using FPad.ExternalEditors;
using FPad.Interaction;
using FPad.Settings;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad
{
    public partial class MainWindow : Form
    {
        bool isNew = false;
        bool hasUnsavedChanges = false;
        bool isExternallyModified = false;

        string currentDocumentFullPath = string.Empty;
        string currentDocumentFileName = string.Empty;
        string lastPathToFolder = string.Empty;
        /// <summary>
        /// Valid until first change
        /// </summary>
        byte[] currentDocumentBytes;
        /// <summary>
        /// Null if <see cref="isNew"/>
        /// </summary>
        EncodingVm initialEncoding = null;
        bool fileContainsPreamble;
        EncodingVm currentEncoding = null;
        FileWatcher currentDocumentWatcher;

        bool enableSizingHandlers = false;
        bool enableTextChangeHandler = true;

        FormWindowState prevWindowState = FormWindowState.Normal;

        CancellationTokenSource externalEditorsLoadingCts;

        public MainWindow()
        {
            InitializeComponent();
            Icon = App.Icon;

            // resized 48->16, bilinear
            newToolStripMenuItem.Image = App.LoadImage("b16_new.png");
            openToolStripMenuItem.Image = App.LoadImage("b16_open.png");
            saveToolStripMenuItem.Image = App.LoadImage("b16_save.png");
            saveAsToolStripMenuItem.Image = App.LoadImage("b16_saveas.png");
            cutToolStripMenuItem.Image = App.LoadImage("b16_cut.png");
            cutContextMenuItem.Image = cutToolStripMenuItem.Image;
            copyToolStripMenuItem.Image = App.LoadImage("b16_copy.png");
            copyContextMenuItem.Image = copyToolStripMenuItem.Image;
            pasteToolStripMenuItem.Image = App.LoadImage("b16_paste.png");
            pasteContextMenuItem.Image = pasteToolStripMenuItem.Image;
            findToolStripMenuItem.Image = App.LoadImage("b16_find.png");
            encodingToolStripMenuItem.Image = App.LoadImage("b16_encoding.png");
            preferencesToolStripMenuItem.Image = App.LoadImage("b16_settings.png");
            aboutToolStripMenuItem.Image = App.LoadImage("b16_about.png");

            ApplySettings();
            ConstructEncodingMenu();

            if (!string.IsNullOrEmpty(App.CmdLineFile))
            {
                if (LoadFile(App.CmdLineFile, false))
                {
                    (int position, _, _) = StringUtils.GetPositionAdaptive(text.Text, App.CmdLineLineIndex ?? 0, App.CmdLineCharIndex ?? 0);
                    SetTextSelection(position, 0);
                }
                else
                {
                    PrepareNew(false);
                }
            }
            else
            {
                PrepareNew(false);
            }

            Interactor.Activate = Interactor_ActivateReceived;
            Interactor.ActivateSetCaret = Interactor_ActivateSetCaretReceived;

            // Start loading external editors (don't hold UI anymore)
            externalEditorsLoadingCts = new CancellationTokenSource();
            Task.Run(() => ExternalEditorsLoadingProc(externalEditorsLoadingCts.Token));
        }

        #region Cross-Window communication

        public (int Start, int Length) GetTextSelection()
        {
            return (text.SelectionStart, text.SelectionLength);
        }

        public void ActivateAndSetTextSelection(int selectionStart, int selectionLength)
        {
            Activate();
            text.Focus();

            SetTextSelection(selectionStart, selectionLength);
        }

        public event EventHandler SelectionChanged;

        public string GetText()
        {
            return text.Text;
        }

        public void SetText(string value)
        {
            text.Text = value;
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

        private void Interactor_ActivateSetCaretReceived(int? lineIndex, int? charIndex)
        {
            Invoke(() =>
            {
                if (WindowState == FormWindowState.Minimized)
                    WindowState = prevWindowState;

                if (lineIndex.HasValue || charIndex.HasValue)
                {
                    (int position, _, _) = StringUtils.GetPositionAdaptive(text.Text, lineIndex ?? 0, charIndex ?? 0);
                    ActivateAndSetTextSelection(position, 0);
                }
                else
                {
                    Activate();
                }
            });
        }

        private void CurrentDocumentWatcher_FileModified(object sender, EventArgs e)
        {
            Invoke(() =>
            {
                // Check once we are inside the Invoke that the message is still relevant
                if (sender == currentDocumentWatcher)
                {
                    isExternallyModified = true;
                    UpdateStatusBar();

                    if (!isNew && !hasUnsavedChanges && App.Settings.AutoReload)
                        ReloadButtonClicked();
                }
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
                externalEditorsLoadingCts.Cancel();
                currentDocumentWatcher?.Dispose();

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

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            // Clipboard content might have changed, update for "Paste" availability:
            UpdateMenu();
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

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                toolStripButtonReload.IsPressed = true;
            }
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                toolStripButtonReload.IsPressed = false;
                ReloadButtonClicked();
            }
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
            UpdateMenu();
            UpdateStatusBar();
            SelectionChanged?.Invoke(this, e);
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

        private void openInExternalEditorMenuItem_Click(object sender, EventArgs e)
        {
            if ((!isNew)
                && (sender is ToolStripItem menuItem)
                && (menuItem.Tag is IExternalEditor externalEditor))
            {
                if (HandleUnsavedChanges())
                {
                    (int lineIndex, int charIndex) = StringUtils.GetLineAndCol(text.Text, text.SelectionStart);
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = externalEditor.PathToExe,
                            Arguments = externalEditor.CreateCommandLineArgs(currentDocumentFullPath, lineIndex, charIndex)
                        });
                    }
                    catch (Exception ex)
                    {
                        App.ShowError(ex);
                    }
                }
            }
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
                SetTextSelection(newCursorPosition, 0);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (text.SelectionLength > 0)
            {
                Clipboard.SetText(text.Text.Substring(text.SelectionStart, text.SelectionLength));
                UpdateMenu();
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
                SetTextSelection(newCursorPosition, 0);
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

        private void goToLineMenuItem_Click(object sender, EventArgs e)
        {
            (int currentLine, _) = StringUtils.GetLineAndCol(text.Text, text.SelectionStart);
            int linesCount = StringUtils.GetLinesCount(text.Text);
            int? targetLine = GoToLineDialog.ShowDialog(this, currentLine, linesCount);
            if (targetLine.HasValue)
            {
                (int targetPosition, _, _) = StringUtils.GetPositionAdaptive(text.Text, targetLine.Value, 0);
                SetTextSelection(targetPosition, 0);
            }
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
                        // Controversial scenario:
                        // - loaded UTF-8 with preamble
                        // - reinterpreted as ANSI
                        // - while it's in ANSI form - deleted preamble
                        // - reinterpreted back as UTF-8
                        // Now should save with preamble or without it? - With, file initially contained it.
                        // - again reinterpred as ANSI
                        // Show preamble because we are going to save the file with it
                        // or don't show because it was manually erased?

                        byte[] bytes = currentDocumentBytes
                            ?? currentEncoding.StringToFileBytes(text.Text,
                                fileContainsPreamble && (currentEncoding == initialEncoding));

                        enableTextChangeHandler = false;
                        text.Text = encodingVm.FileBytesToString(bytes);
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
                if (App.SaveSettings(SettingsFlags.General | SettingsFlags.Font))
                    StatusBarShowSecondOrderSuccessMessage("Settings Saved");
                else
                    StatusBarShowSecondOrderErrorMessage("Error when saving settings. Settings not saved.");

                ApplySettings();
            }
        }

        #endregion

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutWindow dlg = new AboutWindow();
            dlg.ShowDialog(this);
        }

        private void toolStripButtonReload_MouseUp(object sender, MouseEventArgs e)
        {
            ReloadButtonClicked();
        }

        private void ReloadButtonClicked()
        {
            if (!isNew && isExternallyModified)
            {
                if (hasUnsavedChanges)
                {
                    if (!App.WarningQuestion("Your changes will be lost." + Environment.NewLine + "Continue?"))
                        return;
                }

                // Keep cursor at the same line/pos,
                // however if there is a selection - it would be misleading to keep selection at place
                // while its contents might have changed, so we reset the selection in order for user to know.
                (int selStartLine, int selStartChar) = StringUtils.GetLineAndCol(text.Text, text.SelectionStart);

                ReloadFile();

                (int newSelStartPos, _, _) = StringUtils.GetPositionAdaptive(text.Text, selStartLine, selStartChar);
                SetTextSelection(newSelStartPos, 0);
            }
        }

        private void blinkingTimer_Tick(object sender, EventArgs e)
        {
            if (isExternallyModified)
            {
                isLabelExternallyModifiedLit = !isLabelExternallyModifiedLit;
                UpdateLabelExternallyModifiedColor();
            }
        }

        #endregion

        #region New, Load, Save

        private void PrepareNew(bool isCurrentDocumentStateValid)
        {
            if (isCurrentDocumentStateValid)
                CloseCurrentDocument();

            text.Text = string.Empty;

            currentDocumentFileName = "new.txt";
            currentDocumentFullPath = string.IsNullOrEmpty(lastPathToFolder)
                ? Path.Combine(Environment.CurrentDirectory, currentDocumentFileName)
                : Path.Combine(lastPathToFolder, currentDocumentFileName);
            isNew = true;
            hasUnsavedChanges = false;
            isExternallyModified = false;
            currentDocumentBytes = null;
            UpdateTitle();
            UpdateMenu();

            initialEncoding = null;
            currentEncoding = EncodingManager.DefaultEncoding;
            fileContainsPreamble = false; // no file
            UpdateEncodingMenuCheckboxes();
            UpdateStatusBar();

            Interactor.UpdateCurrentDocumentFullPath(currentDocumentFullPath);
        }

        private bool LoadFile(string fileName, bool isCurrentDocumentStateValid)
        {
            try
            {
                string fullPath = Path.GetFullPath(fileName);
                byte[] allBytes = File.ReadAllBytes(fullPath);

                if (isCurrentDocumentStateValid)
                    CloseCurrentDocument();

                currentDocumentFullPath = fullPath;
                currentDocumentFileName = Path.GetFileName(fullPath);
                initialEncoding = EncodingManager.DetectEncoding(allBytes);
                currentEncoding = initialEncoding;
                fileContainsPreamble = currentEncoding.StartsWithPreamble(allBytes);
                UpdateEncodingMenuCheckboxes();

                text.Text = currentEncoding.FileBytesToString(allBytes);

                currentDocumentBytes = allBytes; // after text change
                hasUnsavedChanges = false; // after text change
                isNew = false;
                isExternallyModified = false;
                ResetSelection();
                UpdateTitle();
                UpdateMenu();
                UpdateStatusBar();

                Interactor.UpdateCurrentDocumentFullPath(currentDocumentFullPath);
                currentDocumentWatcher = new FileWatcher(currentDocumentFullPath);
                currentDocumentWatcher.FileModified += CurrentDocumentWatcher_FileModified;

                return true;
            }
            catch (Exception ex)
            {
                App.ShowError(ex);
                return false;
            }
        }

        private bool ReloadFile()
        {
            if (isNew)
                throw new InvalidOperationException();

            try
            {
                byte[] allBytes = File.ReadAllBytes(currentDocumentFullPath);

                initialEncoding = EncodingManager.DetectEncoding(allBytes);
                currentEncoding = initialEncoding;
                fileContainsPreamble = currentEncoding.StartsWithPreamble(allBytes);
                UpdateEncodingMenuCheckboxes();

                text.Text = currentEncoding.FileBytesToString(allBytes);

                currentDocumentBytes = allBytes; // after text change
                hasUnsavedChanges = false; // after text change
                isNew = false;
                isExternallyModified = false;
                UpdateTitle();
                UpdateMenu();
                UpdateStatusBar();

                StatusBarShowSecondOrderSuccessMessage("Reloaded");
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

                if (!isNew && string.Equals(currentDocumentFullPath, destPath, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Save without closing current document
                    return ExecuteSave();
                }
                else
                {
                    (bool proceed, byte[] encodedBytes) = EncodeForSave(text.Text, false);
                    if (!proceed)
                        return null;

                    bool saveResult = UnsafeSave(destPath, encodedBytes, false);
                    if (saveResult)
                    {
                        CloseCurrentDocument();

                        currentDocumentFullPath = destPath;
                        currentDocumentFileName = Path.GetFileName(destPath);
                        isNew = false;
                        hasUnsavedChanges = false;
                        isExternallyModified = false;
                        currentDocumentBytes = encodedBytes;
                        initialEncoding = currentEncoding;
                        fileContainsPreamble = currentEncoding.StartsWithPreamble(encodedBytes);
                        UpdateTitle();
                        UpdateMenu();
                        UpdateStatusBar();

                        Interactor.UpdateCurrentDocumentFullPath(currentDocumentFullPath);
                        currentDocumentWatcher = new FileWatcher(currentDocumentFullPath);
                        currentDocumentWatcher.FileModified += CurrentDocumentWatcher_FileModified;

                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
            if (isNew)
                throw new InvalidOperationException();

            (bool proceed, byte[] encodedBytes) = EncodeForSave(text.Text,
                // We save UTF-8 by default without preamble, but if it was already there - we preserve it
                fileContainsPreamble && (currentEncoding == initialEncoding));
            if (!proceed)
                return null;

            bool saveResult = UnsafeSave(currentDocumentFullPath, encodedBytes, true);
            if (saveResult)
            {
                isNew = false;
                hasUnsavedChanges = false;
                isExternallyModified = false;
                currentDocumentBytes = encodedBytes;
                initialEncoding = currentEncoding;
                fileContainsPreamble = currentEncoding.StartsWithPreamble(encodedBytes);
            }

            UpdateTitle();
            UpdateMenu();
            UpdateStatusBar();
            return saveResult;
        }

        private (bool proceed, byte[] encodedBytes) EncodeForSave(string allText, bool preferPreamble)
        {
            byte[] result = currentEncoding.StringToFileBytes(allText, preferPreamble);
            if (!currentEncoding.IsLossless)
            {
                string decoded = currentEncoding.FileBytesToString(result);
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

        private bool UnsafeSave(string destPath, byte[] bytes, bool suspendDocumentWatcher)
        {
            try
            {
                if (suspendDocumentWatcher)
                {
                    currentDocumentWatcher.SaveWrapper(() =>
                    {
                        SaveProc(destPath, bytes);
                    });
                }
                else
                {
                    SaveProc(destPath, bytes);
                }

                StatusBarShowSuccessMessage("SAVED");
                return true;
            }
            catch (Exception ex)
            {
                App.ShowError(ex);
                return false;
            }

            static void SaveProc(string destPath, byte[] bytes)
            {
                using (FileStream fs = new FileStream(destPath, FileMode.Create, FileAccess.Write, FileShare.None, App.BUFFERSIZE))
                {
                    fs.Write(bytes);
                }
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

        private void CloseCurrentDocument()
        {
            if (!isNew)
            {
                currentDocumentWatcher.Dispose(); // should be not null when not isNew, null reference crash here would reveal inconsistencies in code
                currentDocumentWatcher = null;

                RememberWindowPosition();
                App.SaveSettings(SettingsFlags.FileWindowPosition, currentDocumentFullPath);
            }
        }

        #endregion

        private void ExternalEditorsLoadingProc(CancellationToken ct)
        {
            try
            {
                App.ExternalEditors.Load(editor => Invoke(() =>
                {
                    // Add menu item which will call this external editor
                    ToolStripMenuItem menuItem = new($"Edit in {editor.DisplayName}",
                        editor.Icon, openInExternalEditorMenuItem_Click);
                    menuItem.Tag = editor;

                    int separatorIndex = fileToolStripMenuItem.DropDownItems.IndexOf(externalEditorsSeparator);
                    fileToolStripMenuItem.DropDownItems.Insert(separatorIndex, menuItem);

                    if (!externalEditorsSeparator.Visible)
                        externalEditorsSeparator.Visible = true;

                    UpdateMenu();
                }), externalEditorsLoadingCts.Token);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                    BeginInvoke(() => App.ShowError(ex));
            }
        }

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

        private void SetTextSelection(int selectionStart, int selectionLength)
        {
            bool changed = (selectionStart != text.SelectionStart) || (selectionLength != text.SelectionLength);

            text.SelectionStart = selectionStart;
            text.SelectionLength = selectionLength;
            text.ScrollToCaret();

            UpdateMenu();
            UpdateStatusBar();

            if (changed)
                SelectionChanged?.Invoke(this, EventArgs.Empty);
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

        private void UpdateMenu()
        {
            foreach (ToolStripItem fileItem in fileToolStripMenuItem.DropDownItems)
            {
                if (fileItem.Tag is IExternalEditor)
                {
                    fileItem.Enabled = !isNew;
                }
            }

            cutToolStripMenuItem.Enabled = text.SelectionLength > 0;
            cutContextMenuItem.Enabled = cutToolStripMenuItem.Enabled;
            copyToolStripMenuItem.Enabled = text.SelectionLength > 0;
            copyContextMenuItem.Enabled = copyToolStripMenuItem.Enabled;
            pasteToolStripMenuItem.Enabled = Clipboard.ContainsText();
            pasteContextMenuItem.Enabled = pasteToolStripMenuItem.Enabled;
        }

        private void UpdateStatusBar()
        {
            encodingLabel.Text = currentEncoding?.DisplayName ?? string.Empty;
            wrapLabel.Visible = App.Settings.Wrap;
            modifiedLabel.Visible = hasUnsavedChanges;

            bool wasLabelExternallyModifiedVisible = labelExternallyModified.Visible;
            labelExternallyModified.Visible = isExternallyModified;
            toolStripButtonReload.Visible = isExternallyModified;
            if (isExternallyModified != wasLabelExternallyModifiedVisible)
            {
                if (isExternallyModified)
                {
                    isLabelExternallyModifiedLit = true;
                    UpdateLabelExternallyModifiedColor();
                    blinkingTimer.Start();
                }
                else
                {
                    blinkingTimer.Stop();
                }
            }

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
        bool isLabelExternallyModifiedLit = false;
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

        private void UpdateLabelExternallyModifiedColor()
        {
            if (isLabelExternallyModifiedLit)
            {
                labelExternallyModified.ForeColor = Color.White;
                labelExternallyModified.BackColor = Color.Red;
                labelExternallyModified.BorderSides = ToolStripStatusLabelBorderSides.None;
                labelExternallyModified.Margin = statusLabelMarginBorderless;
            }
            else
            {
                labelExternallyModified.ForeColor = SystemColors.ControlText;
                labelExternallyModified.BackColor = SystemColors.Control;
                labelExternallyModified.BorderSides = ToolStripStatusLabelBorderSides.All;
                labelExternallyModified.Margin = statusLabelMarginForBorders;
            }
        }

        #endregion

        private Point GetTopRightForFindReplace()
        {
            return text.PointToScreen(new Point(text.Width - 10, 0));
        }
    }
}
