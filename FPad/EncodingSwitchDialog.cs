using FPad.Encodings;
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
public partial class EncodingSwitchDialog : Form
{
    public EncodingSwitchMethod? Result { get; private set; }

    private EncodingSwitchDialog(EncodingVm newEncoding)
    {
        InitializeComponent();

        label1.Text = "Switch to " + newEncoding.DisplayName;
    }

    public static EncodingSwitchMethod? ShowDialog(Form parent, EncodingVm newEncoding)
    {
        EncodingSwitchDialog dlg = new(newEncoding);
        dlg.ShowDialog(parent);
        return dlg.Result;
    }

    private void bReinterpret_Click(object sender, EventArgs e)
    {
        Result = EncodingSwitchMethod.Reinterpret;
        Close();
    }

    private void bUseForSaving_Click(object sender, EventArgs e)
    {
        Result = EncodingSwitchMethod.UseForSave;
        Close();
    }
}
