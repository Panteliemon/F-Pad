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
public partial class PrintSettingsEditor : UserControl
{
    private int chPageNumberTop1;
    private int gbPageNumberTop1;

    public PrintSettingsEditor()
    {
        InitializeComponent();

        chPageNumberTop1 = chPageNumber.Top;
        gbPageNumberTop1 = gbPageNumber.Top;

        ChangeLayout();
    }

    private void chFileName_CheckedChanged(object sender, EventArgs e)
    {
        ChangeLayout();
    }

    private void chPageNumber_CheckedChanged(object sender, EventArgs e)
    {
        ChangeLayout();
    }

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
}
