using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad;
public partial class EmbarrassmentWindow : Form
{
    private EmbarrassmentWindow()
    {
        InitializeComponent();

        Text = "Set File Associations - " + App.TITLE;

        pictureBox1.Image = App.LoadImage("howto_1_1.png");
        pictureBox2.Image = App.LoadImage("howto_1_2.png");
        pictureBox3.Image = App.LoadImage("howto_1_3.png");

        pictureBox4.Image = App.LoadImage("howto_2_1.png");
        pictureBox5.Image = App.LoadImage("howto_2_2.png");

        label7.Text = $"Select {App.TITLE}, then - \"Set default\"";
        label10.Text = $"Select {App.TITLE}, then - \"Always\"";
    }

    private void bOk_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void bSystemSettings_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "ms-settings:defaultapps",
            UseShellExecute = true
        });
    }

    public static void ShowDialog1(Form owner)
    {
        EmbarrassmentWindow window = new();
        window.ShowDialog(owner);
    }
}
