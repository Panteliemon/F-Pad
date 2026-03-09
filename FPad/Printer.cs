using FPad.Settings;
using FPad.Settings.Print;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPad;

public class Printer
{
    private string allText;
    private string documentFullPath;

    private Font mainTextFont;
    private Font fileNameFont;
    private Font pageNumberFont;
    /// <summary>
    /// Null if not included
    /// </summary>
    private FileNameContent? fileNameContent;
    /// <summary>
    /// Null if template is not used
    /// </summary>
    private string pageNumberTemplate;
    /// <summary>
    /// Null if not included
    /// </summary>
    private HorizontalAlignment? pageNumberAlignment;

    private int currentStartPosition;
    private int currentPage; // 1-based
    private int pagesCount;
    private int documentCopiesLeft;
    private int pageCopiesLeft;
    private CancellationToken ct;

    public PrintDocument Document { get; }

    public int NumberOfCopies { get; set; } = 1;
    public bool Collate { get; set; } = true;
    /// <summary>
    /// 1-based; set to 0 for "all pages"
    /// </summary>
    public int PageFrom { get; set; }
    /// <summary>
    /// 1-based; set to 0 for "all pages"
    /// </summary>
    public int PageTo { get; set; }

    public int? PagesCount { get; set; }
    public event EventHandler PagesCountDetermined;

    public Printer(string allText, Font mainTextFont, string documentFullPath)
    {
        this.allText = allText;
        this.mainTextFont = mainTextFont;
        this.documentFullPath = documentFullPath;

        Document = new PrintDocument();

        Document.BeginPrint += Document_BeginPrint;
        Document.PrintPage += Document_PrintPage;
        Document.EndPrint += Document_EndPrint;
    }

    public void SetSettings(PrintSettings printSettings)
    {
        fileNameFont = FontUtils.GetFontBySettings(printSettings.FileNameFont, FontCategory.Serif);
        pageNumberFont = FontUtils.GetFontBySettings(printSettings.PageNumberFont, FontCategory.Serif);

        fileNameContent = printSettings.IncludeFileName ? printSettings.FileNameContent : null;
        pageNumberAlignment = printSettings.IncludePageNumber ? printSettings.PageNumberAlignment : null;
        pageNumberTemplate = printSettings.UsePageNumberTemplate ? printSettings.PageNumberTemplate : null;
    }

    public void Print(CancellationToken ct)
    {
        if (fileNameFont == null)
            throw new InvalidOperationException("Settings not set");
        
        this.ct = ct;

        Document.Print();
    }

    private void Document_BeginPrint(object sender, PrintEventArgs e)
    {
        currentStartPosition = 0;
        currentPage = 1;
        pagesCount = 0;
        documentCopiesLeft = NumberOfCopies;
        pageCopiesLeft = NumberOfCopies;
    }

    private void Document_PrintPage(object sender, PrintPageEventArgs e)
    {
        ReadOnlySpan<char> allTextSpan = allText.AsSpan();

        // Outer cycle skips over pages excluded from printing
        bool printed = false;
        do
        {
            // DrawString line by line, because multiline one reports like lines fit, while
            // it cuts tails of letters in the bottommost line, so that row doesn't really fit.
            // And if we draw line by line ourselves that means we also must perform word wrap ourselves.

            float lineHeight = mainTextFont.GetHeight(e.Graphics);
            float verticalOffset = 0f;
            int startPositionForCurrentPage = currentStartPosition;
            bool isCurrentPageIncluded = IsPageIncluded(currentPage);

            // ===== Print one page (main text) =====
            do
            {
                ReadOnlySpan<char> textLeft = allTextSpan[currentStartPosition..];

                StringUtils.IterateOverSplitByLines(textLeft,
                    (ReadOnlySpan<char> line, int lineIndex, int lineStartPosition, int nextLineStartPosition, bool isLastLine) =>
                    {
                        ReadOnlySpan<char> printableLine = line.TrimEnd();
                        bool wordWrapped = false;
                        if (line.Length > 0)
                        {
                            e.Graphics.MeasureString(printableLine, mainTextFont,
                                new SizeF(e.MarginBounds.Width, lineHeight),
                                null,
                                out int charactersFitted,
                                out int _);

                            // For some reason looks better if we draw by 1 symbol more than it "fits".
                            charactersFitted++;

                            if (charactersFitted < printableLine.Length)
                            {
                                int initialPrintableLineLength = printableLine.Length;
                                printableLine = StringUtils.CutOnSpaceToFit(printableLine, charactersFitted, out int continuationStart);
                                // Checks that the symbols which didn't fit aren't spaces and we really have some tail left
                                if (continuationStart < initialPrintableLineLength)
                                {
                                    currentStartPosition += continuationStart;
                                    wordWrapped = true;
                                }
                            }

                            if (isCurrentPageIncluded)
                            {
                                e.Graphics.DrawString(printableLine, mainTextFont, Brushes.Black,
                                    new PointF(e.MarginBounds.Left, e.MarginBounds.Top + verticalOffset));
                            }
                        }

                        verticalOffset += lineHeight;
                        if (!wordWrapped)
                        {
                            currentStartPosition += (nextLineStartPosition - lineStartPosition);
                        }

                        // We take the first line only, we don't iterate completely over split.
                        // Break inner (outer) cycle:
                        return false;
                    });

                if (ct.IsCancellationRequested)
                {
                    e.HasMorePages = false;
                    return;
                }
            }
            while (((float)e.MarginBounds.Height - verticalOffset >= lineHeight) // enough space for the next line on current page
                   && (currentStartPosition < allText.Length));

            // ===== Print file name =====
            if (fileNameContent.HasValue && isCurrentPageIncluded)
            {
                string fileNameStr = fileNameContent.Value switch
                {
                    FileNameContent.Name => Path.GetFileNameWithoutExtension(documentFullPath),
                    FileNameContent.NameExt => Path.GetFileName(documentFullPath),
                    _ => documentFullPath
                };

                SizeF fileNameSize = e.Graphics.MeasureString(fileNameStr, fileNameFont);
                float x = (float)e.MarginBounds.Left / 2.0f;
                float y = ((float)e.MarginBounds.Top - fileNameSize.Height) / 2.0f;
                if (y < 0.0f)
                    y = 0.0f;

                e.Graphics.DrawString(fileNameStr, fileNameFont, Brushes.Black, new PointF(x, y));
            }

            // ===== Print page number =====
            if (pageNumberAlignment.HasValue && isCurrentPageIncluded)
            {
                string pageNumberStr = pageNumberTemplate == null
                    ? currentPage.ToString()
                    : pageNumberTemplate.Replace("{page}", currentPage.ToString())
                                        // Cannot know number of pages until printed to the end ¯\_(ツ)_/¯
                                        // Provided that it only depends on document's text and document's font - both immutable here -
                                        // use first time calculated value, if any.
                                        .Replace("{total}", (PagesCount ?? 0).ToString());
                
                SizeF pageNumberSize = e.Graphics.MeasureString(pageNumberStr, pageNumberFont);
                float y = ((float)e.MarginBounds.Bottom + (float)e.PageBounds.Bottom - pageNumberSize.Height) / 2.0f;
                if (y + pageNumberSize.Height > (float)e.PageBounds.Bottom)
                    y = (float)e.PageBounds.Bottom - pageNumberSize.Height;

                float x = pageNumberAlignment.Value switch
                {
                    HorizontalAlignment.Left => e.MarginBounds.Left,
                    HorizontalAlignment.Right => e.MarginBounds.Right - pageNumberSize.Width,
                    _ => ((float)e.MarginBounds.Left + (float)e.MarginBounds.Right - pageNumberSize.Width) / 2.0f
                };

                e.Graphics.DrawString(pageNumberStr, pageNumberFont, Brushes.Black, new PointF(x, y));
            }

            if (isCurrentPageIncluded)
                printed = true;

            // ===== Proceed to next page =====
            if (Collate) // All pages of the document, then next document
            {
                // Count pages only if the last copy
                if (documentCopiesLeft <= 1)
                    pagesCount++;

                if (currentStartPosition >= allText.Length) // ==
                {
                    documentCopiesLeft--;
                    currentStartPosition = 0; // restart the document on next iteration
                    currentPage = 1;
                    e.HasMorePages = documentCopiesLeft > 0;
                }
                else
                {
                    currentPage++;
                    e.HasMorePages = true;
                    // Went out of printable range when printing last copy:
                    if (isCurrentPageIncluded && !IsPageIncluded(currentPage) && (documentCopiesLeft <= 1))
                        e.HasMorePages = false;
                }
            }
            else // All copies of the page, then next page
            {
                pageCopiesLeft--;
                if (pageCopiesLeft > 0)
                {
                    currentStartPosition = startPositionForCurrentPage; // restart the page on next iteration
                    e.HasMorePages = true;
                }
                else
                {
                    // Next page on next iteration
                    currentPage++;
                    pagesCount++;
                    pageCopiesLeft = NumberOfCopies;
                    e.HasMorePages = currentStartPosition < allText.Length;
                    // Went out of printable range
                    if (isCurrentPageIncluded && !IsPageIncluded(currentPage))
                        e.HasMorePages = false;
                }
            }
        } while (!printed && e.HasMorePages);

        if (ct.IsCancellationRequested)
            e.HasMorePages = false;
    }

    private void Document_EndPrint(object sender, PrintEventArgs e)
    {
        if (!PagesCount.HasValue)
        {
            PagesCount = pagesCount;
            PagesCountDetermined?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool IsPageIncluded(int pageNumber)
    {
        if (PageFrom == 0)
        {
            if (PageTo == 0)
            {
                return true;
            }
            else
            {
                return (pageNumber <= PageTo) && (pageNumber >= 1);
            }
        }
        else
        {
            if (PageTo == 0)
            {
                return (pageNumber >= PageFrom) && (PageFrom >= 1);
            }
            else
            {
                return (pageNumber >= PageFrom) && (pageNumber <= PageTo) && (PageFrom >= 1);
            }
        }
    }
}
