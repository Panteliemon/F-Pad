using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad;

public partial class ReplaceForm : Form
{
    private static ReplaceForm instance;

    private Point topRight;

    private ReplaceForm(Point topRight)
    {
        InitializeComponent();
        this.topRight = topRight;
    }

    public static void Show(Form owner, Point topRight)
    {
        if (instance != null)
        {
            instance.BringToFront();
        }
        else
        {
            instance = new ReplaceForm(topRight);
            instance.Show(owner);
        }
    }

    public static void HideIfShown()
    {
        instance?.Close();
    }

    #region Event Handlers

    private void ReplaceForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        instance = null;
    }

    private void ReplaceForm_Load(object sender, EventArgs e)
    {
        Top = topRight.Y;
        Left = topRight.X - Width;
    }

    #endregion
}
