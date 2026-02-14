namespace FPad;

partial class GoToLineDialog
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
        bOk = new System.Windows.Forms.Button();
        bCancel = new System.Windows.Forms.Button();
        tbValue = new System.Windows.Forms.TextBox();
        errorLabel = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(12, 9);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(31, 15);
        label1.TabIndex = 0;
        label1.Text = "===";
        // 
        // bOk
        // 
        bOk.Location = new System.Drawing.Point(12, 71);
        bOk.Name = "bOk";
        bOk.Size = new System.Drawing.Size(111, 34);
        bOk.TabIndex = 2;
        bOk.Text = "Go";
        bOk.UseVisualStyleBackColor = true;
        bOk.Click += bOk_Click;
        // 
        // bCancel
        // 
        bCancel.Location = new System.Drawing.Point(129, 71);
        bCancel.Name = "bCancel";
        bCancel.Size = new System.Drawing.Size(111, 34);
        bCancel.TabIndex = 3;
        bCancel.Text = "Cancel";
        bCancel.UseVisualStyleBackColor = true;
        bCancel.Click += bCancel_Click;
        // 
        // tbValue
        // 
        tbValue.Location = new System.Drawing.Point(12, 27);
        tbValue.Name = "tbValue";
        tbValue.Size = new System.Drawing.Size(228, 23);
        tbValue.TabIndex = 4;
        // 
        // errorLabel
        // 
        errorLabel.AutoSize = true;
        errorLabel.ForeColor = System.Drawing.Color.Red;
        errorLabel.Location = new System.Drawing.Point(12, 53);
        errorLabel.Name = "errorLabel";
        errorLabel.Size = new System.Drawing.Size(31, 15);
        errorLabel.TabIndex = 5;
        errorLabel.Text = "===";
        errorLabel.Visible = false;
        // 
        // GoToLineDialog
        // 
        AcceptButton = bOk;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        CancelButton = bCancel;
        ClientSize = new System.Drawing.Size(252, 120);
        Controls.Add(errorLabel);
        Controls.Add(tbValue);
        Controls.Add(bCancel);
        Controls.Add(bOk);
        Controls.Add(label1);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "GoToLineDialog";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "Go to Line";
        Activated += GoToLineDialog_Activated;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button bOk;
    private System.Windows.Forms.Button bCancel;
    private System.Windows.Forms.TextBox tbValue;
    private System.Windows.Forms.Label errorLabel;
}