using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad;

/// <summary>
/// Adds SelectionChanged event to a textbox
/// </summary>
public class SelectionChangedBehavior : IDisposable
{
    private TextBox textBox;
    private int prevSelectionStart;
    private int prevSelectionLength;

    public event EventHandler SelectionChanged;

    public SelectionChangedBehavior(TextBox textBox)
    {
        this.textBox = textBox;
        textBox.MouseUp += OnMouse;
        textBox.MouseDown += OnMouse;
        textBox.MouseMove += OnMouse;
        textBox.KeyUp += OnKeyboard;
        textBox.TextChanged += OnTextChanged;

        prevSelectionStart = textBox.SelectionStart;
        prevSelectionLength = textBox.SelectionLength;
    }

    public void Dispose()
    {
        if (textBox != null)
        {
            textBox.MouseUp -= OnMouse;
            textBox.MouseDown -= OnMouse;
            textBox.MouseMove -= OnMouse;
            textBox.KeyUp -= OnKeyboard;
            textBox.TextChanged -= OnTextChanged;
            textBox = null;
        }
    }

    private void OnTextChanged(object sender, EventArgs e)
    {
        OnAnything();
    }

    private void OnKeyboard(object sender, KeyEventArgs e)
    {
        OnAnything();
    }

    private void OnMouse(object sender, MouseEventArgs e)
    {
        OnAnything();
    }

    private void OnAnything()
    {
        if ((textBox.SelectionStart != prevSelectionStart) || (textBox.SelectionLength != prevSelectionLength))
        {
            prevSelectionStart = textBox.SelectionStart;
            prevSelectionLength = textBox.SelectionLength;
            SelectionChanged?.Invoke(textBox, EventArgs.Empty);
        }
    }
}
