using FPad.Controls;
using FPad.Encodings;
using FPad.Settings;
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
    /// <summary>
    /// For display in this window only.
    /// </summary>
    private FontSettings innerFontSettings = new();
    private bool enableHandlers = false;

    private EncodingVm[] encodings;

    public bool Result { get; private set; }

    private SettingsDialog()
    {
        InitializeComponent();
        Icon = App.Icon;

        encodings = EncodingManager.Encodings.ToArray();
        EncodingVm currentDefaultEncoding = EncodingManager.GetDefaultEncoding();
        cbEncodings.Items.AddRange(encodings);
        cbEncodings.DisplayMember = nameof(EncodingVm.DisplayName);
        cbEncodings.SelectedIndex = Array.FindIndex(encodings, x => x == currentDefaultEncoding);

        chAutoReload.Checked = App.Settings.AutoReload;

        chWrap.Checked = App.Settings.Wrap;
        exampleText.WordWrap = App.Settings.Wrap;

        Text = "Settings – " + App.TITLE; // Em Dash
        exampleText.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent sit amet libero aliquet, vestibulum massa dignissim, facilisis lacus. Phasellus ligula ex, sodales eu suscipit non, dapibus sit amet ex."
            + Environment.NewLine + "Maecenas in rutrum massa, a dictum leo."
            + Environment.NewLine + "Sed pellentesque, massa tincidunt pulvinar vulputate, nibh dignissim nisi, ac rhoncus urna neque eget arcu.";

        label5.Text = $"Associate txt files with {App.TITLE} (takes effect immediately, Cancel button doesn't roll back):";
        UacIconBehavior _ = new(bAssociateAllUsers);

        fontPickerMain.DisplayFont(App.Settings.Font);
        printSettingsEditor.DisplaySettings(App.Settings.PrintSettings);

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
        fontPickerMain.SaveFont(innerFontSettings);
        exampleText.Font = FontUtils.GetFontBySettings(innerFontSettings, FontCategory.Monospace);
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
        fontPickerMain.SaveFont(App.Settings.Font);
        App.Settings.AutoReload = chAutoReload.Checked;
        App.Settings.DefaultEncodingWebName = (cbEncodings.SelectedIndex >= 0)
            ? encodings[cbEncodings.SelectedIndex].Encoding.WebName : null;
        printSettingsEditor.SaveSettings(App.Settings.PrintSettings);
        Close();
    }

    private void fontPickerMain_Changed(object sender, EventArgs e)
    {
        ApplyFont();
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
