using FPad.Edit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad.Controls;

public class GoodTextBox : TextBox
{
    private const int WM_LBUTTONDBLCLK = 0x0203;

    private Selection prevSelection;
    private Selection selectionBeforeEdit;

    public Selection Selection => new Selection(SelectionStart, SelectionLength);
    public event EventHandler SelectionChanged;

    public Selection SelectionBeforeEdit => selectionBeforeEdit;

    public GoodTextBox()
    {
        prevSelection = Selection;
        selectionBeforeEdit = Selection;

        MouseUp += OnMouse;
        MouseDown += OnMouse;
        MouseMove += OnMouse;
        KeyDown += OnKeyDown;
        KeyUp += OnKeyUp;
        TextChanged += OnTextChanged;
    }

    public new string Text
    {
        get => base.Text;
        set
        {
            selectionBeforeEdit = Selection;
            base.Text = value;
        }
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_LBUTTONDBLCLK)
        {
            OnDoubleClick(EventArgs.Empty);

            // Select word correctly
            if (Text.Length > 0)
            {
                // Char under caret determines whether we select whole word or whole non-word.
                // If there are two chars of different quality at different sides of a caret -
                // prefer 'word' over 'non-word' and 'non-word' over 'space'.
                int initialCharIndex = SelectionStart;
                ConseqCharType initialCharType;
                if (initialCharIndex == Text.Length)
                {
                    initialCharIndex--;
                    initialCharType = StringUtils.GetCharType(Text[initialCharIndex]);
                }
                else
                {
                    initialCharType = StringUtils.GetCharType(Text[initialCharIndex]);
                    if ((initialCharType < ConseqCharType.Word) && (initialCharIndex > 0))
                    {
                        ConseqCharType prevCharType = StringUtils.GetCharType(Text[initialCharIndex - 1]);
                        if (prevCharType > initialCharType)
                        {
                            initialCharIndex--;
                            initialCharType = prevCharType;
                        }
                    }
                }

                if (initialCharType > ConseqCharType.Space)
                {
                    int selStart = initialCharIndex;
                    int selEnd = initialCharIndex + 1;

                    // Spread forward
                    while (selEnd < Text.Length)
                    {
                        if (StringUtils.GetCharType(Text[selEnd]) != initialCharType)
                            break;
                        selEnd++;
                    }
                    // Spread backwards
                    while (selStart > 0)
                    {
                        if (StringUtils.GetCharType(Text[selStart - 1]) != initialCharType)
                            break;
                        selStart--;
                    }

                    SelectionStart = selStart;
                    SelectionLength = selEnd - selStart;
                    OnPotentialSelectionChange();
                }
            }

            // Prevent default
            return;
        }

        base.WndProc(ref m);
    }

    private void OnTextChanged(object sender, EventArgs e)
    {
        OnPotentialSelectionChange();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        selectionBeforeEdit = Selection;
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        OnPotentialSelectionChange();
    }

    private void OnMouse(object sender, MouseEventArgs e)
    {
        OnPotentialSelectionChange();
    }

    private void OnPotentialSelectionChange()
    {
        if (Selection != prevSelection)
        {
            prevSelection = Selection;
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
