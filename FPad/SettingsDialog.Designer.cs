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
        chWrap = new System.Windows.Forms.CheckBox();
        label2 = new System.Windows.Forms.Label();
        exampleText = new System.Windows.Forms.TextBox();
        bSave = new System.Windows.Forms.Button();
        bCancel = new System.Windows.Forms.Button();
        chAutoReload = new System.Windows.Forms.CheckBox();
        toolTip = new System.Windows.Forms.ToolTip(components);
        label4 = new System.Windows.Forms.Label();
        cbEncodings = new System.Windows.Forms.ComboBox();
        label5 = new System.Windows.Forms.Label();
        tabControl1 = new System.Windows.Forms.TabControl();
        tabPage1 = new System.Windows.Forms.TabPage();
        bAssociateCurrentUser = new System.Windows.Forms.Button();
        bAssociateAllUsers = new System.Windows.Forms.Button();
        tabPage2 = new System.Windows.Forms.TabPage();
        fontPickerMain = new FPad.Controls.FontPicker();
        tabPage3 = new System.Windows.Forms.TabPage();
        gbPrintSettingsPreview = new System.Windows.Forms.GroupBox();
        lPreviewPageNumber = new System.Windows.Forms.Label();
        lPreviewFileName = new System.Windows.Forms.Label();
        pPrintSettingsContainer = new System.Windows.Forms.Panel();
        printSettingsEditor = new FPad.Controls.PrintSettingsEditor();
        tabControl1.SuspendLayout();
        tabPage1.SuspendLayout();
        tabPage2.SuspendLayout();
        tabPage3.SuspendLayout();
        gbPrintSettingsPreview.SuspendLayout();
        pPrintSettingsContainer.SuspendLayout();
        SuspendLayout();
        // 
        // chWrap
        // 
        chWrap.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        chWrap.AutoSize = true;
        chWrap.Location = new System.Drawing.Point(6, 322);
        chWrap.Name = "chWrap";
        chWrap.Size = new System.Drawing.Size(202, 19);
        chWrap.TabIndex = 7;
        chWrap.Text = "Wrap Lines (for this preview only)";
        chWrap.UseVisualStyleBackColor = true;
        chWrap.CheckedChanged += chWrap_CheckedChanged;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(6, 112);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(51, 15);
        label2.TabIndex = 3;
        label2.Text = "Preview:";
        // 
        // exampleText
        // 
        exampleText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        exampleText.BackColor = System.Drawing.SystemColors.Window;
        exampleText.Location = new System.Drawing.Point(6, 130);
        exampleText.Multiline = true;
        exampleText.Name = "exampleText";
        exampleText.ReadOnly = true;
        exampleText.Size = new System.Drawing.Size(617, 186);
        exampleText.TabIndex = 6;
        // 
        // bSave
        // 
        bSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        bSave.Location = new System.Drawing.Point(445, 484);
        bSave.Name = "bSave";
        bSave.Size = new System.Drawing.Size(96, 32);
        bSave.TabIndex = 10;
        bSave.Text = "OK";
        bSave.UseVisualStyleBackColor = true;
        bSave.Click += bSave_Click;
        // 
        // bCancel
        // 
        bCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        bCancel.Location = new System.Drawing.Point(547, 484);
        bCancel.Name = "bCancel";
        bCancel.Size = new System.Drawing.Size(96, 32);
        bCancel.TabIndex = 11;
        bCancel.Text = "Cancel";
        bCancel.UseVisualStyleBackColor = true;
        bCancel.Click += bCancel_Click;
        // 
        // chAutoReload
        // 
        chAutoReload.AutoSize = true;
        chAutoReload.Location = new System.Drawing.Point(6, 157);
        chAutoReload.Name = "chAutoReload";
        chAutoReload.Size = new System.Drawing.Size(121, 19);
        chAutoReload.TabIndex = 9;
        chAutoReload.Text = "Automatic Reload";
        toolTip.SetToolTip(chAutoReload, "Automatically reload opened file, if another program modifies it and there are no unsaved changes in current editor.");
        chAutoReload.UseVisualStyleBackColor = true;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new System.Drawing.Point(6, 3);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(101, 15);
        label4.TabIndex = 11;
        label4.Text = "Default Encoding:";
        // 
        // cbEncodings
        // 
        cbEncodings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cbEncodings.FormattingEnabled = true;
        cbEncodings.Location = new System.Drawing.Point(6, 23);
        cbEncodings.Name = "cbEncodings";
        cbEncodings.Size = new System.Drawing.Size(337, 23);
        cbEncodings.TabIndex = 8;
        // 
        // label5
        // 
        label5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        label5.AutoSize = true;
        label5.Location = new System.Drawing.Point(6, 388);
        label5.Name = "label5";
        label5.Size = new System.Drawing.Size(31, 15);
        label5.TabIndex = 12;
        label5.Text = "===";
        // 
        // tabControl1
        // 
        tabControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        tabControl1.Controls.Add(tabPage1);
        tabControl1.Controls.Add(tabPage2);
        tabControl1.Controls.Add(tabPage3);
        tabControl1.Location = new System.Drawing.Point(6, 6);
        tabControl1.Name = "tabControl1";
        tabControl1.SelectedIndex = 0;
        tabControl1.Size = new System.Drawing.Size(637, 472);
        tabControl1.TabIndex = 13;
        // 
        // tabPage1
        // 
        tabPage1.BackColor = System.Drawing.SystemColors.Control;
        tabPage1.Controls.Add(bAssociateCurrentUser);
        tabPage1.Controls.Add(bAssociateAllUsers);
        tabPage1.Controls.Add(label4);
        tabPage1.Controls.Add(label5);
        tabPage1.Controls.Add(cbEncodings);
        tabPage1.Controls.Add(chAutoReload);
        tabPage1.Location = new System.Drawing.Point(4, 24);
        tabPage1.Name = "tabPage1";
        tabPage1.Padding = new System.Windows.Forms.Padding(3);
        tabPage1.Size = new System.Drawing.Size(629, 444);
        tabPage1.TabIndex = 0;
        tabPage1.Text = "General";
        // 
        // bAssociateCurrentUser
        // 
        bAssociateCurrentUser.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        bAssociateCurrentUser.Location = new System.Drawing.Point(177, 406);
        bAssociateCurrentUser.Name = "bAssociateCurrentUser";
        bAssociateCurrentUser.Size = new System.Drawing.Size(166, 32);
        bAssociateCurrentUser.TabIndex = 14;
        bAssociateCurrentUser.Text = "For Current User Only";
        bAssociateCurrentUser.UseVisualStyleBackColor = true;
        bAssociateCurrentUser.Click += bAssociateCurrentUser_Click;
        // 
        // bAssociateAllUsers
        // 
        bAssociateAllUsers.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        bAssociateAllUsers.FlatStyle = System.Windows.Forms.FlatStyle.System;
        bAssociateAllUsers.Location = new System.Drawing.Point(6, 406);
        bAssociateAllUsers.Name = "bAssociateAllUsers";
        bAssociateAllUsers.Size = new System.Drawing.Size(165, 32);
        bAssociateAllUsers.TabIndex = 13;
        bAssociateAllUsers.Text = "For All Users";
        bAssociateAllUsers.UseVisualStyleBackColor = true;
        bAssociateAllUsers.Click += bAssociateAllUsers_Click;
        // 
        // tabPage2
        // 
        tabPage2.BackColor = System.Drawing.SystemColors.Control;
        tabPage2.Controls.Add(fontPickerMain);
        tabPage2.Controls.Add(chWrap);
        tabPage2.Controls.Add(exampleText);
        tabPage2.Controls.Add(label2);
        tabPage2.Location = new System.Drawing.Point(4, 24);
        tabPage2.Name = "tabPage2";
        tabPage2.Padding = new System.Windows.Forms.Padding(3);
        tabPage2.Size = new System.Drawing.Size(629, 444);
        tabPage2.TabIndex = 1;
        tabPage2.Text = "Font";
        // 
        // fontPickerMain
        // 
        fontPickerMain.Location = new System.Drawing.Point(6, 6);
        fontPickerMain.Name = "fontPickerMain";
        fontPickerMain.Size = new System.Drawing.Size(348, 103);
        fontPickerMain.TabIndex = 8;
        fontPickerMain.Changed += fontPickerMain_Changed;
        // 
        // tabPage3
        // 
        tabPage3.BackColor = System.Drawing.SystemColors.Control;
        tabPage3.Controls.Add(gbPrintSettingsPreview);
        tabPage3.Controls.Add(pPrintSettingsContainer);
        tabPage3.Location = new System.Drawing.Point(4, 24);
        tabPage3.Name = "tabPage3";
        tabPage3.Padding = new System.Windows.Forms.Padding(3);
        tabPage3.Size = new System.Drawing.Size(629, 444);
        tabPage3.TabIndex = 2;
        tabPage3.Text = "Print";
        // 
        // gbPrintSettingsPreview
        // 
        gbPrintSettingsPreview.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        gbPrintSettingsPreview.Controls.Add(lPreviewPageNumber);
        gbPrintSettingsPreview.Controls.Add(lPreviewFileName);
        gbPrintSettingsPreview.Location = new System.Drawing.Point(6, 334);
        gbPrintSettingsPreview.Name = "gbPrintSettingsPreview";
        gbPrintSettingsPreview.Size = new System.Drawing.Size(617, 104);
        gbPrintSettingsPreview.TabIndex = 1;
        gbPrintSettingsPreview.TabStop = false;
        gbPrintSettingsPreview.Text = "Preview";
        // 
        // lPreviewPageNumber
        // 
        lPreviewPageNumber.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        lPreviewPageNumber.Location = new System.Drawing.Point(4, 34);
        lPreviewPageNumber.Name = "lPreviewPageNumber";
        lPreviewPageNumber.Size = new System.Drawing.Size(609, 67);
        lPreviewPageNumber.TabIndex = 1;
        lPreviewPageNumber.Text = "=";
        // 
        // lPreviewFileName
        // 
        lPreviewFileName.AutoSize = true;
        lPreviewFileName.Location = new System.Drawing.Point(4, 19);
        lPreviewFileName.Name = "lPreviewFileName";
        lPreviewFileName.Size = new System.Drawing.Size(15, 15);
        lPreviewFileName.TabIndex = 0;
        lPreviewFileName.Text = "=";
        // 
        // pPrintSettingsContainer
        // 
        pPrintSettingsContainer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        pPrintSettingsContainer.AutoScroll = true;
        pPrintSettingsContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        pPrintSettingsContainer.Controls.Add(printSettingsEditor);
        pPrintSettingsContainer.Location = new System.Drawing.Point(6, 6);
        pPrintSettingsContainer.Name = "pPrintSettingsContainer";
        pPrintSettingsContainer.Size = new System.Drawing.Size(617, 322);
        pPrintSettingsContainer.TabIndex = 0;
        // 
        // printSettingsEditor
        // 
        printSettingsEditor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        printSettingsEditor.ImmediatePageNumberTemplateChange = true;
        printSettingsEditor.Location = new System.Drawing.Point(3, 3);
        printSettingsEditor.Name = "printSettingsEditor";
        printSettingsEditor.Size = new System.Drawing.Size(609, 44);
        printSettingsEditor.TabIndex = 0;
        printSettingsEditor.Changed += printSettingsEditor_Changed;
        // 
        // SettingsDialog
        // 
        AcceptButton = bSave;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        CancelButton = bCancel;
        ClientSize = new System.Drawing.Size(649, 525);
        Controls.Add(tabControl1);
        Controls.Add(bCancel);
        Controls.Add(bSave);
        MinimizeBox = false;
        Name = "SettingsDialog";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Load += SettingsDialog_Load;
        tabControl1.ResumeLayout(false);
        tabPage1.ResumeLayout(false);
        tabPage1.PerformLayout();
        tabPage2.ResumeLayout(false);
        tabPage2.PerformLayout();
        tabPage3.ResumeLayout(false);
        gbPrintSettingsPreview.ResumeLayout(false);
        gbPrintSettingsPreview.PerformLayout();
        pPrintSettingsContainer.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion
    private System.Windows.Forms.Button bSave;
    private System.Windows.Forms.Button bCancel;
    private System.Windows.Forms.TextBox exampleText;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.CheckBox chWrap;
    private System.Windows.Forms.CheckBox chAutoReload;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ComboBox cbEncodings;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.Button bAssociateAllUsers;
    private System.Windows.Forms.Button bAssociateCurrentUser;
    private Controls.FontPicker fontPickerMain;
    private System.Windows.Forms.TabPage tabPage3;
    private System.Windows.Forms.GroupBox gbPrintSettingsPreview;
    private System.Windows.Forms.Panel pPrintSettingsContainer;
    private Controls.PrintSettingsEditor printSettingsEditor;
    private System.Windows.Forms.Label lPreviewPageNumber;
    private System.Windows.Forms.Label lPreviewFileName;
}