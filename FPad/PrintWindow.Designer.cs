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
        components = new System.ComponentModel.Container();
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
        collatePic = new System.Windows.Forms.PictureBox();
        tbNumberOfCopies = new System.Windows.Forms.NumericUpDown();
        chCollate = new System.Windows.Forms.CheckBox();
        label7 = new System.Windows.Forms.Label();
        printPreview = new System.Windows.Forms.PrintPreviewControl();
        bOk = new System.Windows.Forms.Button();
        bCancel = new System.Windows.Forms.Button();
        timer1 = new System.Windows.Forms.Timer(components);
        bPrevPage = new System.Windows.Forms.Button();
        pNextPage = new System.Windows.Forms.Button();
        label6 = new System.Windows.Forms.Label();
        groupBox4 = new System.Windows.Forms.GroupBox();
        auxPrintPreview = new System.Windows.Forms.PrintPreviewControl();
        panel1 = new System.Windows.Forms.Panel();
        tbCurrentPage = new System.Windows.Forms.TextBox();
        lOfPageCount = new System.Windows.Forms.Label();
        groupBox5 = new System.Windows.Forms.GroupBox();
        panel2 = new System.Windows.Forms.Panel();
        printSettingsEditor = new FPad.Controls.PrintSettingsEditor();
        groupBox1.SuspendLayout();
        groupBox2.SuspendLayout();
        groupBox3.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)collatePic).BeginInit();
        ((System.ComponentModel.ISupportInitialize)tbNumberOfCopies).BeginInit();
        groupBox4.SuspendLayout();
        panel1.SuspendLayout();
        groupBox5.SuspendLayout();
        panel2.SuspendLayout();
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
        groupBox1.Size = new System.Drawing.Size(353, 183);
        groupBox1.TabIndex = 0;
        groupBox1.TabStop = false;
        groupBox1.Text = "Printer";
        // 
        // chPrintToFile
        // 
        chPrintToFile.AutoSize = true;
        chPrintToFile.Location = new System.Drawing.Point(175, 54);
        chPrintToFile.Name = "chPrintToFile";
        chPrintToFile.Size = new System.Drawing.Size(86, 19);
        chPrintToFile.TabIndex = 11;
        chPrintToFile.Text = "Print to File";
        chPrintToFile.UseVisualStyleBackColor = true;
        chPrintToFile.Visible = false;
        // 
        // bPrinterProperties
        // 
        bPrinterProperties.Location = new System.Drawing.Point(6, 46);
        bPrinterProperties.Name = "bPrinterProperties";
        bPrinterProperties.Size = new System.Drawing.Size(157, 32);
        bPrinterProperties.TabIndex = 10;
        bPrinterProperties.Text = "Properties";
        bPrinterProperties.UseVisualStyleBackColor = true;
        bPrinterProperties.Click += bPrinterProperties_Click;
        // 
        // lComment
        // 
        lComment.AutoSize = true;
        lComment.Location = new System.Drawing.Point(80, 155);
        lComment.Name = "lComment";
        lComment.Size = new System.Drawing.Size(16, 15);
        lComment.TabIndex = 9;
        lComment.Text = "...";
        // 
        // lWhere
        // 
        lWhere.AutoSize = true;
        lWhere.Location = new System.Drawing.Point(80, 133);
        lWhere.Name = "lWhere";
        lWhere.Size = new System.Drawing.Size(16, 15);
        lWhere.TabIndex = 8;
        lWhere.Text = "...";
        // 
        // lType
        // 
        lType.AutoSize = true;
        lType.Location = new System.Drawing.Point(80, 111);
        lType.Name = "lType";
        lType.Size = new System.Drawing.Size(16, 15);
        lType.TabIndex = 7;
        lType.Text = "...";
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Location = new System.Drawing.Point(6, 155);
        label5.Name = "label5";
        label5.Size = new System.Drawing.Size(64, 15);
        label5.TabIndex = 6;
        label5.Text = "Comment:";
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new System.Drawing.Point(6, 133);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(44, 15);
        label4.TabIndex = 5;
        label4.Text = "Where:";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new System.Drawing.Point(6, 111);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(35, 15);
        label3.TabIndex = 4;
        label3.Text = "Type:";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(6, 89);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(42, 15);
        label2.TabIndex = 3;
        label2.Text = "Status:";
        // 
        // lStatus
        // 
        lStatus.AutoSize = true;
        lStatus.Location = new System.Drawing.Point(80, 89);
        lStatus.Name = "lStatus";
        lStatus.Size = new System.Drawing.Size(16, 15);
        lStatus.TabIndex = 2;
        lStatus.Text = "...";
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
        groupBox2.Location = new System.Drawing.Point(6, 195);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new System.Drawing.Size(163, 137);
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
        groupBox3.Controls.Add(collatePic);
        groupBox3.Controls.Add(tbNumberOfCopies);
        groupBox3.Controls.Add(chCollate);
        groupBox3.Controls.Add(label7);
        groupBox3.Location = new System.Drawing.Point(175, 195);
        groupBox3.Name = "groupBox3";
        groupBox3.Size = new System.Drawing.Size(184, 137);
        groupBox3.TabIndex = 2;
        groupBox3.TabStop = false;
        groupBox3.Text = "Copies";
        // 
        // collatePic
        // 
        collatePic.Location = new System.Drawing.Point(6, 72);
        collatePic.Name = "collatePic";
        collatePic.Size = new System.Drawing.Size(172, 58);
        collatePic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        collatePic.TabIndex = 4;
        collatePic.TabStop = false;
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
        chCollate.CheckedChanged += chCollate_CheckedChanged;
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
        printPreview.Location = new System.Drawing.Point(6, 55);
        printPreview.Name = "printPreview";
        printPreview.Size = new System.Drawing.Size(487, 568);
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
        // timer1
        // 
        timer1.Interval = 2000;
        timer1.Tick += timer1_Tick;
        // 
        // bPrevPage
        // 
        bPrevPage.Location = new System.Drawing.Point(0, 0);
        bPrevPage.Name = "bPrevPage";
        bPrevPage.Size = new System.Drawing.Size(75, 32);
        bPrevPage.TabIndex = 6;
        bPrevPage.Text = "<< Prev";
        bPrevPage.UseVisualStyleBackColor = true;
        bPrevPage.Click += bPrevPage_Click;
        // 
        // pNextPage
        // 
        pNextPage.Location = new System.Drawing.Point(236, 0);
        pNextPage.Name = "pNextPage";
        pNextPage.Size = new System.Drawing.Size(75, 32);
        pNextPage.TabIndex = 7;
        pNextPage.Text = "Next >>";
        pNextPage.UseVisualStyleBackColor = true;
        pNextPage.Click += pNextPage_Click;
        // 
        // label6
        // 
        label6.AutoSize = true;
        label6.Location = new System.Drawing.Point(81, 9);
        label6.Name = "label6";
        label6.Size = new System.Drawing.Size(36, 15);
        label6.TabIndex = 8;
        label6.Text = "Page:";
        // 
        // groupBox4
        // 
        groupBox4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        groupBox4.Controls.Add(auxPrintPreview);
        groupBox4.Controls.Add(panel1);
        groupBox4.Controls.Add(printPreview);
        groupBox4.Location = new System.Drawing.Point(365, 6);
        groupBox4.Name = "groupBox4";
        groupBox4.Size = new System.Drawing.Size(499, 629);
        groupBox4.TabIndex = 9;
        groupBox4.TabStop = false;
        groupBox4.Text = "Preview";
        // 
        // auxPrintPreview
        // 
        auxPrintPreview.Location = new System.Drawing.Point(323, 17);
        auxPrintPreview.Name = "auxPrintPreview";
        auxPrintPreview.Size = new System.Drawing.Size(100, 100);
        auxPrintPreview.TabIndex = 10;
        auxPrintPreview.Visible = false;
        // 
        // panel1
        // 
        panel1.Controls.Add(tbCurrentPage);
        panel1.Controls.Add(bPrevPage);
        panel1.Controls.Add(label6);
        panel1.Controls.Add(pNextPage);
        panel1.Controls.Add(lOfPageCount);
        panel1.Location = new System.Drawing.Point(6, 17);
        panel1.Name = "panel1";
        panel1.Size = new System.Drawing.Size(311, 32);
        panel1.TabIndex = 9;
        // 
        // tbCurrentPage
        // 
        tbCurrentPage.Location = new System.Drawing.Point(135, 6);
        tbCurrentPage.Name = "tbCurrentPage";
        tbCurrentPage.Size = new System.Drawing.Size(54, 23);
        tbCurrentPage.TabIndex = 9;
        // 
        // lOfPageCount
        // 
        lOfPageCount.AutoSize = true;
        lOfPageCount.Location = new System.Drawing.Point(195, 9);
        lOfPageCount.Name = "lOfPageCount";
        lOfPageCount.Size = new System.Drawing.Size(16, 15);
        lOfPageCount.TabIndex = 10;
        lOfPageCount.Text = "...";
        // 
        // groupBox5
        // 
        groupBox5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        groupBox5.Controls.Add(panel2);
        groupBox5.Location = new System.Drawing.Point(6, 338);
        groupBox5.Name = "groupBox5";
        groupBox5.Size = new System.Drawing.Size(353, 335);
        groupBox5.TabIndex = 10;
        groupBox5.TabStop = false;
        groupBox5.Text = "Print Settings";
        // 
        // panel2
        // 
        panel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        panel2.AutoScroll = true;
        panel2.Controls.Add(printSettingsEditor);
        panel2.Location = new System.Drawing.Point(6, 22);
        panel2.Name = "panel2";
        panel2.Size = new System.Drawing.Size(341, 307);
        panel2.TabIndex = 0;
        // 
        // printSettingsEditor
        // 
        printSettingsEditor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        printSettingsEditor.ImmediatePageNumberTemplateChange = true;
        printSettingsEditor.Location = new System.Drawing.Point(0, 0);
        printSettingsEditor.Margin = new System.Windows.Forms.Padding(0);
        printSettingsEditor.Name = "printSettingsEditor";
        printSettingsEditor.Size = new System.Drawing.Size(341, 44);
        printSettingsEditor.TabIndex = 0;
        printSettingsEditor.Changed += printSettingsEditor_Changed;
        // 
        // PrintWindow
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        CancelButton = bCancel;
        ClientSize = new System.Drawing.Size(870, 680);
        Controls.Add(groupBox5);
        Controls.Add(groupBox4);
        Controls.Add(bCancel);
        Controls.Add(bOk);
        Controls.Add(groupBox3);
        Controls.Add(groupBox2);
        Controls.Add(groupBox1);
        KeyPreview = true;
        MinimizeBox = false;
        Name = "PrintWindow";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Activated += PrintWindow_Activated;
        FormClosing += PrintWindow_FormClosing;
        KeyDown += PrintWindow_KeyDown;
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
        groupBox3.ResumeLayout(false);
        groupBox3.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)collatePic).EndInit();
        ((System.ComponentModel.ISupportInitialize)tbNumberOfCopies).EndInit();
        groupBox4.ResumeLayout(false);
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        groupBox5.ResumeLayout(false);
        panel2.ResumeLayout(false);
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
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.Button bPrevPage;
    private System.Windows.Forms.Button pNextPage;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label lOfPageCount;
    private System.Windows.Forms.TextBox tbCurrentPage;
    private System.Windows.Forms.PictureBox collatePic;
    private System.Windows.Forms.GroupBox groupBox5;
    private System.Windows.Forms.Panel panel2;
    private Controls.PrintSettingsEditor printSettingsEditor;
    private System.Windows.Forms.PrintPreviewControl auxPrintPreview;
}