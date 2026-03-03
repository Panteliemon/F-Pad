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
    private Font font;

    public PrintDocument Document { get; }

    public Printer(string allText, Font font)
    {
        this.allText = allText;
        this.font = font;
        Document = new PrintDocument();

        Document.BeginPrint += Document_BeginPrint;
        Document.PrintPage += Document_PrintPage;
    }

    private void Document_BeginPrint(object sender, PrintEventArgs e)
    {
    }

    private void Document_PrintPage(object sender, PrintPageEventArgs e)
    {
        float lineHeight = font.GetHeight(e.Graphics);

        //e.Graphics.MeasureString()

        e.Graphics.DrawString(
            allText,
            font,
            Brushes.Black,
            e.MarginBounds
        );

        e.HasMorePages = false;
    }

    public void Print()
    {
        Document.Print();
    }
}
