namespace FPad;

partial class PrintWindow
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
        groupBox1 = new System.Windows.Forms.GroupBox();
        chPrintToFile = new System.Windows.Forms.CheckBox();
        bPrinterProperties = new System.Windows.Forms.Button();
        lComment = new System.Windows.Forms.Label();
        lWhere = new System.Windows.Forms.Label();
        lType = new System.Windows.Forms.Label();
        label5 = new System.Windows.Forms.Label();
        label4 = new System.Windows.Forms.Label();
        label3 = new System.Windows.Forms.Label();
        label2 = new System.Windows.Forms.Label();
        lStatus = new System.Windows.Forms.Label();
        cbPrinter = new System.Windows.Forms.ComboBox();
        label1 = new System.Windows.Forms.Label();
        groupBox2 = new System.Windows.Forms.GroupBox();
        lFrom = new System.Windows.Forms.Label();
        lTo = new System.Windows.Forms.Label();
        tbTo = new System.Windows.Forms.TextBox();
        tbFrom = new System.Windows.Forms.TextBox();
        rbPageRange = new System.Windows.Forms.RadioButton();
        rbAll = new System.Windows.Forms.RadioButton();
        groupBox3 = new System.Windows.Forms.GroupBox();
        tbNumberOfCopies = new System.Windows.Forms.NumericUpDown();
        chCollate = new System.Windows.Forms.CheckBox();
        label7 = new System.Windows.Forms.Label();
        printPreview = new System.Windows.Forms.PrintPreviewControl();
        bOk = new System.Windows.Forms.Button();
        bCancel = new System.Windows.Forms.Button();
        groupBox1.SuspendLayout();
        groupBox2.SuspendLayout();
        groupBox3.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)tbNumberOfCopies).BeginInit();
        SuspendLayout();
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(chPrintToFile);
        groupBox1.Controls.Add(bPrinterProperties);
        groupBox1.Controls.Add(lComment);
        groupBox1.Controls.Add(lWhere);
        groupBox1.Controls.Add(lType);
        groupBox1.Controls.Add(label5);
        groupBox1.Controls.Add(label4);
        groupBox1.Controls.Add(label3);
        groupBox1.Controls.Add(label2);
        groupBox1.Controls.Add(lStatus);
        groupBox1.Controls.Add(cbPrinter);
        groupBox1.Controls.Add(label1);
        groupBox1.Location = new System.Drawing.Point(6, 6);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new System.Drawing.Size(353, 139);
        groupBox1.TabIndex = 0;
        groupBox1.TabStop = false;
        groupBox1.Text = "Printer";
        // 
        // chPrintToFile
        // 
        chPrintToFile.AutoSize = true;
        chPrintToFile.Location = new System.Drawing.Point(209, 88);
        chPrintToFile.Name = "chPrintToFile";
        chPrintToFile.Size = new System.Drawing.Size(86, 19);
        chPrintToFile.TabIndex = 11;
        chPrintToFile.Text = "Print to File";
        chPrintToFile.UseVisualStyleBackColor = true;
        // 
        // bPrinterProperties
        // 
        bPrinterProperties.Location = new System.Drawing.Point(209, 49);
        bPrinterProperties.Name = "bPrinterProperties";
        bPrinterProperties.Size = new System.Drawing.Size(138, 32);
        bPrinterProperties.TabIndex = 10;
        bPrinterProperties.Text = "Properties";
        bPrinterProperties.UseVisualStyleBackColor = true;
        bPrinterProperties.Click += bPrinterProperties_Click;
        // 
        // lComment
        // 
        lComment.AutoSize = true;
        lComment.Location = new System.Drawing.Point(80, 110);
        lComment.Name = "lComment";
        lComment.Size = new System.Drawing.Size(15, 15);
        lComment.TabIndex = 9;
        lComment.Text = "=";
        // 
        // lWhere
        // 
        lWhere.AutoSize = true;
        lWhere.Location = new System.Drawing.Point(80, 88);
        lWhere.Name = "lWhere";
        lWhere.Size = new System.Drawing.Size(15, 15);
        lWhere.TabIndex = 8;
        lWhere.Text = "=";
        // 
        // lType
        // 
        lType.AutoSize = true;
        lType.Location = new System.Drawing.Point(80, 66);
        lType.Name = "lType";
        lType.Size = new System.Drawing.Size(15, 15);
        lType.TabIndex = 7;
        lType.Text = "=";
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Location = new System.Drawing.Point(6, 110);
        label5.Name = "label5";
        label5.Size = new System.Drawing.Size(64, 15);
        label5.TabIndex = 6;
        label5.Text = "Comment:";
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new System.Drawing.Point(6, 88);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(44, 15);
        label4.TabIndex = 5;
        label4.Text = "Where:";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new System.Drawing.Point(6, 66);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(35, 15);
        label3.TabIndex = 4;
        label3.Text = "Type:";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(6, 44);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(42, 15);
        label2.TabIndex = 3;
        label2.Text = "Status:";
        // 
        // lStatus
        // 
        lStatus.AutoSize = true;
        lStatus.Location = new System.Drawing.Point(80, 44);
        lStatus.Name = "lStatus";
        lStatus.Size = new System.Drawing.Size(15, 15);
        lStatus.TabIndex = 2;
        lStatus.Text = "=";
        // 
        // cbPrinter
        // 
        cbPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cbPrinter.FormattingEnabled = true;
        cbPrinter.Location = new System.Drawing.Point(80, 17);
        cbPrinter.Name = "cbPrinter";
        cbPrinter.Size = new System.Drawing.Size(267, 23);
        cbPrinter.TabIndex = 1;
        cbPrinter.SelectedIndexChanged += cbPrinter_SelectedIndexChanged;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(6, 20);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(42, 15);
        label1.TabIndex = 0;
        label1.Text = "Name:";
        // 
        // groupBox2
        // 
        groupBox2.Controls.Add(lFrom);
        groupBox2.Controls.Add(lTo);
        groupBox2.Controls.Add(tbTo);
        groupBox2.Controls.Add(tbFrom);
        groupBox2.Controls.Add(rbPageRange);
        groupBox2.Controls.Add(rbAll);
        groupBox2.Location = new System.Drawing.Point(6, 151);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new System.Drawing.Size(163, 119);
        groupBox2.TabIndex = 1;
        groupBox2.TabStop = false;
        groupBox2.Text = "Print Range";
        // 
        // lFrom
        // 
        lFrom.AutoSize = true;
        lFrom.Location = new System.Drawing.Point(3, 75);
        lFrom.Name = "lFrom";
        lFrom.Size = new System.Drawing.Size(35, 15);
        lFrom.TabIndex = 5;
        lFrom.Text = "From";
        // 
        // lTo
        // 
        lTo.AutoSize = true;
        lTo.Location = new System.Drawing.Point(90, 75);
        lTo.Name = "lTo";
        lTo.Size = new System.Drawing.Size(18, 15);
        lTo.TabIndex = 4;
        lTo.Text = "to";
        // 
        // tbTo
        // 
        tbTo.Location = new System.Drawing.Point(114, 72);
        tbTo.Name = "tbTo";
        tbTo.Size = new System.Drawing.Size(43, 23);
        tbTo.TabIndex = 3;
        // 
        // tbFrom
        // 
        tbFrom.Location = new System.Drawing.Point(41, 72);
        tbFrom.Name = "tbFrom";
        tbFrom.Size = new System.Drawing.Size(43, 23);
        tbFrom.TabIndex = 2;
        // 
        // rbPageRange
        // 
        rbPageRange.AutoSize = true;
        rbPageRange.Location = new System.Drawing.Point(6, 47);
        rbPageRange.Name = "rbPageRange";
        rbPageRange.Size = new System.Drawing.Size(56, 19);
        rbPageRange.TabIndex = 1;
        rbPageRange.TabStop = true;
        rbPageRange.Text = "Pages";
        rbPageRange.UseVisualStyleBackColor = true;
        rbPageRange.CheckedChanged += rbPageRange_CheckedChanged;
        // 
        // rbAll
        // 
        rbAll.AutoSize = true;
        rbAll.Location = new System.Drawing.Point(6, 22);
        rbAll.Name = "rbAll";
        rbAll.Size = new System.Drawing.Size(39, 19);
        rbAll.TabIndex = 0;
        rbAll.TabStop = true;
        rbAll.Text = "All";
        rbAll.UseVisualStyleBackColor = true;
        rbAll.CheckedChanged += rbAll_CheckedChanged;
        // 
        // groupBox3
        // 
        groupBox3.Controls.Add(tbNumberOfCopies);
        groupBox3.Controls.Add(chCollate);
        groupBox3.Controls.Add(label7);
        groupBox3.Location = new System.Drawing.Point(175, 151);
        groupBox3.Name = "groupBox3";
        groupBox3.Size = new System.Drawing.Size(184, 119);
        groupBox3.TabIndex = 2;
        groupBox3.TabStop = false;
        groupBox3.Text = "Copies";
        // 
        // tbNumberOfCopies
        // 
        tbNumberOfCopies.Location = new System.Drawing.Point(117, 22);
        tbNumberOfCopies.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        tbNumberOfCopies.Name = "tbNumberOfCopies";
        tbNumberOfCopies.Size = new System.Drawing.Size(61, 23);
        tbNumberOfCopies.TabIndex = 3;
        tbNumberOfCopies.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // chCollate
        // 
        chCollate.AutoSize = true;
        chCollate.Location = new System.Drawing.Point(6, 47);
        chCollate.Name = "chCollate";
        chCollate.Size = new System.Drawing.Size(63, 19);
        chCollate.TabIndex = 2;
        chCollate.Text = "Collate";
        chCollate.UseVisualStyleBackColor = true;
        // 
        // label7
        // 
        label7.AutoSize = true;
        label7.Location = new System.Drawing.Point(6, 24);
        label7.Name = "label7";
        label7.Size = new System.Drawing.Size(105, 15);
        label7.TabIndex = 0;
        label7.Text = "Number of copies:";
        // 
        // printPreview
        // 
        printPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        printPreview.Location = new System.Drawing.Point(365, 13);
        printPreview.Name = "printPreview";
        printPreview.Size = new System.Drawing.Size(499, 622);
        printPreview.TabIndex = 3;
        // 
        // bOk
        // 
        bOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        bOk.Location = new System.Drawing.Point(634, 641);
        bOk.Name = "bOk";
        bOk.Size = new System.Drawing.Size(112, 32);
        bOk.TabIndex = 4;
        bOk.Text = "Print";
        bOk.UseVisualStyleBackColor = true;
        bOk.Click += bOk_Click;
        // 
        // bCancel
        // 
        bCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        bCancel.Location = new System.Drawing.Point(752, 641);
        bCancel.Name = "bCancel";
        bCancel.Size = new System.Drawing.Size(112, 32);
        bCancel.TabIndex = 5;
        bCancel.Text = "Cancel";
        bCancel.UseVisualStyleBackColor = true;
        bCancel.Click += bCancel_Click;
        // 
        // PrintWindow
        // 
        AcceptButton = bOk;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        CancelButton = bCancel;
        ClientSize = new System.Drawing.Size(870, 680);
        Controls.Add(bCancel);
        Controls.Add(bOk);
        Controls.Add(printPreview);
        Controls.Add(groupBox3);
        Controls.Add(groupBox2);
        Controls.Add(groupBox1);
        MinimizeBox = false;
        Name = "PrintWindow";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
        groupBox3.ResumeLayout(false);
        groupBox3.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)tbNumberOfCopies).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.ComboBox cbPrinter;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label lComment;
    private System.Windows.Forms.Label lWhere;
    private System.Windows.Forms.Label lType;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label lStatus;
    private System.Windows.Forms.CheckBox chPrintToFile;
    private System.Windows.Forms.Button bPrinterProperties;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.RadioButton rbAll;
    private System.Windows.Forms.RadioButton rbPageRange;
    private System.Windows.Forms.Label lTo;
    private System.Windows.Forms.TextBox tbTo;
    private System.Windows.Forms.TextBox tbFrom;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.CheckBox chCollate;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label lFrom;
    private System.Windows.Forms.PrintPreviewControl printPreview;
    private System.Windows.Forms.NumericUpDown tbNumberOfCopies;
    private System.Windows.Forms.Button bOk;
    private System.Windows.Forms.Button bCancel;
}