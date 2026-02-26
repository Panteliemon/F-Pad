using FPad.Edit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad.Controls;

public class GoodTextBox : TextBox
{
    [DllImport("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

    private const int EM_GETFIRSTVISIBLELINE = 0xCE;
    private const int EM_LINESCROLL = 0xB6;
    private const int WM_SETREDRAW = 0xB;

    private const int WM_LBUTTONDBLCLK = 0x0203;

    private Selection prevSelection;
    private Selection selectionBeforeEdit;

    public Selection Selection => new Selection(SelectionStart, SelectionLength);
    public event EventHandler SelectionChanged;

    public Selection SelectionBeforeEdit => selectionBeforeEdit;
    /// <summary>
    /// Valid during TextChanged event.
    /// Setting this property does nothing, and is mainly here for possibility
    /// to set null for garbage collection.
    /// </summary>
    public string TextBeforeEdit { get; set; }

    public GoodTextBox()
    {
        prevSelection = Selection;
        selectionBeforeEdit = Selection;
        TextBeforeEdit = Text;

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
            TextBeforeEdit = base.Text;

            // TextBox internally resets scroll position to 0 when Text property is changed,
            // and then ScrollToCaret scrolls from zero to that line, making the line the bottommost
            // (the line jumps to bottom). Here is how to avoid:
            
            SendMessage(Handle, WM_SETREDRAW, 0, 0); // prevent flicker

            int firstVisibleLine = SendMessage(Handle, EM_GETFIRSTVISIBLELINE, 0, 0);
            base.Text = value; // this resets scroll to 0
            SendMessage(Handle, EM_LINESCROLL, 0, firstVisibleLine); // "scroll" back to where we were - might be out of range
            ScrollToCaret(); // restore caret position not from 0, but from where we were before

            SendMessage(Handle, WM_SETREDRAW, 1, 0);
            Refresh(); // this is because Refresh was disabled entire time
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
        TextBeforeEdit = Text;
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
