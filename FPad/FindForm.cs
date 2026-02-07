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

public partial class FindForm : Form
{
    private static FindForm instance;

    private Point topRight;

    private FindForm(Point topRight)
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
            instance = new FindForm(topRight);
            instance.Show(owner);
        }
    }

    public static void HideIfShown()
    {
        instance?.Close();
    }

    #region Event Handlers

    private void FindForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        instance = null;
    }

    private void FindForm_Load(object sender, EventArgs e)
    {
        Top = topRight.Y;
        Left = topRight.X - Width;
    }

    #endregion
}
