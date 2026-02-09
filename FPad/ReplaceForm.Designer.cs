namespace FPad;

partial class ReplaceForm
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
        label2 = new System.Windows.Forms.Label();
        tbReplaceWith = new System.Windows.Forms.TextBox();
        bSwap = new System.Windows.Forms.Button();
        chMatchCase = new System.Windows.Forms.CheckBox();
        chWholeWords = new System.Windows.Forms.CheckBox();
        bReplaceAll = new System.Windows.Forms.Button();
        bNext = new System.Windows.Forms.Button();
        bPrev = new System.Windows.Forms.Button();
        bReplace = new System.Windows.Forms.Button();
        bFindFirst = new System.Windows.Forms.Button();
        bReplaceAllInSelection = new System.Windows.Forms.Button();
        labelResult = new System.Windows.Forms.Label();
        labelReachedEnd = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(12, 9);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(33, 15);
        label1.TabIndex = 0;
        label1.Text = "Find:";
        // 
        // tbFind
        // 
        tbFind.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        tbFind.Location = new System.Drawing.Point(12, 27);
        tbFind.Name = "tbFind";
        tbFind.Size = new System.Drawing.Size(434, 23);
        tbFind.TabIndex = 1;
        tbFind.TextChanged += tbFind_TextChanged;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(12, 133);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(77, 15);
        label2.TabIndex = 2;
        label2.Text = "Replace with:";
        // 
        // tbReplaceWith
        // 
        tbReplaceWith.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        tbReplaceWith.Location = new System.Drawing.Point(12, 151);
        tbReplaceWith.Name = "tbReplaceWith";
        tbReplaceWith.Size = new System.Drawing.Size(434, 23);
        tbReplaceWith.TabIndex = 2;
        tbReplaceWith.TextChanged += tbReplaceWith_TextChanged;
        // 
        // bSwap
        // 
        bSwap.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        bSwap.Location = new System.Drawing.Point(387, 121);
        bSwap.Name = "bSwap";
        bSwap.Size = new System.Drawing.Size(59, 24);
        bSwap.TabIndex = 8;
        bSwap.Text = "Swap";
        bSwap.UseVisualStyleBackColor = true;
        bSwap.Click += bSwap_Click;
        // 
        // chMatchCase
        // 
        chMatchCase.AutoSize = true;
        chMatchCase.Location = new System.Drawing.Point(12, 56);
        chMatchCase.Name = "chMatchCase";
        chMatchCase.Size = new System.Drawing.Size(113, 19);
        chMatchCase.TabIndex = 3;
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
        chWholeWords.TabIndex = 4;
        chWholeWords.Text = "[_W_] Whole Words";
        chWholeWords.UseVisualStyleBackColor = true;
        chWholeWords.CheckedChanged += chWholeWords_CheckedChanged;
        // 
        // bReplaceAll
        // 
        bReplaceAll.Location = new System.Drawing.Point(139, 180);
        bReplaceAll.Name = "bReplaceAll";
        bReplaceAll.Size = new System.Drawing.Size(120, 34);
        bReplaceAll.TabIndex = 10;
        bReplaceAll.Text = "Replace All";
        bReplaceAll.UseVisualStyleBackColor = true;
        bReplaceAll.Click += bReplaceAll_Click;
        // 
        // bNext
        // 
        bNext.Location = new System.Drawing.Point(139, 81);
        bNext.Name = "bNext";
        bNext.Size = new System.Drawing.Size(120, 34);
        bNext.TabIndex = 6;
        bNext.Text = "Find Next (F3)";
        bNext.UseVisualStyleBackColor = true;
        bNext.Click += bNext_Click;
        // 
        // bPrev
        // 
        bPrev.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        bPrev.Location = new System.Drawing.Point(265, 81);
        bPrev.Name = "bPrev";
        bPrev.Size = new System.Drawing.Size(181, 34);
        bPrev.TabIndex = 7;
        bPrev.Text = "Find Previous (Shift+F3)";
        bPrev.UseVisualStyleBackColor = true;
        bPrev.Click += bPrev_Click;
        // 
        // bReplace
        // 
        bReplace.Location = new System.Drawing.Point(12, 180);
        bReplace.Name = "bReplace";
        bReplace.Size = new System.Drawing.Size(120, 34);
        bReplace.TabIndex = 9;
        bReplace.Text = "Replace (F4)";
        bReplace.UseVisualStyleBackColor = true;
        bReplace.Click += bReplace_Click;
        // 
        // bFindFirst
        // 
        bFindFirst.Location = new System.Drawing.Point(12, 81);
        bFindFirst.Name = "bFindFirst";
        bFindFirst.Size = new System.Drawing.Size(120, 34);
        bFindFirst.TabIndex = 5;
        bFindFirst.Text = "Find First";
        bFindFirst.UseVisualStyleBackColor = true;
        bFindFirst.Click += bFindFirst_Click;
        // 
        // bReplaceAllInSelection
        // 
        bReplaceAllInSelection.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        bReplaceAllInSelection.Location = new System.Drawing.Point(265, 180);
        bReplaceAllInSelection.Name = "bReplaceAllInSelection";
        bReplaceAllInSelection.Size = new System.Drawing.Size(181, 34);
        bReplaceAllInSelection.TabIndex = 11;
        bReplaceAllInSelection.Text = "Replace All in Selection";
        bReplaceAllInSelection.UseVisualStyleBackColor = true;
        bReplaceAllInSelection.Click += bReplaceAllInSelection_Click;
        // 
        // labelResult
        // 
        labelResult.AutoSize = true;
        labelResult.Location = new System.Drawing.Point(12, 217);
        labelResult.Name = "labelResult";
        labelResult.Size = new System.Drawing.Size(31, 15);
        labelResult.TabIndex = 13;
        labelResult.Text = "===";
        // 
        // labelReachedEnd
        // 
        labelReachedEnd.Location = new System.Drawing.Point(265, 217);
        labelReachedEnd.Name = "labelReachedEnd";
        labelReachedEnd.Size = new System.Drawing.Size(181, 15);
        labelReachedEnd.TabIndex = 14;
        labelReachedEnd.Text = "===";
        labelReachedEnd.TextAlign = System.Drawing.ContentAlignment.TopRight;
        labelReachedEnd.Visible = false;
        // 
        // ReplaceForm
        // 
        AcceptButton = bFindFirst;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(458, 245);
        Controls.Add(labelReachedEnd);
        Controls.Add(labelResult);
        Controls.Add(bReplaceAllInSelection);
        Controls.Add(bFindFirst);
        Controls.Add(bReplace);
        Controls.Add(bPrev);
        Controls.Add(bNext);
        Controls.Add(bReplaceAll);
        Controls.Add(chWholeWords);
        Controls.Add(chMatchCase);
        Controls.Add(bSwap);
        Controls.Add(tbReplaceWith);
        Controls.Add(label2);
        Controls.Add(tbFind);
        Controls.Add(label1);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        KeyPreview = true;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ReplaceForm";
        ShowInTaskbar = false;
        Text = "Replace";
        FormClosed += ReplaceForm_FormClosed;
        Load += ReplaceForm_Load;
        KeyDown += ReplaceForm_KeyDown;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox tbFind;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox tbReplaceWith;
    private System.Windows.Forms.Button bSwap;
    private System.Windows.Forms.CheckBox chMatchCase;
    private System.Windows.Forms.CheckBox chWholeWords;
    private System.Windows.Forms.Button bReplaceAll;
    private System.Windows.Forms.Button bNext;
    private System.Windows.Forms.Button bPrev;
    private System.Windows.Forms.Button bReplace;
    private System.Windows.Forms.Button bFindFirst;
    private System.Windows.Forms.Button bReplaceAllInSelection;
    private System.Windows.Forms.Label labelResult;
    private System.Windows.Forms.Label labelReachedEnd;
}