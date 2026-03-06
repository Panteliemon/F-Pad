using FPad.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad.Controls;

public partial class FontPicker : UserControl
{
    private FontFamily[] families;
    private bool enableHandlers;

    /// <summary>
    /// Invoked when the user changes something on UI.
    /// </summary>
    public event EventHandler Changed;

    public FontPicker()
    {
        InitializeComponent();

        tbSize.Minimum = FontSettings.MIN_SIZE;
        tbSize.Maximum = FontSettings.MAX_SIZE;
        slSize.Minimum = FontSettings.MIN_SIZE;
        slSize.Maximum = 36;

        families = FontFamily.Families.OrderBy(x => x.Name).ToArray();
        cbFontName.Items.AddRange(families);
        cbFontName.DisplayMember = nameof(FontFamily.Name);

        enableHandlers = true;
    }

    public void DisplaySettings(FontSettings fontSettings)
    {
        enableHandlers = false;

        int index = Array.FindIndex(families, x => x.Name == fontSettings.Family);
        cbFontName.SelectedIndex = index;
        tbSize.Value = fontSettings.Size;
        SetSliderByNumeric();
        chBold.Checked = fontSettings.IsBold;
        chItalic.Checked = fontSettings.IsItalic;

        enableHandlers = true;
    }

    public void SaveSettings(FontSettings dest)
    {
        if (cbFontName.SelectedIndex >= 0)
            dest.Family = families[cbFontName.SelectedIndex].Name;
        else
            dest.Family = string.Empty;
        dest.Size = (int)Math.Round(tbSize.Value);
        dest.IsBold = chBold.Checked;
        dest.IsItalic = chItalic.Checked;
    }

    #region Event Handlers

    private void cbFontName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (enableHandlers)
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void tbSize_ValueChanged(object sender, EventArgs e)
    {
        if (enableHandlers)
        {
            enableHandlers = false;
            SetSliderByNumeric();
            enableHandlers = true;

            Changed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void chBold_CheckedChanged(object sender, EventArgs e)
    {
        if (enableHandlers)
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void chItalic_CheckedChanged(object sender, EventArgs e)
    {
        if (enableHandlers)
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void slFontSize_Scroll(object sender, EventArgs e)
    {
        if (enableHandlers)
        {
            enableHandlers = false;
            SetNumericBySlider();
            enableHandlers = true;

            Changed?.Invoke(this, EventArgs.Empty);
        }
    }

    #endregion

    private void SetSliderByNumeric()
    {
        int value = (int)Math.Round(tbSize.Value);
        if (value < slSize.Minimum)
            slSize.Value = slSize.Minimum;
        else if (value > slSize.Maximum)
            slSize.Value = slSize.Maximum;
        else
            slSize.Value = value;
    }

    private void SetNumericBySlider()
    {
        tbSize.Value = slSize.Value;
    }
}
