using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad.Controls;

public class ToolStripStatusButton : ToolStripStatusLabel
{
    private bool isPressed;

    public bool IsPressed
    {
        get => isPressed;
        set
        {
            if (value != isPressed)
            {
                isPressed = value;
                BorderStyle = isPressed ? Border3DStyle.SunkenOuter : Border3DStyle.RaisedOuter;
            }
        }
    }

    public ToolStripStatusButton()
    {
        BorderSides = ToolStripStatusLabelBorderSides.All;
        IsPressed = false;

        MouseDown += ToolStripStatusButton_MouseDown;
        MouseUp += ToolStripStatusButton_MouseUp;
        MouseLeave += ToolStripStatusButton_MouseLeave;
    }

    private void ToolStripStatusButton_MouseLeave(object sender, EventArgs e)
    {
        IsPressed = false;
    }

    private void ToolStripStatusButton_MouseUp(object sender, MouseEventArgs e)
    {
        IsPressed = false;
    }

    private void ToolStripStatusButton_MouseDown(object sender, MouseEventArgs e)
    {
        IsPressed = true;
    }
}
