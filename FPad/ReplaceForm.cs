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

        // TODO set default text and make sure handler is called even if the text is empty

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

    private void tbReplaceWith_TextChanged(object sender, EventArgs e)
    {

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
            List<int> matches = FindAllMatches();
            if (matches.Count > 0)
            {
                owner.SetTextSelection(matches[0], tbFind.Text.Length);
            }
            // Let know if cannot find due to everything being already replaced
            else if (isDisplayingFindResult && wasSomethingFound && (matches.Count == 0))
            {
                ShowReachedEnd("Nothing left");
                SystemSounds.Beep.Play();
            }

            DisplayFindResult(matches.Count, 0);
        }
    }

    private void bNext_Click(object sender, EventArgs e)
    {
        if (isSearchAllowed)
        {
            List<int> matches = FindAllMatches();
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
                        owner.SetTextSelection(matches[matchIndex], tbFind.Text.Length);
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
                    owner.SetTextSelection(matches[matchIndex], tbFind.Text.Length);
                }
            }
            // Let know if cannot find due to everything being already replaced
            else if (isDisplayingFindResult && wasSomethingFound && (matches.Count == 0))
            {
                ShowReachedEnd("Nothing left");
                SystemSounds.Beep.Play();
            }

            DisplayFindResult(matches.Count, matchIndex);
        }
    }

    private void bPrev_Click(object sender, EventArgs e)
    {
        if (isSearchAllowed)
        {
            List<int> matches = FindAllMatches();
            (int selStart, int selLength) = owner.GetTextSelection();
            int matchIndex = matches.FindLastIndex(x => x < selStart);
            if (matchIndex >= 0)
            {
                owner.SetTextSelection(matches[matchIndex], tbFind.Text.Length);
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
                owner.SetTextSelection(selStart, tbReplaceWith.Text.Length);
            }
        }
    }

    private void bReplaceAll_Click(object sender, EventArgs e)
    {
        if (bReplaceAll.Enabled)
        {
            List<int> matches = FindAllMatches();
            if (matches.Count > 0)
            {
                ReadOnlySpan<char> text = owner.GetText();
                (int selStart, int selLength) = owner.GetTextSelection();
                int selEnd = selStart + selLength;

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
                owner.SetTextSelection(selStart, selEnd - selStart);

                labelResult.Text = $"{matches.Count} occurences replaced within document";
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
        }
    }

    private void bReplaceAllInSelection_Click(object sender, EventArgs e)
    {
        if (bReplaceAllInSelection.Enabled)
        {

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

    private List<int> FindAllMatches()
    {
        List<int> result = new();
        StringSearch engine = new(tbFind.Text, chMatchCase.Checked);
        string text = owner.GetText();
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
