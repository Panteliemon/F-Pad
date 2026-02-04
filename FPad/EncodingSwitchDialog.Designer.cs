namespace FPad;

partial class EncodingSwitchDialog
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
        label1 = new System.Windows.Forms.Label();
        bReinterpret = new System.Windows.Forms.Button();
        bUseForSaving = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(12, 9);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(38, 15);
        label1.TabIndex = 0;
        label1.Text = "label1";
        // 
        // bReinterpret
        // 
        bReinterpret.Location = new System.Drawing.Point(12, 39);
        bReinterpret.Name = "bReinterpret";
        bReinterpret.Size = new System.Drawing.Size(165, 37);
        bReinterpret.TabIndex = 1;
        bReinterpret.Text = "&Reinterpret Bytes";
        bReinterpret.UseVisualStyleBackColor = true;
        bReinterpret.Click += bReinterpret_Click;
        // 
        // bUseForSaving
        // 
        bUseForSaving.Location = new System.Drawing.Point(183, 39);
        bUseForSaving.Name = "bUseForSaving";
        bUseForSaving.Size = new System.Drawing.Size(165, 37);
        bUseForSaving.TabIndex = 2;
        bUseForSaving.Text = "Use for &Save";
        bUseForSaving.UseVisualStyleBackColor = true;
        bUseForSaving.Click += bUseForSaving_Click;
        // 
        // EncodingSwitchDialog
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(359, 86);
        Controls.Add(bUseForSaving);
        Controls.Add(bReinterpret);
        Controls.Add(label1);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        KeyPreview = true;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "EncodingSwitchDialog";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "Switch Encoding";
        Activated += EncodingSwitchDialog_Activated;
        KeyUp += EncodingSwitchDialog_KeyUp;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button bReinterpret;
    private System.Windows.Forms.Button bUseForSaving;
}