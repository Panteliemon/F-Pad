using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad;

public partial class ReplaceForm : Form
{
    private static ReplaceForm instance;

    private Point topRight;
    private MainWindow owner;

    private bool isSearchAllowed;
    private bool areCheckboxHandlersEnabled;

    private ReplaceForm(MainWindow owner, Point topRight)
    {
        InitializeComponent();
        this.topRight = topRight;
        this.owner = owner;

        owner.KeyDown += ReplaceForm_KeyDown;
        owner.SelectionChanged += Owner_SelectionChanged;

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
        RefreshButtons();
    }

    private void tbReplaceWith_TextChanged(object sender, EventArgs e)
    {

    }

    private void chMatchCase_CheckedChanged(object sender, EventArgs e)
    {
        if (areCheckboxHandlersEnabled)
        {
            owner.ChangeSearchSettings(chMatchCase.Checked, chWholeWords.Checked);
        }
    }

    private void chWholeWords_CheckedChanged(object sender, EventArgs e)
    {
        if (areCheckboxHandlersEnabled)
        {
            owner.ChangeSearchSettings(chMatchCase.Checked, chWholeWords.Checked);
        }
    }

    private void bFindFirst_Click(object sender, EventArgs e)
    {
        if (isSearchAllowed)
        {

        }
    }

    private void bNext_Click(object sender, EventArgs e)
    {
        if (isSearchAllowed)
        {

        }
    }

    private void bPrev_Click(object sender, EventArgs e)
    {
        if (isSearchAllowed)
        {

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
}
