using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FPad.Settings;

namespace FPad;

public partial class SettingsDialog : Form
{
    private bool enableHandlers = false;

    private FontFamily[] fontFamilies;
    private FontFamily selectedFontFamily;
    private int selectedFontSize;

    public bool Result { get; private set; }

    private SettingsDialog()
    {
        InitializeComponent();
        Icon = App.Icon;

        selectedFontSize = App.Settings.FontSize;
        SetSliderValue();
        tbFontSize.Value = selectedFontSize;

        fontFamilies = FontFamily.Families.OrderBy(x => x.Name).ToArray();
        selectedFontFamily = FontUtils.GetFontFamilyByString(App.Settings.FontFamily, fontFamilies);

        cbFonts.Items.AddRange(fontFamilies);
        cbFonts.DisplayMember = nameof(FontFamily.Name);
        cbFonts.SelectedIndex = Array.FindIndex(fontFamilies, x => x == selectedFontFamily);

        chBold.Checked = App.Settings.IsBold;
        chItalic.Checked = App.Settings.IsItalic;
        chAutoReload.Checked = App.Settings.AutoReload;

        chWrap.Checked = App.Settings.Wrap;
        exampleText.WordWrap = App.Settings.Wrap;

        Text = "Settings – " + App.TITLE; // Em Dash
        exampleText.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent sit amet libero aliquet, vestibulum massa dignissim, facilisis lacus. Phasellus ligula ex, sodales eu suscipit non, dapibus sit amet ex."
            + Environment.NewLine + "Maecenas in rutrum massa, a dictum leo."
            + Environment.NewLine + "Sed pellentesque, massa tincidunt pulvinar vulputate, nibh dignissim nisi, ac rhoncus urna neque eget arcu.";

        enableHandlers = true;
    }

    public static bool ShowADialog()
    {
        SettingsDialog instance = new();
        instance.ShowDialog();
        return instance.Result;
    }

    private void ApplyFont()
    {
        exampleText.Font = FontUtils.GetFontByParameters(selectedFontFamily, selectedFontSize, chBold.Checked, chItalic.Checked);
    }

    private void SetSliderValue()
    {
        slFontSize.Value = (selectedFontSize > slFontSize.Maximum)
            ? slFontSize.Maximum : selectedFontSize;
    }

    #region Event Handlers

    private void SettingsDialog_Load(object sender, EventArgs e)
    {
        ApplyFont();

        exampleText.Select(0, 0);
        BeginInvoke(() => cbFonts.Focus());
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
        Result = false;
        Close();
    }

    private void bSave_Click(object sender, EventArgs e)
    {
        Result = true;
        App.Settings.FontFamily = selectedFontFamily.Name;
        App.Settings.FontSize = selectedFontSize;
        App.Settings.IsBold = chBold.Checked;
        App.Settings.IsItalic = chItalic.Checked;
        App.Settings.AutoReload = chAutoReload.Checked;
        Close();
    }

    private void cbFonts_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!enableHandlers)
            return;

        selectedFontFamily = fontFamilies[cbFonts.SelectedIndex];
        ApplyFont();
    }

    private void tbFontSize_ValueChanged(object sender, EventArgs e)
    {
        if (enableHandlers)
        {
            selectedFontSize = (int)Math.Round(tbFontSize.Value);

            enableHandlers = false;
            SetSliderValue();
            enableHandlers = true;

            ApplyFont();
        }
    }

    private void slFontSize_Scroll(object sender, EventArgs e)
    {
        if (enableHandlers)
        {
            enableHandlers = false;
            tbFontSize.Value = slFontSize.Value;
            enableHandlers = true;

            selectedFontSize = slFontSize.Value;
            ApplyFont();
        }
    }

    private void chBold_CheckedChanged(object sender, EventArgs e)
    {
        if (enableHandlers)
        {
            ApplyFont();
        }
    }

    private void chItalic_CheckedChanged(object sender, EventArgs e)
    {
        if (enableHandlers)
        {
            ApplyFont();
        }
    }

    private void chWrap_CheckedChanged(object sender, EventArgs e)
    {
        exampleText.WordWrap = chWrap.Checked;
    }

    #endregion
}
