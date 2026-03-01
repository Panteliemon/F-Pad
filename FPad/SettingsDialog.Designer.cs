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
        label4 = new System.Windows.Forms.Label();
        cbEncodings = new System.Windows.Forms.ComboBox();
        label5 = new System.Windows.Forms.Label();
        tabControl1 = new System.Windows.Forms.TabControl();
        tabPage1 = new System.Windows.Forms.TabPage();
        bAssociateCurrentUser = new System.Windows.Forms.Button();
        bAssociateAllUsers = new System.Windows.Forms.Button();
        tabPage2 = new System.Windows.Forms.TabPage();
        ((System.ComponentModel.ISupportInitialize)tbFontSize).BeginInit();
        ((System.ComponentModel.ISupportInitialize)slFontSize).BeginInit();
        tabControl1.SuspendLayout();
        tabPage1.SuspendLayout();
        tabPage2.SuspendLayout();
        SuspendLayout();
        // 
        // chWrap
        // 
        chWrap.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        chWrap.AutoSize = true;
        chWrap.Location = new System.Drawing.Point(6, 281);
        chWrap.Name = "chWrap";
        chWrap.Size = new System.Drawing.Size(202, 19);
        chWrap.TabIndex = 7;
        chWrap.Text = "Wrap Lines (for this preview only)";
        chWrap.UseVisualStyleBackColor = true;
        chWrap.CheckedChanged += chWrap_CheckedChanged;
        // 
        // chItalic
        // 
        chItalic.AutoSize = true;
        chItalic.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
        chItalic.Location = new System.Drawing.Point(380, 23);
        chItalic.Name = "chItalic";
        chItalic.Size = new System.Drawing.Size(51, 19);
        chItalic.TabIndex = 3;
        chItalic.Text = "Italic";
        chItalic.UseVisualStyleBackColor = true;
        chItalic.CheckedChanged += chItalic_CheckedChanged;
        // 
        // chBold
        // 
        chBold.AutoSize = true;
        chBold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        chBold.Location = new System.Drawing.Point(291, 23);
        chBold.Name = "chBold";
        chBold.Size = new System.Drawing.Size(51, 19);
        chBold.TabIndex = 2;
        chBold.Text = "Bold";
        chBold.UseVisualStyleBackColor = true;
        chBold.CheckedChanged += chBold_CheckedChanged;
        // 
        // tbFontSize
        // 
        tbFontSize.Location = new System.Drawing.Point(60, 50);
        tbFontSize.Maximum = new decimal(new int[] { 128, 0, 0, 0 });
        tbFontSize.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
        tbFontSize.Name = "tbFontSize";
        tbFontSize.Size = new System.Drawing.Size(73, 23);
        tbFontSize.TabIndex = 4;
        tbFontSize.Value = new decimal(new int[] { 5, 0, 0, 0 });
        tbFontSize.ValueChanged += tbFontSize_ValueChanged;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new System.Drawing.Point(6, 52);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(30, 15);
        label3.TabIndex = 5;
        label3.Text = "Size:";
        // 
        // slFontSize
        // 
        slFontSize.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        slFontSize.BackColor = System.Drawing.SystemColors.Control;
        slFontSize.Location = new System.Drawing.Point(6, 79);
        slFontSize.Maximum = 36;
        slFontSize.Minimum = 5;
        slFontSize.Name = "slFontSize";
        slFontSize.Size = new System.Drawing.Size(492, 45);
        slFontSize.TabIndex = 5;
        slFontSize.Value = 5;
        slFontSize.Scroll += slFontSize_Scroll;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(6, 133);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(51, 15);
        label2.TabIndex = 3;
        label2.Text = "Preview:";
        // 
        // cbFonts
        // 
        cbFonts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cbFonts.FormattingEnabled = true;
        cbFonts.Location = new System.Drawing.Point(6, 21);
        cbFonts.Name = "cbFonts";
        cbFonts.Size = new System.Drawing.Size(265, 23);
        cbFonts.TabIndex = 1;
        cbFonts.SelectedIndexChanged += cbFonts_SelectedIndexChanged;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(6, 3);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(42, 15);
        label1.TabIndex = 1;
        label1.Text = "Name:";
        // 
        // exampleText
        // 
        exampleText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        exampleText.BackColor = System.Drawing.SystemColors.Window;
        exampleText.Location = new System.Drawing.Point(6, 151);
        exampleText.Multiline = true;
        exampleText.Name = "exampleText";
        exampleText.ReadOnly = true;
        exampleText.Size = new System.Drawing.Size(492, 124);
        exampleText.TabIndex = 6;
        // 
        // bSave
        // 
        bSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        bSave.Location = new System.Drawing.Point(372, 369);
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
        bCancel.Location = new System.Drawing.Point(474, 369);
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
        chAutoReload.Location = new System.Drawing.Point(6, 84);
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
        label5.AutoSize = true;
        label5.Location = new System.Drawing.Point(6, 137);
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
        tabControl1.Location = new System.Drawing.Point(6, 6);
        tabControl1.Name = "tabControl1";
        tabControl1.SelectedIndex = 0;
        tabControl1.Size = new System.Drawing.Size(564, 357);
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
        tabPage1.Size = new System.Drawing.Size(556, 329);
        tabPage1.TabIndex = 0;
        tabPage1.Text = "General";
        // 
        // bAssociateCurrentUser
        // 
        bAssociateCurrentUser.Location = new System.Drawing.Point(177, 155);
        bAssociateCurrentUser.Name = "bAssociateCurrentUser";
        bAssociateCurrentUser.Size = new System.Drawing.Size(166, 32);
        bAssociateCurrentUser.TabIndex = 14;
        bAssociateCurrentUser.Text = "For Current User Only";
        bAssociateCurrentUser.UseVisualStyleBackColor = true;
        bAssociateCurrentUser.Click += bAssociateCurrentUser_Click;
        // 
        // bAssociateAllUsers
        // 
        bAssociateAllUsers.FlatStyle = System.Windows.Forms.FlatStyle.System;
        bAssociateAllUsers.Location = new System.Drawing.Point(6, 155);
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
        tabPage2.Controls.Add(chWrap);
        tabPage2.Controls.Add(label1);
        tabPage2.Controls.Add(exampleText);
        tabPage2.Controls.Add(label2);
        tabPage2.Controls.Add(slFontSize);
        tabPage2.Controls.Add(tbFontSize);
        tabPage2.Controls.Add(chItalic);
        tabPage2.Controls.Add(label3);
        tabPage2.Controls.Add(cbFonts);
        tabPage2.Controls.Add(chBold);
        tabPage2.Location = new System.Drawing.Point(4, 24);
        tabPage2.Name = "tabPage2";
        tabPage2.Padding = new System.Windows.Forms.Padding(3);
        tabPage2.Size = new System.Drawing.Size(556, 329);
        tabPage2.TabIndex = 1;
        tabPage2.Text = "Font";
        // 
        // SettingsDialog
        // 
        AcceptButton = bSave;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        CancelButton = bCancel;
        ClientSize = new System.Drawing.Size(576, 410);
        Controls.Add(tabControl1);
        Controls.Add(bCancel);
        Controls.Add(bSave);
        MinimizeBox = false;
        Name = "SettingsDialog";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Load += SettingsDialog_Load;
        ((System.ComponentModel.ISupportInitialize)tbFontSize).EndInit();
        ((System.ComponentModel.ISupportInitialize)slFontSize).EndInit();
        tabControl1.ResumeLayout(false);
        tabPage1.ResumeLayout(false);
        tabPage1.PerformLayout();
        tabPage2.ResumeLayout(false);
        tabPage2.PerformLayout();
        ResumeLayout(false);
    }

    #endregion
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
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ComboBox cbEncodings;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.Button bAssociateAllUsers;
    private System.Windows.Forms.Button bAssociateCurrentUser;
}