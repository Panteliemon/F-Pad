namespace FPad.Controls;

partial class FontPicker
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        label1 = new System.Windows.Forms.Label();
        cbFontName = new System.Windows.Forms.ComboBox();
        label2 = new System.Windows.Forms.Label();
        tbSize = new System.Windows.Forms.NumericUpDown();
        chBold = new System.Windows.Forms.CheckBox();
        chItalic = new System.Windows.Forms.CheckBox();
        slSize = new System.Windows.Forms.TrackBar();
        ((System.ComponentModel.ISupportInitialize)tbSize).BeginInit();
        ((System.ComponentModel.ISupportInitialize)slSize).BeginInit();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(0, 3);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(42, 15);
        label1.TabIndex = 0;
        label1.Text = "Name:";
        // 
        // cbFontName
        // 
        cbFontName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        cbFontName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cbFontName.FormattingEnabled = true;
        cbFontName.Location = new System.Drawing.Point(62, 0);
        cbFontName.Name = "cbFontName";
        cbFontName.Size = new System.Drawing.Size(277, 23);
        cbFontName.TabIndex = 1;
        cbFontName.SelectedIndexChanged += cbFontName_SelectedIndexChanged;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(0, 31);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(30, 15);
        label2.TabIndex = 2;
        label2.Text = "Size:";
        // 
        // tbSize
        // 
        tbSize.Location = new System.Drawing.Point(62, 29);
        tbSize.Name = "tbSize";
        tbSize.Size = new System.Drawing.Size(66, 23);
        tbSize.TabIndex = 2;
        tbSize.ValueChanged += tbSize_ValueChanged;
        // 
        // chBold
        // 
        chBold.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        chBold.AutoSize = true;
        chBold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        chBold.Location = new System.Drawing.Point(171, 30);
        chBold.Name = "chBold";
        chBold.Size = new System.Drawing.Size(51, 19);
        chBold.TabIndex = 3;
        chBold.Text = "Bold";
        chBold.UseVisualStyleBackColor = true;
        chBold.CheckedChanged += chBold_CheckedChanged;
        // 
        // chItalic
        // 
        chItalic.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        chItalic.AutoSize = true;
        chItalic.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
        chItalic.Location = new System.Drawing.Point(259, 30);
        chItalic.Name = "chItalic";
        chItalic.Size = new System.Drawing.Size(51, 19);
        chItalic.TabIndex = 4;
        chItalic.Text = "Italic";
        chItalic.UseVisualStyleBackColor = true;
        chItalic.CheckedChanged += chItalic_CheckedChanged;
        // 
        // slSize
        // 
        slSize.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        slSize.Location = new System.Drawing.Point(0, 58);
        slSize.Maximum = 36;
        slSize.Minimum = 5;
        slSize.Name = "slSize";
        slSize.Size = new System.Drawing.Size(339, 45);
        slSize.TabIndex = 5;
        slSize.Value = 5;
        slSize.Scroll += slFontSize_Scroll;
        // 
        // FontPicker
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Controls.Add(slSize);
        Controls.Add(chItalic);
        Controls.Add(chBold);
        Controls.Add(tbSize);
        Controls.Add(label2);
        Controls.Add(cbFontName);
        Controls.Add(label1);
        Name = "FontPicker";
        Size = new System.Drawing.Size(339, 101);
        ((System.ComponentModel.ISupportInitialize)tbSize).EndInit();
        ((System.ComponentModel.ISupportInitialize)slSize).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbFontName;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.NumericUpDown tbSize;
    private System.Windows.Forms.CheckBox chBold;
    private System.Windows.Forms.CheckBox chItalic;
    private System.Windows.Forms.TrackBar slSize;
}
