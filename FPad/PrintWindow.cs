using FPad.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad;

public partial class PrintWindow : Form
{
    private const int DM_OUT_BUFFER = 2;
    private const int DM_IN_PROMPT = 4;
    private const int DM_IN_BUFFER = 8;

    private Printer printer;
    private PrintDocument document;
    private List<string> installedPrinters;
    private string selectedPrinter;

    private bool isFirstActivate = true;
    private bool enableHandlers;
    private int updatePrinterAttributesCallId = 0;

    private PrintWindow(Printer printer)
    {
        this.printer = printer;
        document = printer.Document;

        InitializeComponent();

        Text = "Print Options – " + App.TITLE;
        Icon = App.Icon;

        DialogResult = DialogResult.Cancel;

        installedPrinters = new List<string>();
        foreach (string printerName in PrinterSettings.InstalledPrinters)
        {
            installedPrinters.Add(printerName);
            cbPrinter.Items.Add(printerName);
        }
        selectedPrinter = document.PrinterSettings.PrinterName;
        cbPrinter.SelectedIndex = installedPrinters.IndexOf(selectedPrinter);

        printPreview.Document = document;
        _ = new DigitOnlyBehavior(tbFrom);
        _ = new DigitOnlyBehavior(tbTo);

        rbAll.Checked = true;
        rbPageRange.Checked = false;
        UpdatePagesEnabled();

        enableHandlers = true;
    }

    public static bool ShowDialog(Printer printer)
    {
        PrintWindow dlg = new(printer);
        return dlg.ShowDialog() == DialogResult.OK;
    }

    #region Event Handlers

    private void bOk_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void cbPrinter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (enableHandlers)
        {
            selectedPrinter = installedPrinters[cbPrinter.SelectedIndex];
            document.PrinterSettings.PrinterName = selectedPrinter;

            UpdatePrinterAttributes();
            printPreview.InvalidatePreview();
        }
    }

    private void rbAll_CheckedChanged(object sender, EventArgs e)
    {
        if (enableHandlers && rbAll.Checked)
        {
            rbPageRange.Checked = false;
            UpdatePagesEnabled();
        }
    }

    private void rbPageRange_CheckedChanged(object sender, EventArgs e)
    {
        if (enableHandlers && rbPageRange.Checked)
        {
            rbAll.Checked = false;
            UpdatePagesEnabled();
        }
    }

    private void bPrinterProperties_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(selectedPrinter))
            return;

        if (WinApi.OpenPrinterW(selectedPrinter, out nint hPrinter, 0))
        {
            try
            {
                int size = WinApi.DocumentPropertiesW(Handle, hPrinter, selectedPrinter, 0, 0, 0);
                if (size > 0)
                {
                    nint pDevMode = Marshal.AllocHGlobal(size);
                    try
                    {
                        int result = WinApi.DocumentPropertiesW(Handle, hPrinter, selectedPrinter, pDevMode, pDevMode,
                                                                DM_OUT_BUFFER | DM_IN_PROMPT | DM_IN_BUFFER);
                        if (result == 1) // OK
                        {
                            // Apply updated DEVMODE back to PrintDocument
                            document.PrinterSettings.SetHdevmode(pDevMode);
                            document.DefaultPageSettings.SetHdevmode(pDevMode);

                            printPreview.InvalidatePreview();
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(pDevMode);
                    }
                }
            }
            catch (Exception ex)
            {
                App.ShowError(ex);
            }
            finally
            {
                WinApi.ClosePrinter(hPrinter);
            }
        }
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        UpdatePrinterAttributes();
    }

    private void PrintWindow_Activated(object sender, EventArgs e)
    {
        if (isFirstActivate)
        {
            isFirstActivate = false;
            timer1.Enabled = true;

            UpdatePrinterAttributes(); // don't start in constructor, because sometimes too fast
        }
    }

    private void PrintWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
        timer1.Enabled = false;
    }

    #endregion

    private void UpdatePagesEnabled()
    {
        tbFrom.Enabled = rbPageRange.Checked;
        tbTo.Enabled = rbPageRange.Checked;
        lFrom.Enabled = rbPageRange.Checked;
        lTo.Enabled = rbPageRange.Checked;
    }

    private void UpdatePrinterAttributes()
    {
        // MSDN advises to not run GetPrinter on UI thread
        string localPrinterName = selectedPrinter;
        int localCallId = ++updatePrinterAttributesCallId;
        Task.Run(() =>
        {
            WinApi.PRINTER_INFO_2? printerInfo = GetPrinterInfo(localPrinterName);
            BeginInvoke(() =>
            {
                if (localCallId == updatePrinterAttributesCallId)
                {
                    if (printerInfo.HasValue)
                    {
                        lStatus.Text = PrinterStatusToStr(printerInfo.Value.Status);
                        lType.Text = printerInfo.Value.pDriverName;
                        lWhere.Text = printerInfo.Value.pPortName;
                        lComment.Text = printerInfo.Value.pComment;
                    }
                    else
                    {
                        lStatus.Text = "< error >";
                        lType.Text = "< error >";
                        lWhere.Text = "< error >";
                        lComment.Text = "< error >";
                    }
                }
            });
        });
    }

    private static WinApi.PRINTER_INFO_2? GetPrinterInfo(string printerName)
    {
        if (string.IsNullOrEmpty(printerName))
            return null;

        WinApi.PRINTER_INFO_2? result = null;
        if (WinApi.OpenPrinterW(printerName, out nint hPrinter, 0))
        {
            try
            {
                WinApi.GetPrinter(hPrinter, 2, 0, 0, out uint size);
                if (size > 0)
                {
                    nint pPrinterInfo = Marshal.AllocHGlobal((int)size);
                    try
                    {
                        if (WinApi.GetPrinter(hPrinter, 2, pPrinterInfo, size, out uint _))
                        {
                            result = Marshal.PtrToStructure<WinApi.PRINTER_INFO_2>(pPrinterInfo);
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(pPrinterInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                App.Discard(ex);
            }
            finally
            {
                WinApi.ClosePrinter(hPrinter);
            }
        }

        return result;
    }

    private static string PrinterStatusToStr(WinApi.PrinterStatus value)
    {
        if (value == 0)
            return "Ready";

        if (value.HasFlag(WinApi.PrinterStatus.Offline))
            return "Offline";

        // Errors
        if (value.HasFlag(WinApi.PrinterStatus.PaperJam))
            return "Paper jam";

        if (value.HasFlag(WinApi.PrinterStatus.PaperOut))
            return "Out of paper";

        if (value.HasFlag(WinApi.PrinterStatus.DoorOpen))
            return "Door open";

        if (value.HasFlag(WinApi.PrinterStatus.NoToner))
            return "No toner";

        if (value.HasFlag(WinApi.PrinterStatus.OutputBinFull))
            return "Output bin full";

        if (value.HasFlag(WinApi.PrinterStatus.Error))
            return "Error";

        // Normal operation
        if (value.HasFlag(WinApi.PrinterStatus.Paused))
            return "Paused";

        if (value.HasFlag(WinApi.PrinterStatus.Printing))
            return "Printing";

        if (value.HasFlag(WinApi.PrinterStatus.WarmingUp))
            return "Warming up";

        if (value.HasFlag(WinApi.PrinterStatus.Initializing))
            return "Initializing";

        if (value.HasFlag(WinApi.PrinterStatus.Busy))
            return "Busy";

        // Warnings (only if no "normal operation" flags and no errors)
        if (value.HasFlag(WinApi.PrinterStatus.TonerLow))
            return "Toner low";

        return "Unknown";
    }
}
