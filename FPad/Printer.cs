using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad;

public class Printer
{
    private string allText;
    private int currentStartPosition;
    private int pagesCount;
    private Font font;

    public PrintDocument Document { get; }

    public int? PagesCount { get; set; }
    public event EventHandler PagesCountChanged;

    public Printer(string allText, Font font)
    {
        this.allText = allText;
        this.font = font;
        Document = new PrintDocument();

        Document.BeginPrint += Document_BeginPrint;
        Document.PrintPage += Document_PrintPage;
        Document.EndPrint += Document_EndPrint;
    }

    private void Document_BeginPrint(object sender, PrintEventArgs e)
    {
        currentStartPosition = 0;
        pagesCount = 0;
        PagesCount = null;
    }

    private void Document_PrintPage(object sender, PrintPageEventArgs e)
    {
        ReadOnlySpan<char> allTextSpan = allText.AsSpan();
        float lineHeight = font.GetHeight(e.Graphics);
        float verticalOffset = 0f;

        do
        {
            ReadOnlySpan<char> textLeft = allTextSpan[currentStartPosition..];

            StringUtils.IterateOverSplitByLines(textLeft,
                (ReadOnlySpan<char> line, int lineIndex, int lineStartPosition, int nextLineStartPosition, bool isLastLine) =>
                {
                    // Without word wrap for now
                    e.Graphics.DrawString(line, font, Brushes.Black,
                        new PointF(e.MarginBounds.Left, e.MarginBounds.Top + verticalOffset));

                    currentStartPosition += (nextLineStartPosition - lineStartPosition);
                    verticalOffset += lineHeight;
                    return false;
                });
        }
        while (((float)e.MarginBounds.Height - verticalOffset >= lineHeight) // enough space for the next line on current page
               && (currentStartPosition < allText.Length));

        pagesCount++;
        e.HasMorePages = currentStartPosition < allText.Length;
    }

    private void Document_EndPrint(object sender, PrintEventArgs e)
    {
        PagesCount = pagesCount;
        PagesCountChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Print()
    {
        Document.Print();
    }
}
