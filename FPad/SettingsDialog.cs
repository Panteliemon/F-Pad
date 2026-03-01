using FPad.Controls;
using FPad.Encodings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad;

public partial class SettingsDialog : Form
{
    private bool enableHandlers = false;

    private FontFamily[] fontFamilies;
    private FontFamily selectedFontFamily;
    private int selectedFontSize;
    private EncodingVm[] encodings;

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

        encodings = EncodingManager.Encodings.ToArray();
        EncodingVm currentDefaultEncoding = EncodingManager.GetDefaultEncoding();
        cbEncodings.Items.AddRange(encodings);
        cbEncodings.DisplayMember = nameof(EncodingVm.DisplayName);
        cbEncodings.SelectedIndex = Array.FindIndex(encodings, x => x == currentDefaultEncoding);

        chBold.Checked = App.Settings.IsBold;
        chItalic.Checked = App.Settings.IsItalic;
        chAutoReload.Checked = App.Settings.AutoReload;

        chWrap.Checked = App.Settings.Wrap;
        exampleText.WordWrap = App.Settings.Wrap;

        Text = "Settings – " + App.TITLE; // Em Dash
        exampleText.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent sit amet libero aliquet, vestibulum massa dignissim, facilisis lacus. Phasellus ligula ex, sodales eu suscipit non, dapibus sit amet ex."
            + Environment.NewLine + "Maecenas in rutrum massa, a dictum leo."
            + Environment.NewLine + "Sed pellentesque, massa tincidunt pulvinar vulputate, nibh dignissim nisi, ac rhoncus urna neque eget arcu.";

        label5.Text = $"Associate txt files with {App.TITLE} (takes effect immediately, Cancel button doesn't roll back):";
        UacIconBehavior _ = new(bAssociateAllUsers);

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
        BeginInvoke(() => tabControl1.Focus());        
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
        App.Settings.DefaultEncodingWebName = (cbEncodings.SelectedIndex >= 0)
            ? encodings[cbEncodings.SelectedIndex].Encoding.WebName : null;
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

    private void bAssociateAllUsers_Click(object sender, EventArgs e)
    {
        if (IsAdmin())
        {
            ExecuteAssociate(true);
        }
        else
        {
            try
            {
                string exePath = Process.GetCurrentProcess().MainModule.FileName;
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = "-associate_txt",
                    Verb = "runas",
                    UseShellExecute = true
                };

                bool success = false;
                using (Process process = Process.Start(psi))
                {
                    if (process != null)
                    {
                        process.WaitForExit();
                        success = process.ExitCode == 0;
                    }
                }

                if (success)
                {
                    EmbarrassmentWindow.ShowDialog1(this);
                }
                else
                {
                    MessageBox.Show($"An error occured. Try to re-run {App.TITLE} as administrator and repeat this action manually to see more details on the error.",
                        App.TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Win32Exception ex) when (ex.NativeErrorCode == 1223) // ERROR_CANCELLED
            {
                // User cancelled the UAC prompt, do nothing
            }
            catch (Exception ex)
            {
                App.ShowError(ex);
            }
        }
    }

    private void bAssociateCurrentUser_Click(object sender, EventArgs e)
    {
        ExecuteAssociate(false);
    }

    #endregion

    private void ExecuteAssociate(bool forAllUsers)
    {
        try
        {
            App.AssociateTxt(forAllUsers);
            EmbarrassmentWindow.ShowDialog1(this);
        }
        catch (Exception ex)
        {
            App.ShowError(ex);
        }
    }

    private static bool IsAdmin()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}
