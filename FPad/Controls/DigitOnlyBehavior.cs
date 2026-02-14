using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad.Controls;

public class DigitOnlyBehavior : IDisposable
{
    TextBox textBox;

    public DigitOnlyBehavior(TextBox textBox)
    {
        this.textBox = textBox;

        textBox.KeyPress += TextBox_KeyPress;
    }

    public void Dispose()
    {
        if (textBox != null)
        {
            textBox.KeyPress -= TextBox_KeyPress;
            textBox = null;
        }
    }

    private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            e.Handled = true;
    }
}
