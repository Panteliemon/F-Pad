using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad.Controls;

public class GoodTextBox : TextBox
{
    private const int WM_LBUTTONDBLCLK = 0x0203;
    private const int CHAR_SPACE = 0;
    private const int CHAR_NONWORD = 1;
    private const int CHAR_WORD = 2;

    private int prevSelectionStart;
    private int prevSelectionLength;

    public event EventHandler SelectionChanged;

    public GoodTextBox()
    {
        prevSelectionStart = SelectionStart;
        prevSelectionLength = SelectionLength;

        MouseUp += OnMouse;
        MouseDown += OnMouse;
        MouseMove += OnMouse;
        KeyUp += OnKeyboard;
        TextChanged += OnTextChanged;
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
                int initialCharQuality;
                if (initialCharIndex == Text.Length)
                {
                    initialCharIndex--;
                    initialCharQuality = GetCharQuality(Text[initialCharIndex]);
                }
                else
                {
                    initialCharQuality = GetCharQuality(Text[initialCharIndex]);
                    if ((initialCharQuality < CHAR_WORD) && (initialCharIndex > 0))
                    {
                        int prevCharQuality = GetCharQuality(Text[initialCharIndex - 1]);
                        if (prevCharQuality > initialCharQuality)
                        {
                            initialCharIndex--;
                            initialCharQuality = prevCharQuality;
                        }
                    }
                }

                if (initialCharQuality > CHAR_SPACE)
                {
                    int selStart = initialCharIndex;
                    int selEnd = initialCharIndex + 1;

                    // Spread forward
                    while (selEnd < Text.Length)
                    {
                        if (GetCharQuality(Text[selEnd]) != initialCharQuality)
                            break;
                        selEnd++;
                    }
                    // Spread backwards
                    while (selStart > 0)
                    {
                        if (GetCharQuality(Text[selStart - 1]) != initialCharQuality)
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetCharQuality(char c)
    {
        if (c <= 32)
            return CHAR_SPACE;
        else
            return StringUtils.IsPartOfWord(c) ? CHAR_WORD : CHAR_NONWORD;
    }

    private void OnTextChanged(object sender, EventArgs e)
    {
        OnPotentialSelectionChange();
    }

    private void OnKeyboard(object sender, KeyEventArgs e)
    {
        OnPotentialSelectionChange();
    }

    private void OnMouse(object sender, MouseEventArgs e)
    {
        OnPotentialSelectionChange();
    }

    private void OnPotentialSelectionChange()
    {
        if ((SelectionStart != prevSelectionStart) || (SelectionLength != prevSelectionLength))
        {
            prevSelectionStart = SelectionStart;
            prevSelectionLength = SelectionLength;
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
