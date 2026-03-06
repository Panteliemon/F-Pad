namespace FPad.Controls;

partial class PrintSettingsEditor
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
        chFileName = new System.Windows.Forms.CheckBox();
        gbFileName = new System.Windows.Forms.GroupBox();
        groupBox1 = new System.Windows.Forms.GroupBox();
        rbFnFullPath = new System.Windows.Forms.RadioButton();
        rbFnNameExt = new System.Windows.Forms.RadioButton();
        rbFnName = new System.Windows.Forms.RadioButton();
        chPageNumber = new System.Windows.Forms.CheckBox();
        gbPageNumber = new System.Windows.Forms.GroupBox();
        fontPicker1 = new FontPicker();
        rbPnStandard = new System.Windows.Forms.RadioButton();
        rbPnTemplate = new System.Windows.Forms.RadioButton();
        tbPageNumberTemplate = new System.Windows.Forms.TextBox();
        groupBox2 = new System.Windows.Forms.GroupBox();
        rbPnALeft = new System.Windows.Forms.RadioButton();
        rbPnACenter = new System.Windows.Forms.RadioButton();
        rbPnARight = new System.Windows.Forms.RadioButton();
        groupBox3 = new System.Windows.Forms.GroupBox();
        fontPicker2 = new FontPicker();
        gbFileName.SuspendLayout();
        groupBox1.SuspendLayout();
        gbPageNumber.SuspendLayout();
        groupBox2.SuspendLayout();
        groupBox3.SuspendLayout();
        SuspendLayout();
        // 
        // chFileName
        // 
        chFileName.AutoSize = true;
        chFileName.Location = new System.Drawing.Point(0, 0);
        chFileName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
        chFileName.Name = "chFileName";
        chFileName.Size = new System.Drawing.Size(121, 19);
        chFileName.TabIndex = 0;
        chFileName.Text = "Include File Name";
        chFileName.UseVisualStyleBackColor = true;
        chFileName.CheckedChanged += chFileName_CheckedChanged;
        // 
        // gbFileName
        // 
        gbFileName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        gbFileName.Controls.Add(groupBox1);
        gbFileName.Controls.Add(rbFnFullPath);
        gbFileName.Controls.Add(rbFnNameExt);
        gbFileName.Controls.Add(rbFnName);
        gbFileName.Location = new System.Drawing.Point(0, 19);
        gbFileName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
        gbFileName.Name = "gbFileName";
        gbFileName.Size = new System.Drawing.Size(430, 211);
        gbFileName.TabIndex = 1;
        gbFileName.TabStop = false;
        gbFileName.Text = "File Name Options";
        // 
        // groupBox1
        // 
        groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        groupBox1.Controls.Add(fontPicker1);
        groupBox1.Location = new System.Drawing.Point(6, 72);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new System.Drawing.Size(418, 132);
        groupBox1.TabIndex = 3;
        groupBox1.TabStop = false;
        groupBox1.Text = "Font";
        // 
        // rbFnFullPath
        // 
        rbFnFullPath.AutoSize = true;
        rbFnFullPath.Location = new System.Drawing.Point(186, 22);
        rbFnFullPath.Name = "rbFnFullPath";
        rbFnFullPath.Size = new System.Drawing.Size(71, 19);
        rbFnFullPath.TabIndex = 2;
        rbFnFullPath.TabStop = true;
        rbFnFullPath.Text = "Full Path";
        rbFnFullPath.UseVisualStyleBackColor = true;
        // 
        // rbFnNameExt
        // 
        rbFnNameExt.AutoSize = true;
        rbFnNameExt.Location = new System.Drawing.Point(6, 47);
        rbFnNameExt.Name = "rbFnNameExt";
        rbFnNameExt.Size = new System.Drawing.Size(121, 19);
        rbFnNameExt.TabIndex = 1;
        rbFnNameExt.TabStop = true;
        rbFnNameExt.Text = "Name + Extension";
        rbFnNameExt.UseVisualStyleBackColor = true;
        // 
        // rbFnName
        // 
        rbFnName.AutoSize = true;
        rbFnName.Location = new System.Drawing.Point(6, 22);
        rbFnName.Name = "rbFnName";
        rbFnName.Size = new System.Drawing.Size(57, 19);
        rbFnName.TabIndex = 0;
        rbFnName.TabStop = true;
        rbFnName.Text = "Name";
        rbFnName.UseVisualStyleBackColor = true;
        // 
        // chPageNumber
        // 
        chPageNumber.AutoSize = true;
        chPageNumber.Location = new System.Drawing.Point(0, 236);
        chPageNumber.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
        chPageNumber.Name = "chPageNumber";
        chPageNumber.Size = new System.Drawing.Size(141, 19);
        chPageNumber.TabIndex = 2;
        chPageNumber.Text = "Include Page Number";
        chPageNumber.UseVisualStyleBackColor = true;
        chPageNumber.CheckedChanged += chPageNumber_CheckedChanged;
        // 
        // gbPageNumber
        // 
        gbPageNumber.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        gbPageNumber.Controls.Add(groupBox3);
        gbPageNumber.Controls.Add(groupBox2);
        gbPageNumber.Controls.Add(tbPageNumberTemplate);
        gbPageNumber.Controls.Add(rbPnTemplate);
        gbPageNumber.Controls.Add(rbPnStandard);
        gbPageNumber.Location = new System.Drawing.Point(0, 255);
        gbPageNumber.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
        gbPageNumber.Name = "gbPageNumber";
        gbPageNumber.Size = new System.Drawing.Size(430, 268);
        gbPageNumber.TabIndex = 3;
        gbPageNumber.TabStop = false;
        gbPageNumber.Text = "Page Number Options";
        // 
        // fontPicker1
        // 
        fontPicker1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        fontPicker1.Location = new System.Drawing.Point(6, 22);
        fontPicker1.Name = "fontPicker1";
        fontPicker1.Size = new System.Drawing.Size(406, 103);
        fontPicker1.TabIndex = 0;
        // 
        // rbPnStandard
        // 
        rbPnStandard.AutoSize = true;
        rbPnStandard.Location = new System.Drawing.Point(6, 22);
        rbPnStandard.Name = "rbPnStandard";
        rbPnStandard.Size = new System.Drawing.Size(72, 19);
        rbPnStandard.TabIndex = 0;
        rbPnStandard.TabStop = true;
        rbPnStandard.Text = "Standard";
        rbPnStandard.UseVisualStyleBackColor = true;
        // 
        // rbPnTemplate
        // 
        rbPnTemplate.AutoSize = true;
        rbPnTemplate.Location = new System.Drawing.Point(6, 47);
        rbPnTemplate.Name = "rbPnTemplate";
        rbPnTemplate.Size = new System.Drawing.Size(77, 19);
        rbPnTemplate.TabIndex = 1;
        rbPnTemplate.TabStop = true;
        rbPnTemplate.Text = "Template:";
        rbPnTemplate.UseVisualStyleBackColor = true;
        // 
        // tbPageNumberTemplate
        // 
        tbPageNumberTemplate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        tbPageNumberTemplate.Location = new System.Drawing.Point(108, 46);
        tbPageNumberTemplate.Name = "tbPageNumberTemplate";
        tbPageNumberTemplate.Size = new System.Drawing.Size(316, 23);
        tbPageNumberTemplate.TabIndex = 2;
        // 
        // groupBox2
        // 
        groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        groupBox2.Controls.Add(rbPnARight);
        groupBox2.Controls.Add(rbPnACenter);
        groupBox2.Controls.Add(rbPnALeft);
        groupBox2.Location = new System.Drawing.Point(6, 75);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new System.Drawing.Size(418, 48);
        groupBox2.TabIndex = 3;
        groupBox2.TabStop = false;
        groupBox2.Text = "Align";
        // 
        // rbPnALeft
        // 
        rbPnALeft.AutoSize = true;
        rbPnALeft.Location = new System.Drawing.Point(6, 22);
        rbPnALeft.Name = "rbPnALeft";
        rbPnALeft.Size = new System.Drawing.Size(45, 19);
        rbPnALeft.TabIndex = 0;
        rbPnALeft.TabStop = true;
        rbPnALeft.Text = "Left";
        rbPnALeft.UseVisualStyleBackColor = true;
        // 
        // rbPnACenter
        // 
        rbPnACenter.AutoSize = true;
        rbPnACenter.Location = new System.Drawing.Point(125, 22);
        rbPnACenter.Name = "rbPnACenter";
        rbPnACenter.Size = new System.Drawing.Size(60, 19);
        rbPnACenter.TabIndex = 1;
        rbPnACenter.TabStop = true;
        rbPnACenter.Text = "Center";
        rbPnACenter.UseVisualStyleBackColor = true;
        // 
        // rbPnARight
        // 
        rbPnARight.AutoSize = true;
        rbPnARight.Location = new System.Drawing.Point(242, 22);
        rbPnARight.Name = "rbPnARight";
        rbPnARight.Size = new System.Drawing.Size(53, 19);
        rbPnARight.TabIndex = 2;
        rbPnARight.TabStop = true;
        rbPnARight.Text = "Right";
        rbPnARight.UseVisualStyleBackColor = true;
        // 
        // groupBox3
        // 
        groupBox3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        groupBox3.Controls.Add(fontPicker2);
        groupBox3.Location = new System.Drawing.Point(6, 129);
        groupBox3.Name = "groupBox3";
        groupBox3.Size = new System.Drawing.Size(418, 133);
        groupBox3.TabIndex = 4;
        groupBox3.TabStop = false;
        groupBox3.Text = "Font";
        // 
        // fontPicker2
        // 
        fontPicker2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        fontPicker2.Location = new System.Drawing.Point(6, 22);
        fontPicker2.Name = "fontPicker2";
        fontPicker2.Size = new System.Drawing.Size(406, 103);
        fontPicker2.TabIndex = 0;
        // 
        // PrintSettingsEditor
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Controls.Add(gbPageNumber);
        Controls.Add(chPageNumber);
        Controls.Add(gbFileName);
        Controls.Add(chFileName);
        Name = "PrintSettingsEditor";
        Size = new System.Drawing.Size(430, 526);
        gbFileName.ResumeLayout(false);
        gbFileName.PerformLayout();
        groupBox1.ResumeLayout(false);
        gbPageNumber.ResumeLayout(false);
        gbPageNumber.PerformLayout();
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
        groupBox3.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.CheckBox chFileName;
    private System.Windows.Forms.GroupBox gbFileName;
    private System.Windows.Forms.CheckBox chPageNumber;
    private System.Windows.Forms.GroupBox gbPageNumber;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RadioButton rbFnFullPath;
    private System.Windows.Forms.RadioButton rbFnNameExt;
    private System.Windows.Forms.RadioButton rbFnName;
    private FontPicker fontPicker1;
    private System.Windows.Forms.RadioButton rbPnStandard;
    private System.Windows.Forms.TextBox tbPageNumberTemplate;
    private System.Windows.Forms.RadioButton rbPnTemplate;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.RadioButton rbPnARight;
    private System.Windows.Forms.RadioButton rbPnACenter;
    private System.Windows.Forms.RadioButton rbPnALeft;
    private System.Windows.Forms.GroupBox groupBox3;
    private FontPicker fontPicker2;
}
