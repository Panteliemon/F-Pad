using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad;

public class Printer
{
    private string allText;
    private Font font;

    private bool enableHandlers;

    public PrintDocument Document { get; }

    public Printer(string allText, Font font)
    {
        Document = new PrintDocument();

        this.allText = allText;
        this.font = font;
    }

    public void Print()
    {
        Document.Print();
    }
}
