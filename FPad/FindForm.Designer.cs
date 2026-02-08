namespace FPad;

partial class FindForm
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
        tbFind = new System.Windows.Forms.TextBox();
        bFindFirst = new System.Windows.Forms.Button();
        bFindNext = new System.Windows.Forms.Button();
        bFindPrev = new System.Windows.Forms.Button();
        chMatchCase = new System.Windows.Forms.CheckBox();
        chWholeWords = new System.Windows.Forms.CheckBox();
        labelResults = new System.Windows.Forms.Label();
        labelReachedEnd = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(12, 9);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(54, 15);
        label1.TabIndex = 0;
        label1.Text = "Look for:";
        // 
        // tbFind
        // 
        tbFind.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        tbFind.Location = new System.Drawing.Point(12, 27);
        tbFind.Name = "tbFind";
        tbFind.Size = new System.Drawing.Size(369, 23);
        tbFind.TabIndex = 1;
        tbFind.TextChanged += tbFind_TextChanged;
        tbFind.Leave += tbFind_Leave;
        // 
        // bFindFirst
        // 
        bFindFirst.Location = new System.Drawing.Point(12, 81);
        bFindFirst.Name = "bFindFirst";
        bFindFirst.Size = new System.Drawing.Size(113, 34);
        bFindFirst.TabIndex = 2;
        bFindFirst.Text = "Find First";
        bFindFirst.UseVisualStyleBackColor = true;
        bFindFirst.Click += bFindFirst_Click;
        // 
        // bFindNext
        // 
        bFindNext.Location = new System.Drawing.Point(131, 81);
        bFindNext.Name = "bFindNext";
        bFindNext.Size = new System.Drawing.Size(113, 34);
        bFindNext.TabIndex = 3;
        bFindNext.Text = "Next (F3)";
        bFindNext.UseVisualStyleBackColor = true;
        bFindNext.Click += bFindNext_Click;
        // 
        // bFindPrev
        // 
        bFindPrev.Location = new System.Drawing.Point(250, 81);
        bFindPrev.Name = "bFindPrev";
        bFindPrev.Size = new System.Drawing.Size(131, 34);
        bFindPrev.TabIndex = 4;
        bFindPrev.Text = "Previous (Shift+F3)";
        bFindPrev.UseVisualStyleBackColor = true;
        bFindPrev.Click += bFindPrev_Click;
        // 
        // chMatchCase
        // 
        chMatchCase.AutoSize = true;
        chMatchCase.Location = new System.Drawing.Point(12, 56);
        chMatchCase.Name = "chMatchCase";
        chMatchCase.Size = new System.Drawing.Size(113, 19);
        chMatchCase.TabIndex = 5;
        chMatchCase.Text = "[Aa] Match Case";
        chMatchCase.UseVisualStyleBackColor = true;
        chMatchCase.CheckedChanged += chMatchCase_CheckedChanged;
        // 
        // chWholeWords
        // 
        chWholeWords.AutoSize = true;
        chWholeWords.Location = new System.Drawing.Point(170, 56);
        chWholeWords.Name = "chWholeWords";
        chWholeWords.Size = new System.Drawing.Size(129, 19);
        chWholeWords.TabIndex = 6;
        chWholeWords.Text = "[_W_] Whole Words";
        chWholeWords.UseVisualStyleBackColor = true;
        chWholeWords.CheckedChanged += chWholeWords_CheckedChanged;
        // 
        // labelResults
        // 
        labelResults.AutoSize = true;
        labelResults.Location = new System.Drawing.Point(12, 125);
        labelResults.Name = "labelResults";
        labelResults.Size = new System.Drawing.Size(31, 15);
        labelResults.TabIndex = 7;
        labelResults.Text = "===";
        // 
        // labelReachedEnd
        // 
        labelReachedEnd.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        labelReachedEnd.Location = new System.Drawing.Point(250, 125);
        labelReachedEnd.Name = "labelReachedEnd";
        labelReachedEnd.Size = new System.Drawing.Size(131, 15);
        labelReachedEnd.TabIndex = 8;
        labelReachedEnd.Text = "===";
        labelReachedEnd.TextAlign = System.Drawing.ContentAlignment.TopRight;
        labelReachedEnd.Visible = false;
        // 
        // FindForm
        // 
        AcceptButton = bFindFirst;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(393, 149);
        Controls.Add(labelReachedEnd);
        Controls.Add(labelResults);
        Controls.Add(chWholeWords);
        Controls.Add(chMatchCase);
        Controls.Add(bFindPrev);
        Controls.Add(bFindNext);
        Controls.Add(bFindFirst);
        Controls.Add(tbFind);
        Controls.Add(label1);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        KeyPreview = true;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FindForm";
        ShowInTaskbar = false;
        Text = "Find";
        Activated += FindForm_Activated;
        FormClosed += FindForm_FormClosed;
        Load += FindForm_Load;
        KeyDown += FindForm_KeyDown;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox tbFind;
    private System.Windows.Forms.Button bFindFirst;
    private System.Windows.Forms.Button bFindNext;
    private System.Windows.Forms.Button bFindPrev;
    private System.Windows.Forms.CheckBox chMatchCase;
    private System.Windows.Forms.CheckBox chWholeWords;
    private System.Windows.Forms.Label labelResults;
    private System.Windows.Forms.Label labelReachedEnd;
}