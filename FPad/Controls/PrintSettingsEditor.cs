using FPad.Settings.Print;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FPad.Controls;
public partial class PrintSettingsEditor : UserControl
{
    private int chPageNumberTop1;
    private int gbPageNumberTop1;
    private bool enableHandlers;

    /// <summary>
    /// Happens when the user changes some value
    /// </summary>
    public event EventHandler Changed;

    /// <summary>
    /// If true, <see cref="Changed"/> is fired on each input in template textbox.
    /// If false, <see cref="Changed"/> is fired on Enter or when the control loses focus.
    /// </summary>
    public bool ImmediatePageNumberTemplateChange { get; set; } = true;

    public PrintSettingsEditor()
    {
        InitializeComponent();

        chPageNumberTop1 = chPageNumber.Top;
        gbPageNumberTop1 = gbPageNumber.Top;

        EditFinishedBehavior tbTemplateBehavior = new EditFinishedBehavior(tbPageNumberTemplate);
        tbTemplateBehavior.EditFinished += tbPageNumberTemplate_EditFinished;

        toolTip1.SetToolTip(tbPageNumberTemplate, $"Use {Printer.PLACEHOLDER_PAGE} placeholder for page number, and {Printer.PLACEHOLDER_TOTAL} for total number of pages.");

        ChangeLayout();
        ChangeTamplateEnabled();
        enableHandlers = true;
    }

    public void DisplaySettings(PrintSettings settings)
    {
        enableHandlers = false;

        chFileName.Checked = settings.IncludeFileName;
        rbFnName.Checked = settings.FileNameContent == FileNameContent.Name;
        rbFnNameExt.Checked = settings.FileNameContent == FileNameContent.NameExt;
        rbFnFullPath.Checked = settings.FileNameContent == FileNameContent.FullPath;
        fontPickerFileName.DisplayFont(settings.FileNameFont);

        chPageNumber.Checked = settings.IncludePageNumber;
        rbPnStandard.Checked = !settings.UsePageNumberTemplate;
        rbPnTemplate.Checked = settings.UsePageNumberTemplate;
        tbPageNumberTemplate.Text = settings.PageNumberTemplate ?? string.Empty;
        rbPnALeft.Checked = settings.PageNumberAlignment == Settings.Print.HorizontalAlignment.Left;
        rbPnACenter.Checked = settings.PageNumberAlignment == Settings.Print.HorizontalAlignment.Center;
        rbPnARight.Checked = settings.PageNumberAlignment == Settings.Print.HorizontalAlignment.Right;
        fontPickerPageNumber.DisplayFont(settings.PageNumberFont);

        enableHandlers = true;

        ChangeLayout();
        ChangeTamplateEnabled();
    }

    public void SaveSettings(PrintSettings dest)
    {
        dest.IncludeFileName = chFileName.Checked;
        dest.FileNameContent = GetFileNameContent();
        fontPickerFileName.SaveFont(dest.FileNameFont);

        dest.IncludePageNumber = chPageNumber.Checked;
        dest.UsePageNumberTemplate = rbPnTemplate.Checked;
        dest.PageNumberTemplate = tbPageNumberTemplate.Text;
        dest.PageNumberAlignment = GetHorizontalAlignment();
        fontPickerPageNumber.SaveFont(dest.PageNumberFont);
    }

    private FileNameContent GetFileNameContent()
    {
        if (rbFnName.Checked)
            return FileNameContent.Name;
        else if (rbFnNameExt.Checked)
            return FileNameContent.NameExt;
        else
            return FileNameContent.FullPath;
    }

    private Settings.Print.HorizontalAlignment GetHorizontalAlignment()
    {
        if (rbPnALeft.Checked)
            return Settings.Print.HorizontalAlignment.Left;
        else if (rbPnACenter.Checked)
            return Settings.Print.HorizontalAlignment.Center;
        else
            return Settings.Print.HorizontalAlignment.Right;
    }

    #region Event Handlers

    private void chFileName_CheckedChanged(object sender, EventArgs e)
    {
        if (enableHandlers)
        {
            ChangeLayout();
            Notify();
        }
    }

    private void rbFnOption_CheckedChanged(object sender, EventArgs e)
    {
        if (enableHandlers && (sender is RadioButton rbs) && rbs.Checked)
        {
            Notify();
        }
    }

    private void fontPickerFileName_Changed(object sender, EventArgs e)
    {
        Notify();
    }

    private void chPageNumber_CheckedChanged(object sender, EventArgs e)
    {
        if (enableHandlers)
        {
            ChangeLayout();
            Notify();
        }
    }

    private void rbPnOption_CheckedChanged(object sender, EventArgs e)
    {
        if (enableHandlers && (sender is RadioButton rbs) && rbs.Checked)
        {
            ChangeTamplateEnabled();
            Notify();
        }
    }

    private void tbPageNumberTemplate_TextChanged(object sender, EventArgs e)
    {
        if (enableHandlers && ImmediatePageNumberTemplateChange)
        {
            Notify();
        }
    }

    private void tbPageNumberTemplate_EditFinished(object sender, string e)
    {
        if (!ImmediatePageNumberTemplateChange)
        {
            Notify();
        }
    }

    private void rbPnAlign_CheckedChanged(object sender, EventArgs e)
    {
        if (enableHandlers && (sender is RadioButton rbs) && rbs.Checked)
        {
            Notify();
        }
    }

    private void fontPickerPageNumber_Changed(object sender, EventArgs e)
    {
        Notify();
    }

    #endregion

    private void ChangeLayout()
    {
        gbFileName.Visible = chFileName.Checked;
        gbPageNumber.Visible = chPageNumber.Checked;

        if (chFileName.Checked)
        {
            chPageNumber.Top = chPageNumberTop1;
            gbPageNumber.Top = gbPageNumberTop1;
        }
        else
        {
            chPageNumber.Top = gbFileName.Top + 3;
            gbPageNumber.Top = chPageNumber.Top + (gbPageNumberTop1 - chPageNumberTop1);
        }

        if (chPageNumber.Checked)
        {
            Height = gbPageNumber.Bottom + 3;
        }
        else
        {
            Height = chPageNumber.Bottom + 3;
        }
    }

    private void ChangeTamplateEnabled()
    {
        tbPageNumberTemplate.Enabled = rbPnTemplate.Checked;
    }

    private void Notify()
    {
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
