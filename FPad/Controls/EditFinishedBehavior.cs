using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad.Controls;

public class EditFinishedBehavior : IDisposable
{
    private TextBox textBox;

    public event EventHandler<string> EditFinished;

    public EditFinishedBehavior(TextBox textBox)
    {
        this.textBox = textBox;

        this.textBox.Leave += TextBox_Leave;
        this.textBox.KeyDown += TextBox_KeyDown;
    }

    public void Dispose()
    {
        if (textBox != null)
        {
            textBox.Leave -= TextBox_Leave;
            textBox.KeyDown -= TextBox_KeyDown;
            textBox = null;
        }
    }

    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (!textBox.Multiline && (e.KeyCode == Keys.Enter))
        {
            EditFinished?.Invoke(this, textBox.Text);
        }
    }

    private void TextBox_Leave(object sender, EventArgs e)
    {
        EditFinished?.Invoke(this, textBox.Text);
    }
}
