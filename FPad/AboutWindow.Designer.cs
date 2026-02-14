namespace FPad;

partial class AboutWindow
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutWindow));
        picLogo = new System.Windows.Forms.PictureBox();
        labelTitle = new System.Windows.Forms.Label();
        labelBuild = new System.Windows.Forms.Label();
        linkLabelGithub = new System.Windows.Forms.LinkLabel();
        label1 = new System.Windows.Forms.Label();
        label2 = new System.Windows.Forms.Label();
        linkLabelIcons8 = new System.Windows.Forms.LinkLabel();
        groupBox1 = new System.Windows.Forms.GroupBox();
        textBox1 = new System.Windows.Forms.TextBox();
        ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
        groupBox1.SuspendLayout();
        SuspendLayout();
        // 
        // picLogo
        // 
        picLogo.Location = new System.Drawing.Point(12, 12);
        picLogo.Name = "picLogo";
        picLogo.Size = new System.Drawing.Size(128, 128);
        picLogo.TabIndex = 0;
        picLogo.TabStop = false;
        // 
        // labelTitle
        // 
        labelTitle.AutoSize = true;
        labelTitle.Font = new System.Drawing.Font("Bookman Old Style", 24F, System.Drawing.FontStyle.Bold);
        labelTitle.Location = new System.Drawing.Point(146, 12);
        labelTitle.Name = "labelTitle";
        labelTitle.Size = new System.Drawing.Size(74, 38);
        labelTitle.TabIndex = 1;
        labelTitle.Text = "===";
        // 
        // labelBuild
        // 
        labelBuild.AutoSize = true;
        labelBuild.Location = new System.Drawing.Point(146, 59);
        labelBuild.Name = "labelBuild";
        labelBuild.Size = new System.Drawing.Size(31, 15);
        labelBuild.TabIndex = 2;
        labelBuild.Text = "===";
        // 
        // linkLabelGithub
        // 
        linkLabelGithub.AutoSize = true;
        linkLabelGithub.Location = new System.Drawing.Point(146, 101);
        linkLabelGithub.Name = "linkLabelGithub";
        linkLabelGithub.Size = new System.Drawing.Size(217, 15);
        linkLabelGithub.TabIndex = 3;
        linkLabelGithub.TabStop = true;
        linkLabelGithub.Text = "https://github.com/Panteliemon/F-Pad";
        linkLabelGithub.LinkClicked += linkLabelGithub_LinkClicked;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(146, 86);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(75, 15);
        label1.TabIndex = 4;
        label1.Text = "Source code:";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(146, 125);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(54, 15);
        label2.TabIndex = 5;
        label2.Text = "Icons by:";
        // 
        // linkLabelIcons8
        // 
        linkLabelIcons8.AutoSize = true;
        linkLabelIcons8.Location = new System.Drawing.Point(203, 125);
        linkLabelIcons8.Name = "linkLabelIcons8";
        linkLabelIcons8.Size = new System.Drawing.Size(41, 15);
        linkLabelIcons8.TabIndex = 6;
        linkLabelIcons8.TabStop = true;
        linkLabelIcons8.Text = "Icons8";
        linkLabelIcons8.LinkClicked += linkLabelIcons8_LinkClicked;
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(textBox1);
        groupBox1.Location = new System.Drawing.Point(12, 157);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new System.Drawing.Size(431, 124);
        groupBox1.TabIndex = 7;
        groupBox1.TabStop = false;
        groupBox1.Text = "Command line arguments";
        // 
        // textBox1
        // 
        textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        textBox1.Location = new System.Drawing.Point(6, 22);
        textBox1.Multiline = true;
        textBox1.Name = "textBox1";
        textBox1.ReadOnly = true;
        textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        textBox1.Size = new System.Drawing.Size(419, 96);
        textBox1.TabIndex = 0;
        textBox1.Text = resources.GetString("textBox1.Text");
        // 
        // AboutWindow
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(455, 293);
        Controls.Add(groupBox1);
        Controls.Add(linkLabelIcons8);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(linkLabelGithub);
        Controls.Add(labelBuild);
        Controls.Add(labelTitle);
        Controls.Add(picLogo);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        KeyPreview = true;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "AboutWindow";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "About";
        KeyDown += AboutWindow_KeyDown;
        ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.PictureBox picLogo;
    private System.Windows.Forms.Label labelTitle;
    private System.Windows.Forms.Label labelBuild;
    private System.Windows.Forms.LinkLabel linkLabelGithub;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.LinkLabel linkLabelIcons8;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox textBox1;
}