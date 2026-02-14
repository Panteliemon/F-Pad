using FPad.Controls;
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
public partial class GoToLineDialog : Form
{
    private int linesCount;
    private bool wasActivated;
    private bool isShowingError;

    public int? Result { get; private set; }

    private GoToLineDialog(int currentLineIndex, int linesCount)
    {
        InitializeComponent();
        this.linesCount = linesCount;

        label1.Text = $"Line Number: (1 to {linesCount})";

        _ = new DigitOnlyBehavior(tbValue);
        tbValue.Text = (currentLineIndex + 1).ToString();
        tbValue.SelectAll();
    }

    /// <summary>
    /// Returns line index ([user input] - 1) if confirmed, null if canceled.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="currentLineIndex">Line index where the user currently is (for convenience)</param>
    /// <param name="linesCount"></param>
    /// <returns></returns>
    public static int? ShowDialog(Form owner, int currentLineIndex, int linesCount)
    {
        GoToLineDialog dlg = new GoToLineDialog(currentLineIndex, linesCount);
        dlg.ShowDialog(owner);

        return dlg.Result;
    }

    private void GoToLineDialog_Activated(object sender, EventArgs e)
    {
        if (!wasActivated)
        {
            tbValue.Focus();
            wasActivated = true;
        }
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void bOk_Click(object sender, EventArgs e)
    {
        if (int.TryParse(tbValue.Text, out int parsed))
        {
            if ((parsed >= 1) && (parsed <= linesCount))
            {
                Result = parsed - 1;
                Close();
                return;
            }
        }

        SystemSounds.Beep.Play();
        ShowError("Incorrect value");

        tbValue.SelectAll();
        tbValue.Focus();
    }

    private void ShowError(string msg)
    {
        errorLabel.Text = msg;
        if (!isShowingError)
        {
            isShowingError = true;
            Task.Run(async () =>
            {
                BeginInvoke(() => errorLabel.Visible = true);
                await Task.Delay(100);
                BeginInvoke(() => errorLabel.Visible = false);
                await Task.Delay(100);
                BeginInvoke(() => errorLabel.Visible = true);
                await Task.Delay(500);
                BeginInvoke(() =>
                {
                    errorLabel.Visible = false;
                    isShowingError = false;
                });
            });
        }
    }
}
