using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad;

public partial class ReplaceForm : Form
{
    private const int WM_EXITSIZEMOVE = 0x0232;
    private const int REASONABLE_SELECTION_LENGTH = 100;
    private static ReplaceForm instance;

    private Point topRight;
    private MainWindow owner;

    private bool isSearchAllowed;
    private bool areCheckboxHandlersEnabled;
    private bool isDisplayingFindResult;
    private bool wasSomethingFound;
    private bool isShowingReachedEnd;

    private ReplaceForm(MainWindow owner, Point topRight)
    {
        InitializeComponent();
        this.topRight = topRight;
        this.owner = owner;

        owner.KeyDown += ReplaceForm_KeyDown;
        owner.SelectionChanged += Owner_SelectionChanged;

        (int selStart, int selLength) = owner.GetTextSelection();
        // Unlike in Find, here auto-fill from selection happens only on first Ctrl+H
        if ((selLength > 0) && (selLength < REASONABLE_SELECTION_LENGTH))
        {
            tbFind.Text = owner.GetText().Substring(selStart, selLength);
            tbFind.SelectAll();
        }
        else if (!string.IsNullOrEmpty(App.LastSearchStr))
        {
            tbFind.Text = App.LastSearchStr;
            tbFind.SelectAll();
            if (!string.IsNullOrEmpty(App.LastReplaceToStr))
                tbReplaceWith.Text = App.LastReplaceToStr;
        }
        else
        {
            tbFind.Text = string.Empty;
            tbFind_TextChanged(this, EventArgs.Empty);
        }

        chMatchCase.Checked = App.Settings.FindMatchCase;
        chWholeWords.Checked = App.Settings.FindWholeWords;
        areCheckboxHandlersEnabled = true;

        RefreshButtons();
    }

    public static void Show(MainWindow owner, Point topRight)
    {
        if (instance != null)
        {
            instance.BringToFront();
        }
        else
        {
            instance = new ReplaceForm(owner, topRight);
            instance.Show(owner);
        }
    }

    public static void HideIfShown()
    {
        instance?.Close();
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        if (m.Msg == WM_EXITSIZEMOVE)
        {
            DelayedUnfocus();
        }
    }

    #region Event Handlers

    private void ReplaceForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        instance = null;
        owner.KeyDown -= ReplaceForm_KeyDown;
    }

    private void ReplaceForm_Load(object sender, EventArgs e)
    {
        Top = topRight.Y;
        Left = topRight.X - Width;
    }

    private void ReplaceForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            Close();
        }
        else if (e.KeyCode == Keys.F3)
        {
            if (e.Shift)
                bPrev_Click(sender, e);
            else
                bNext_Click(sender, e);
        }
        else if (e.KeyCode == Keys.F4)
        {
            bReplace_Click(sender, e);
        }
    }

    private void Owner_SelectionChanged(object sender, EventArgs e)
    {
        RefreshButtons();
    }

    private void tbFind_TextChanged(object sender, EventArgs e)
    {
        labelResult.Text = "Search not started yet";
        isDisplayingFindResult = false;
        wasSomethingFound = false;
        RefreshButtons();
    }

    private void chMatchCase_CheckedChanged(object sender, EventArgs e)
    {
        if (areCheckboxHandlersEnabled)
        {
            DelayedUnfocus();
            owner.ChangeSearchSettings(chMatchCase.Checked, chWholeWords.Checked);
        }
    }

    private void chWholeWords_CheckedChanged(object sender, EventArgs e)
    {
        if (areCheckboxHandlersEnabled)
        {
            DelayedUnfocus();
            owner.ChangeSearchSettings(chMatchCase.Checked, chWholeWords.Checked);
        }
    }

    private void bFindFirst_Click(object sender, EventArgs e)
    {
        if (isSearchAllowed)
        {
            List<int> matches = FindAllMatches(owner.GetText());
            if (matches.Count > 0)
            {
                owner.ActivateAndSetTextSelection(matches[0], tbFind.Text.Length);
            }
            // Let know if cannot find due to everything being already replaced
            else if (isDisplayingFindResult && wasSomethingFound && (matches.Count == 0))
            {
                ShowReachedEnd("Nothing left");
                SystemSounds.Beep.Play();
            }

            DisplayFindResult(matches.Count, 0);
            App.LastSearchStr = tbFind.Text;
        }
    }

    private void bNext_Click(object sender, EventArgs e)
    {
        if (isSearchAllowed)
        {
            List<int> matches = FindAllMatches(owner.GetText());
            (int selStart, int selLength) = owner.GetTextSelection();
            int matchIndex = matches.FindIndex(x => x >= selStart);
            if (matchIndex >= 0)
            {
                // If some match is already selected - move to the next one
                if ((selStart == matches[matchIndex])
                    && ((selLength == tbFind.Text.Length)
                        // If replace.StartsWith(find) and we have already replaced:
                        || owner.GetText().AsSpan()[selStart..(selStart+selLength)].Equals(tbReplaceWith.Text, StringComparison.CurrentCulture))
                   )
                {
                    if (matchIndex + 1 < matches.Count)
                    {
                        matchIndex++;
                        owner.ActivateAndSetTextSelection(matches[matchIndex], tbFind.Text.Length);
                    }
                    else
                    {
                        // Last match has been reached, stay there.
                        SystemSounds.Beep.Play();
                        ShowReachedEnd("Reached Last");
                    }
                }
                else
                {
                    owner.ActivateAndSetTextSelection(matches[matchIndex], tbFind.Text.Length);
                }
            }
            // Let know if cannot find due to everything being already replaced
            else if (isDisplayingFindResult && wasSomethingFound && (matches.Count == 0))
            {
                ShowReachedEnd("Nothing left");
                SystemSounds.Beep.Play();
            }

            DisplayFindResult(matches.Count, matchIndex);
            App.LastSearchStr = tbFind.Text;
        }
    }

    private void bPrev_Click(object sender, EventArgs e)
    {
        if (isSearchAllowed)
        {
            List<int> matches = FindAllMatches(owner.GetText());
            (int selStart, int selLength) = owner.GetTextSelection();
            int matchIndex = matches.FindLastIndex(x => x < selStart);
            if (matchIndex >= 0)
            {
                owner.ActivateAndSetTextSelection(matches[matchIndex], tbFind.Text.Length);
            }
            // If standing on the first match - beep
            else if ((matches.Count > 0) && (selStart == matches[0]) && (selLength == tbFind.Text.Length))
            {
                matchIndex = 0;
                SystemSounds.Beep.Play();
                ShowReachedEnd("Reached First");
            }
            // Let know if cannot find due to everything being already replaced
            else if (isDisplayingFindResult && wasSomethingFound && (matches.Count == 0))
            {
                ShowReachedEnd("Nothing left");
                SystemSounds.Beep.Play();
            }

            DisplayFindResult(matches.Count, matchIndex);
            App.LastSearchStr = tbFind.Text;
        }
    }

    private void bSwap_Click(object sender, EventArgs e)
    {
        string tmp = tbFind.Text;
        tbFind.Text = tbReplaceWith.Text;
        tbReplaceWith.Text = tmp;
    }

    private void bReplace_Click(object sender, EventArgs e)
    {
        if (bReplace.Enabled)
        {
            (int selStart, int selLength) = owner.GetTextSelection();
            if (selLength > 0)
            {
                ReadOnlySpan<char> textSpan = owner.GetText().AsSpan();

                StringBuilder sb = new();
                sb.Append(textSpan[..selStart]);
                sb.Append(tbReplaceWith.Text);
                sb.Append(textSpan[(selStart + selLength)..]);

                owner.SetText(sb.ToString());
                owner.ActivateAndSetTextSelection(selStart, tbReplaceWith.Text.Length);

                App.LastReplaceToStr = tbReplaceWith.Text;
            }
        }
    }

    private void bReplaceAll_Click(object sender, EventArgs e)
    {
        if (bReplaceAll.Enabled)
        {
            ExecuteReplaceAllMatches(false);
        }
    }

    private void bReplaceAllInSelection_Click(object sender, EventArgs e)
    {
        if (bReplaceAllInSelection.Enabled)
        {
            ExecuteReplaceAllMatches(true);
        }
    }

    #endregion

    private void RefreshButtons()
    {
        isSearchAllowed = !string.IsNullOrEmpty(tbFind.Text);
        bFindFirst.Enabled = isSearchAllowed;
        bNext.Enabled = isSearchAllowed;
        bPrev.Enabled = isSearchAllowed;

        bReplaceAll.Enabled = isSearchAllowed;

        (int selStart, int selLength) = owner.GetTextSelection();
        bReplace.Enabled = selLength > 0;

        bReplaceAllInSelection.Enabled = isSearchAllowed && (selLength >= tbFind.Text.Length);
    }

    private void DisplayFindResult(int matchesCount, int selectedIndex)
    {
        if (matchesCount == 0)
        {
            labelResult.Text = "Nothing found";
        }
        else if (selectedIndex >= 0)
        {
            labelResult.Text = $"Found {selectedIndex + 1}/{matchesCount}";
        }
        else
        {
            labelResult.Text = $"Found {matchesCount}";
        }

        isDisplayingFindResult = true;
        // wasSomethingFound can only go to false when isDisplayingFindResult goes to false
        wasSomethingFound = wasSomethingFound || (matchesCount > 0);
    }

    private List<int> FindAllMatches(ReadOnlySpan<char> text)
    {
        List<int> result = new();
        StringSearch engine = new(tbFind.Text, chMatchCase.Checked);
        int currentSearchStart = 0;
        while (true)
        {
            int currentMatch = engine.FindFirstMatch(text, currentSearchStart, chWholeWords.Checked);
            if (currentMatch >= 0)
            {
                result.Add(currentMatch);
                currentSearchStart = currentMatch + 1;
            }
            else
            {
                break;
            }
        }

        return result;
    }

    private void ExecuteReplaceAllMatches(bool withinSelection)
    {
        ReadOnlySpan<char> text = owner.GetText();
        (int selStart, int selLength) = owner.GetTextSelection();
        int selEnd = selStart + selLength;

        List<int> allMatches;
        if (withinSelection)
        {
            // To make "whole words" option work correctly on borders we will cover broader range
            int virtualSelStart = selStart > 0 ? selStart - 1 : selStart;
            int virtualSelEnd = selEnd < text.Length ? selEnd + 1 : selEnd;
            allMatches = FindAllMatches(text[virtualSelStart..virtualSelEnd])
                .Select(x => x + virtualSelStart) // convert to absolute offsets
                .Where(x => (x >= selStart) && (x + tbFind.Text.Length <= selEnd)) // filter because the search range was wider than the selection
                .ToList();
        }
        else
        {
            allMatches = FindAllMatches(text);
        }

        // For this function we must get rid of overlapping matches
        List<int> matches = new();
        int lastMatch = -1;
        for (int i = 0; i < allMatches.Count; i++)
        {
            int match = allMatches[i];
            if ((i == 0) || (match >= lastMatch + tbFind.Text.Length))
            {
                matches.Add(match);
                lastMatch = match;
            }
        }

        if (matches.Count > 0)
        {
            StringBuilder sb = new();
            int currentFragmentStart = 0;
            foreach (int match in matches)
            {
                sb.Append(text[currentFragmentStart..match]);
                sb.Append(tbReplaceWith.Text);
                currentFragmentStart = match + tbFind.Text.Length;
            }

            if (currentFragmentStart < text.Length)
                sb.Append(text[currentFragmentStart..]);

            selStart = GetPositionAfterReplace(selStart, matches, tbFind.Text.Length, tbReplaceWith.Text.Length);
            selEnd = GetPositionAfterReplace(selEnd, matches, tbFind.Text.Length, tbReplaceWith.Text.Length);

            owner.SetText(sb.ToString());
            owner.ActivateAndSetTextSelection(selStart, selEnd - selStart);

            labelResult.Text = withinSelection
                ? $"{matches.Count} occurences replaced within selection"
                : $"{matches.Count} occurences replaced within document";
            isDisplayingFindResult = false;
            wasSomethingFound = false;
        }
        else
        {
            labelResult.Text = "Nothing found";
            SystemSounds.Beep.Play();
            isDisplayingFindResult = false;
            wasSomethingFound = false;
        }

        App.LastSearchStr = tbFind.Text;
        App.LastReplaceToStr = tbReplaceWith.Text;
    }

    private static int GetPositionAfterReplace(int positionBeforeReplace, List<int> matches, int findLength, int replaceLength)
    {
        if (findLength == replaceLength)
            return positionBeforeReplace;

        int fullMatchesBeforePosition = 0;
        int correction = 0;
        for (int i = 0; i < matches.Count; i++)
        {
            int match = matches[i];
            if (match >= positionBeforeReplace) // matches are ordered, so no point of going past original position
                break;

            int relativePosition = positionBeforeReplace - match;
            if (relativePosition >= findLength)
            {
                fullMatchesBeforePosition++;
            }
            else
            {
                // Position is within match.
                if (relativePosition > replaceLength)
                {
                    // Set position to the end of replaced substring so it doesn't go out of range.
                    correction = replaceLength - relativePosition;
                }

                break;
            }
        }

        return positionBeforeReplace + fullMatchesBeforePosition * (replaceLength - findLength) + correction;
    }

    private void DelayedUnfocus()
    {
        Task.Run(() =>
        {
            Task.Delay(100).Wait();
            if (!IsDisposed)
            {
                BeginInvoke(() =>
                {
                    if (isDisplayingFindResult && !owner.Focused)
                        owner.Activate();
                });
            }
        });
    }

    private void ShowReachedEnd(string text)
    {
        labelReachedEnd.Text = text;
        if (!isShowingReachedEnd)
        {
            isShowingReachedEnd = true;
            Task.Run(async () =>
            {
                BeginInvoke(() => labelReachedEnd.Visible = true);
                await Task.Delay(100);
                BeginInvoke(() => labelReachedEnd.Visible = false);
                await Task.Delay(100);
                BeginInvoke(() => labelReachedEnd.Visible = true);
                await Task.Delay(500);
                BeginInvoke(() =>
                {
                    labelReachedEnd.Visible = false;
                    isShowingReachedEnd = false;
                });
            });
        }
    }
}
