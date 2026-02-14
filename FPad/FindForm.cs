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

public partial class FindForm : Form
{
    private const int WM_EXITSIZEMOVE = 0x0232;
    private const int REASONABLE_SELECTION_LENGTH = 100;
    private static FindForm instance;

    private Point topRight;
    private MainWindow owner;

    private bool areButtonsAvailable;
    private bool areCheckboxHandlersAvailable;
    private bool isShowingResults;
    private bool isShowingReachedEnd;

    private FindForm(MainWindow owner, Point topRight)
    {
        InitializeComponent();
        this.topRight = topRight;

        (int selStart, int selLength) = owner.GetTextSelection();
        if ((selLength == 0) && !string.IsNullOrEmpty(App.LastSearchStr))
        {
            tbFind.Text = App.LastSearchStr;
            tbFind.SelectAll();
        }
        else
        {
            tbFind.Text = string.Empty;
            tbFind_TextChanged(this, EventArgs.Empty);
        }

        this.owner = owner;
        this.owner.KeyDown += FindForm_KeyDown;

        chMatchCase.Checked = App.Settings.FindMatchCase;
        chWholeWords.Checked = App.Settings.FindWholeWords;
        areCheckboxHandlersAvailable = true;
    }

    public static void Show(MainWindow owner, Point topRight)
    {
        if (instance != null)
        {
            instance.BringToFront();
        }
        else
        {
            instance = new FindForm(owner, topRight);
            instance.Show(owner);
        }

        // Auto-fill from current selection (on each Ctrl+F):
        (int selStart, int selLength) = owner.GetTextSelection();
        if ((selLength > 0) && (selLength <= REASONABLE_SELECTION_LENGTH))
        {
            string selectedText = owner.GetText().Substring(selStart, selLength);
            if (!string.Equals(selectedText, instance.tbFind.Text,
                instance.chMatchCase.Checked ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase))
            {
                instance.tbFind.Text = owner.GetText().Substring(selStart, selLength).Trim();
                instance.tbFind.SelectAll();
            }
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

    private void FindForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        instance = null;
        owner.KeyDown -= FindForm_KeyDown;
    }

    private void FindForm_Load(object sender, EventArgs e)
    {
        Top = topRight.Y;
        Left = topRight.X - Width;
    }

    private void FindForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            Close();
        }
        else if (e.KeyCode == Keys.F3)
        {
            if (e.Shift)
                bFindPrev_Click(sender, e);
            else
                bFindNext_Click(sender, e);
        }
    }

    private void tbFind_TextChanged(object sender, EventArgs e)
    {
        UpdateButtonsAvailable();
        isShowingResults = false;
        labelResults.Text = "Search not started yet";
    }

    private void chMatchCase_CheckedChanged(object sender, EventArgs e)
    {
        if (areCheckboxHandlersAvailable)
        {
            DelayedUnfocus();
            owner.ChangeSearchSettings(chMatchCase.Checked, chWholeWords.Checked);
        }
    }

    private void chWholeWords_CheckedChanged(object sender, EventArgs e)
    {
        if (areCheckboxHandlersAvailable)
        {
            DelayedUnfocus();
            owner.ChangeSearchSettings(chMatchCase.Checked, chWholeWords.Checked);
        }
    }

    private void bFindFirst_Click(object sender, EventArgs e)
    {
        if (areButtonsAvailable)
        {
            List<int> allMatches = FindAllMatches();
            if (allMatches.Count > 0)
            {
                owner.ActivateAndSetTextSelection(allMatches[0], tbFind.Text.Length);
            }

            DisplayResults(allMatches, 0);
            App.LastSearchStr = tbFind.Text;
        }
    }

    private void bFindNext_Click(object sender, EventArgs e)
    {
        if (areButtonsAvailable)
        {
            List<int> allMatches = FindAllMatches();
            (int selStart, int selLength) = owner.GetTextSelection();
            int matchIndex = allMatches.FindIndex(x => x >= selStart);
            if (matchIndex >= 0)
            {
                // If some match was exactly selected already - go to the next one.
                if ((selStart == allMatches[matchIndex]) && (selLength == tbFind.Text.Length))
                {
                    if (matchIndex + 1 < allMatches.Count)
                    {
                        matchIndex++;
                        owner.ActivateAndSetTextSelection(allMatches[matchIndex], tbFind.Text.Length);
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
                    owner.ActivateAndSetTextSelection(allMatches[matchIndex], tbFind.Text.Length);
                }
            }

            DisplayResults(allMatches, matchIndex);
            App.LastSearchStr = tbFind.Text;
        }
    }

    private void bFindPrev_Click(object sender, EventArgs e)
    {
        if (areButtonsAvailable)
        {
            List<int> allMatches = FindAllMatches();
            (int selStart, int selLength) = owner.GetTextSelection();
            int matchIndex = allMatches.FindLastIndex(x => x < selStart);
            if (matchIndex >= 0)
            {
                owner.ActivateAndSetTextSelection(allMatches[matchIndex], tbFind.Text.Length);
            }
            // If standing on the first match - beep
            else if ((allMatches.Count > 0) && (selStart == allMatches[0]) && (selLength == tbFind.Text.Length))
            {
                matchIndex = 0;
                SystemSounds.Beep.Play();
                ShowReachedEnd("Reached First");
            }

            DisplayResults(allMatches, matchIndex);
            App.LastSearchStr = tbFind.Text;
        }
    }

    #endregion

    private void UpdateButtonsAvailable()
    {
        areButtonsAvailable = !string.IsNullOrEmpty(tbFind.Text);
        bFindFirst.Enabled = areButtonsAvailable;
        bFindNext.Enabled = areButtonsAvailable;
        bFindPrev.Enabled = areButtonsAvailable;
    }

    private void DisplayResults(List<int> matches, int highlightedIndex)
    {
        if (matches.Count == 0)
        {
            labelResults.Text = "Nothing found";
        }
        else
        {
            if (highlightedIndex >= 0)
                labelResults.Text = $"Found {highlightedIndex + 1}/{matches.Count}";
            else
                labelResults.Text = $"Found {matches.Count}";
        }

        isShowingResults = true;
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

    private void DelayedUnfocus()
    {
        Task.Run(() =>
        {
            Task.Delay(100).Wait();
            if (!IsDisposed)
            {
                BeginInvoke(() =>
                {
                    if (isShowingResults && !owner.Focused)
                    {
                        owner.Activate();
                    }
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
