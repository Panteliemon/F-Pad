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
        components = new System.ComponentModel.Container();
        chFileName = new System.Windows.Forms.CheckBox();
        gbFileName = new System.Windows.Forms.GroupBox();
        groupBox1 = new System.Windows.Forms.GroupBox();
        fontPickerFileName = new FontPicker();
        rbFnFullPath = new System.Windows.Forms.RadioButton();
        rbFnNameExt = new System.Windows.Forms.RadioButton();
        rbFnName = new System.Windows.Forms.RadioButton();
        chPageNumber = new System.Windows.Forms.CheckBox();
        gbPageNumber = new System.Windows.Forms.GroupBox();
        groupBox3 = new System.Windows.Forms.GroupBox();
        fontPickerPageNumber = new FontPicker();
        groupBox2 = new System.Windows.Forms.GroupBox();
        rbPnARight = new System.Windows.Forms.RadioButton();
        rbPnACenter = new System.Windows.Forms.RadioButton();
        rbPnALeft = new System.Windows.Forms.RadioButton();
        tbPageNumberTemplate = new System.Windows.Forms.TextBox();
        rbPnTemplate = new System.Windows.Forms.RadioButton();
        rbPnStandard = new System.Windows.Forms.RadioButton();
        toolTip1 = new System.Windows.Forms.ToolTip(components);
        gbFileName.SuspendLayout();
        groupBox1.SuspendLayout();
        gbPageNumber.SuspendLayout();
        groupBox3.SuspendLayout();
        groupBox2.SuspendLayout();
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
        groupBox1.Controls.Add(fontPickerFileName);
        groupBox1.Location = new System.Drawing.Point(6, 72);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new System.Drawing.Size(418, 132);
        groupBox1.TabIndex = 3;
        groupBox1.TabStop = false;
        groupBox1.Text = "Font";
        // 
        // fontPickerFileName
        // 
        fontPickerFileName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        fontPickerFileName.Location = new System.Drawing.Point(6, 22);
        fontPickerFileName.Name = "fontPickerFileName";
        fontPickerFileName.Size = new System.Drawing.Size(406, 103);
        fontPickerFileName.TabIndex = 0;
        fontPickerFileName.Changed += fontPickerFileName_Changed;
        // 
        // rbFnFullPath
        // 
        rbFnFullPath.AutoSize = true;
        rbFnFullPath.Location = new System.Drawing.Point(186, 22);
        rbFnFullPath.Name = "rbFnFullPath";
        rbFnFullPath.Size = new System.Drawing.Size(71, 19);
        rbFnFullPath.TabIndex = 2;
        rbFnFullPath.Text = "Full Path";
        rbFnFullPath.UseVisualStyleBackColor = true;
        rbFnFullPath.CheckedChanged += rbFnOption_CheckedChanged;
        // 
        // rbFnNameExt
        // 
        rbFnNameExt.AutoSize = true;
        rbFnNameExt.Location = new System.Drawing.Point(6, 47);
        rbFnNameExt.Name = "rbFnNameExt";
        rbFnNameExt.Size = new System.Drawing.Size(121, 19);
        rbFnNameExt.TabIndex = 1;
        rbFnNameExt.Text = "Name + Extension";
        rbFnNameExt.UseVisualStyleBackColor = true;
        rbFnNameExt.CheckedChanged += rbFnOption_CheckedChanged;
        // 
        // rbFnName
        // 
        rbFnName.AutoSize = true;
        rbFnName.Location = new System.Drawing.Point(6, 22);
        rbFnName.Name = "rbFnName";
        rbFnName.Size = new System.Drawing.Size(57, 19);
        rbFnName.TabIndex = 0;
        rbFnName.Text = "Name";
        rbFnName.UseVisualStyleBackColor = true;
        rbFnName.CheckedChanged += rbFnOption_CheckedChanged;
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
        // groupBox3
        // 
        groupBox3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        groupBox3.Controls.Add(fontPickerPageNumber);
        groupBox3.Location = new System.Drawing.Point(6, 129);
        groupBox3.Name = "groupBox3";
        groupBox3.Size = new System.Drawing.Size(418, 133);
        groupBox3.TabIndex = 4;
        groupBox3.TabStop = false;
        groupBox3.Text = "Font";
        // 
        // fontPickerPageNumber
        // 
        fontPickerPageNumber.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        fontPickerPageNumber.Location = new System.Drawing.Point(6, 22);
        fontPickerPageNumber.Name = "fontPickerPageNumber";
        fontPickerPageNumber.Size = new System.Drawing.Size(406, 103);
        fontPickerPageNumber.TabIndex = 0;
        fontPickerPageNumber.Changed += fontPickerPageNumber_Changed;
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
        rbPnARight.CheckedChanged += rbPnAlign_CheckedChanged;
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
        rbPnACenter.CheckedChanged += rbPnAlign_CheckedChanged;
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
        rbPnALeft.CheckedChanged += rbPnAlign_CheckedChanged;
        // 
        // tbPageNumberTemplate
        // 
        tbPageNumberTemplate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        tbPageNumberTemplate.Location = new System.Drawing.Point(108, 46);
        tbPageNumberTemplate.Name = "tbPageNumberTemplate";
        tbPageNumberTemplate.Size = new System.Drawing.Size(316, 23);
        tbPageNumberTemplate.TabIndex = 2;
        toolTip1.SetToolTip(tbPageNumberTemplate, "Use {page} placeholder for page number, and {total} for total number of pages.");
        tbPageNumberTemplate.TextChanged += tbPageNumberTemplate_TextChanged;
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
        rbPnTemplate.CheckedChanged += rbPnOption_CheckedChanged;
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
        rbPnStandard.CheckedChanged += rbPnOption_CheckedChanged;
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
        groupBox3.ResumeLayout(false);
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
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
    private FontPicker fontPickerFileName;
    private System.Windows.Forms.RadioButton rbPnStandard;
    private System.Windows.Forms.TextBox tbPageNumberTemplate;
    private System.Windows.Forms.RadioButton rbPnTemplate;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.RadioButton rbPnARight;
    private System.Windows.Forms.RadioButton rbPnACenter;
    private System.Windows.Forms.RadioButton rbPnALeft;
    private System.Windows.Forms.GroupBox groupBox3;
    private FontPicker fontPickerPageNumber;
    private System.Windows.Forms.ToolTip toolTip1;
}
