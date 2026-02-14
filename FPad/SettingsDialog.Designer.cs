namespace FPad;

partial class SettingsDialog
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
        components = new System.ComponentModel.Container();
        groupBox1 = new System.Windows.Forms.GroupBox();
        chWrap = new System.Windows.Forms.CheckBox();
        chItalic = new System.Windows.Forms.CheckBox();
        chBold = new System.Windows.Forms.CheckBox();
        tbFontSize = new System.Windows.Forms.NumericUpDown();
        label3 = new System.Windows.Forms.Label();
        slFontSize = new System.Windows.Forms.TrackBar();
        label2 = new System.Windows.Forms.Label();
        cbFonts = new System.Windows.Forms.ComboBox();
        label1 = new System.Windows.Forms.Label();
        exampleText = new System.Windows.Forms.TextBox();
        bSave = new System.Windows.Forms.Button();
        bCancel = new System.Windows.Forms.Button();
        chAutoReload = new System.Windows.Forms.CheckBox();
        toolTip = new System.Windows.Forms.ToolTip(components);
        groupBox1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)tbFontSize).BeginInit();
        ((System.ComponentModel.ISupportInitialize)slFontSize).BeginInit();
        SuspendLayout();
        // 
        // groupBox1
        // 
        groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        groupBox1.Controls.Add(chWrap);
        groupBox1.Controls.Add(chItalic);
        groupBox1.Controls.Add(chBold);
        groupBox1.Controls.Add(tbFontSize);
        groupBox1.Controls.Add(label3);
        groupBox1.Controls.Add(slFontSize);
        groupBox1.Controls.Add(label2);
        groupBox1.Controls.Add(cbFonts);
        groupBox1.Controls.Add(label1);
        groupBox1.Controls.Add(exampleText);
        groupBox1.Location = new System.Drawing.Point(12, 12);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new System.Drawing.Size(573, 312);
        groupBox1.TabIndex = 0;
        groupBox1.TabStop = false;
        groupBox1.Text = "Font";
        // 
        // chWrap
        // 
        chWrap.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        chWrap.AutoSize = true;
        chWrap.Location = new System.Drawing.Point(6, 285);
        chWrap.Name = "chWrap";
        chWrap.Size = new System.Drawing.Size(202, 19);
        chWrap.TabIndex = 9;
        chWrap.Text = "Wrap Lines (for this preview only)";
        chWrap.UseVisualStyleBackColor = true;
        chWrap.CheckedChanged += chWrap_CheckedChanged;
        // 
        // chItalic
        // 
        chItalic.AutoSize = true;
        chItalic.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
        chItalic.Location = new System.Drawing.Point(375, 39);
        chItalic.Name = "chItalic";
        chItalic.Size = new System.Drawing.Size(51, 19);
        chItalic.TabIndex = 8;
        chItalic.Text = "Italic";
        chItalic.UseVisualStyleBackColor = true;
        chItalic.CheckedChanged += chItalic_CheckedChanged;
        // 
        // chBold
        // 
        chBold.AutoSize = true;
        chBold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        chBold.Location = new System.Drawing.Point(287, 39);
        chBold.Name = "chBold";
        chBold.Size = new System.Drawing.Size(51, 19);
        chBold.TabIndex = 7;
        chBold.Text = "Bold";
        chBold.UseVisualStyleBackColor = true;
        chBold.CheckedChanged += chBold_CheckedChanged;
        // 
        // tbFontSize
        // 
        tbFontSize.Location = new System.Drawing.Point(42, 67);
        tbFontSize.Maximum = new decimal(new int[] { 128, 0, 0, 0 });
        tbFontSize.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
        tbFontSize.Name = "tbFontSize";
        tbFontSize.Size = new System.Drawing.Size(73, 23);
        tbFontSize.TabIndex = 6;
        tbFontSize.Value = new decimal(new int[] { 5, 0, 0, 0 });
        tbFontSize.ValueChanged += tbFontSize_ValueChanged;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new System.Drawing.Point(6, 69);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(30, 15);
        label3.TabIndex = 5;
        label3.Text = "Size:";
        // 
        // slFontSize
        // 
        slFontSize.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        slFontSize.Location = new System.Drawing.Point(6, 92);
        slFontSize.Maximum = 36;
        slFontSize.Minimum = 5;
        slFontSize.Name = "slFontSize";
        slFontSize.Size = new System.Drawing.Size(561, 45);
        slFontSize.TabIndex = 4;
        slFontSize.Value = 5;
        slFontSize.Scroll += slFontSize_Scroll;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(6, 140);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(51, 15);
        label2.TabIndex = 3;
        label2.Text = "Preview:";
        // 
        // cbFonts
        // 
        cbFonts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cbFonts.FormattingEnabled = true;
        cbFonts.Location = new System.Drawing.Point(6, 37);
        cbFonts.Name = "cbFonts";
        cbFonts.Size = new System.Drawing.Size(265, 23);
        cbFonts.TabIndex = 2;
        cbFonts.SelectedIndexChanged += cbFonts_SelectedIndexChanged;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(6, 19);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(42, 15);
        label1.TabIndex = 1;
        label1.Text = "Name:";
        // 
        // exampleText
        // 
        exampleText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        exampleText.BackColor = System.Drawing.SystemColors.Window;
        exampleText.Location = new System.Drawing.Point(6, 158);
        exampleText.Multiline = true;
        exampleText.Name = "exampleText";
        exampleText.ReadOnly = true;
        exampleText.Size = new System.Drawing.Size(561, 121);
        exampleText.TabIndex = 0;
        // 
        // bSave
        // 
        bSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        bSave.Location = new System.Drawing.Point(387, 406);
        bSave.Name = "bSave";
        bSave.Size = new System.Drawing.Size(96, 32);
        bSave.TabIndex = 1;
        bSave.Text = "OK";
        bSave.UseVisualStyleBackColor = true;
        bSave.Click += bSave_Click;
        // 
        // bCancel
        // 
        bCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        bCancel.Location = new System.Drawing.Point(489, 406);
        bCancel.Name = "bCancel";
        bCancel.Size = new System.Drawing.Size(96, 32);
        bCancel.TabIndex = 2;
        bCancel.Text = "Cancel";
        bCancel.UseVisualStyleBackColor = true;
        bCancel.Click += bCancel_Click;
        // 
        // chAutoReload
        // 
        chAutoReload.AutoSize = true;
        chAutoReload.Location = new System.Drawing.Point(12, 330);
        chAutoReload.Name = "chAutoReload";
        chAutoReload.Size = new System.Drawing.Size(121, 19);
        chAutoReload.TabIndex = 3;
        chAutoReload.Text = "Automatic Reload";
        toolTip.SetToolTip(chAutoReload, "Automatically reload opened file, if another program modifies it and there are no unsaved changes in current editor.");
        chAutoReload.UseVisualStyleBackColor = true;
        // 
        // SettingsDialog
        // 
        AcceptButton = bSave;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        CancelButton = bCancel;
        ClientSize = new System.Drawing.Size(597, 450);
        Controls.Add(chAutoReload);
        Controls.Add(bCancel);
        Controls.Add(bSave);
        Controls.Add(groupBox1);
        MinimizeBox = false;
        Name = "SettingsDialog";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Load += SettingsDialog_Load;
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)tbFontSize).EndInit();
        ((System.ComponentModel.ISupportInitialize)slFontSize).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button bSave;
    private System.Windows.Forms.Button bCancel;
    private System.Windows.Forms.TextBox exampleText;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbFonts;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.NumericUpDown tbFontSize;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TrackBar slFontSize;
    private System.Windows.Forms.CheckBox chItalic;
    private System.Windows.Forms.CheckBox chBold;
    private System.Windows.Forms.CheckBox chWrap;
    private System.Windows.Forms.CheckBox chAutoReload;
    private System.Windows.Forms.ToolTip toolTip;
}