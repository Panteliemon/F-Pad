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
    private List<string> installedPrinters;
    private string selectedPrinter;

    private bool enableHandlers;

    private PrintWindow(Printer printer)
    {
        this.printer = printer;

        InitializeComponent();

        Text = "Print – " + App.TITLE;
        Icon = App.Icon;

        DialogResult = DialogResult.Cancel;

        installedPrinters = new List<string>();
        foreach (string printerName in PrinterSettings.InstalledPrinters)
        {
            installedPrinters.Add(printerName);
            cbPrinter.Items.Add(printerName);
        }
        selectedPrinter = printer.Document.PrinterSettings.PrinterName;
        cbPrinter.SelectedIndex = installedPrinters.IndexOf(selectedPrinter);

        printPreview.Document = printer.Document;
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
                            printer.Document.PrinterSettings.SetHdevmode(pDevMode);
                            printer.Document.DefaultPageSettings.SetHdevmode(pDevMode);

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

    #endregion

    private void UpdatePagesEnabled()
    {
        tbFrom.Enabled = rbPageRange.Checked;
        tbTo.Enabled = rbPageRange.Checked;
        lFrom.Enabled = rbPageRange.Checked;
        lTo.Enabled = rbPageRange.Checked;
    }
}
