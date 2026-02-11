using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPad;
public partial class AboutWindow : Form
{
    public AboutWindow()
    {
        InitializeComponent();
        Icon = App.Icon;
        Text = "About – " + App.TITLE;

        picLogo.Image = App.LoadImage("logo.png");
        labelTitle.Text = $"{App.TITLE} {App.Version.Major}.{App.Version.Minor}";
        labelBuild.Text = "Build " + App.BuildDate.ToShortDateString();
    }

    private void AboutWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
            Close();
    }

    private void linkLabelGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        OpenLink(linkLabelGithub.Text);
    }

    private void linkLabelIcons8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        OpenLink("https://icons8.com");
    }

    private static void OpenLink(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
        }
    }
}
