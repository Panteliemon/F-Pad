using System.Drawing;
using System.Windows.Forms;
using FPad.Controls;

namespace FPad
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            statusBar = new StatusStrip();
            msgLabel = new ToolStripStatusLabel();
            labelExternallyModified = new ToolStripStatusLabel();
            toolStripButtonReload = new ToolStripStatusButton();
            modifiedLabel = new ToolStripStatusLabel();
            lineAndColLabel = new ToolStripStatusLabel();
            labelSelection = new ToolStripStatusLabel();
            wrapLabel = new ToolStripStatusLabel();
            encodingLabel = new ToolStripStatusLabel();
            mainMenu = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            cutToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            pasteToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            findToolStripMenuItem = new ToolStripMenuItem();
            replaceToolStripMenuItem = new ToolStripMenuItem();
            goToLineMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            wrapLinesMenuItem = new ToolStripMenuItem();
            encodingToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            preferencesToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            panel1 = new Panel();
            text = new GoodTextBox();
            blinkingTimer = new Timer(components);
            statusBar.SuspendLayout();
            mainMenu.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // statusBar
            // 
            statusBar.Items.AddRange(new ToolStripItem[] { msgLabel, labelExternallyModified, toolStripButtonReload, modifiedLabel, lineAndColLabel, labelSelection, wrapLabel, encodingLabel });
            statusBar.Location = new Point(0, 426);
            statusBar.Name = "statusBar";
            statusBar.Size = new Size(800, 24);
            statusBar.TabIndex = 0;
            statusBar.Text = "statusStrip1";
            // 
            // msgLabel
            // 
            msgLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            msgLabel.BorderStyle = Border3DStyle.SunkenOuter;
            msgLabel.Margin = new Padding(2, 3, 0, 2);
            msgLabel.Name = "msgLabel";
            msgLabel.Size = new Size(321, 19);
            msgLabel.Spring = true;
            msgLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelExternallyModified
            // 
            labelExternallyModified.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            labelExternallyModified.BorderStyle = Border3DStyle.SunkenOuter;
            labelExternallyModified.ForeColor = SystemColors.ControlText;
            labelExternallyModified.Name = "labelExternallyModified";
            labelExternallyModified.Size = new Size(168, 19);
            labelExternallyModified.Text = "Changed by another program";
            // 
            // toolStripButtonReload
            // 
            toolStripButtonReload.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            toolStripButtonReload.BorderStyle = Border3DStyle.RaisedOuter;
            toolStripButtonReload.IsPressed = false;
            toolStripButtonReload.Name = "toolStripButtonReload";
            toolStripButtonReload.Size = new Size(70, 19);
            toolStripButtonReload.Text = "Reload (F5)";
            toolStripButtonReload.MouseUp += toolStripButtonReload_MouseUp;
            // 
            // modifiedLabel
            // 
            modifiedLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            modifiedLabel.BorderStyle = Border3DStyle.SunkenOuter;
            modifiedLabel.ForeColor = Color.MediumBlue;
            modifiedLabel.Name = "modifiedLabel";
            modifiedLabel.Size = new Size(43, 19);
            modifiedLabel.Text = "Modif";
            // 
            // lineAndColLabel
            // 
            lineAndColLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            lineAndColLabel.BorderStyle = Border3DStyle.SunkenOuter;
            lineAndColLabel.Name = "lineAndColLabel";
            lineAndColLabel.Size = new Size(54, 19);
            lineAndColLabel.Text = "Line Col";
            // 
            // labelSelection
            // 
            labelSelection.BackColor = Color.MediumBlue;
            labelSelection.ForeColor = Color.White;
            labelSelection.Name = "labelSelection";
            labelSelection.Size = new Size(22, 19);
            labelSelection.Text = "Sel";
            // 
            // wrapLabel
            // 
            wrapLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            wrapLabel.BorderStyle = Border3DStyle.SunkenOuter;
            wrapLabel.Name = "wrapLabel";
            wrapLabel.Size = new Size(44, 19);
            wrapLabel.Text = "WRAP";
            // 
            // encodingLabel
            // 
            encodingLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            encodingLabel.BorderStyle = Border3DStyle.SunkenOuter;
            encodingLabel.Name = "encodingLabel";
            encodingLabel.Size = new Size(61, 19);
            encodingLabel.Text = "Encoding";
            // 
            // mainMenu
            // 
            mainMenu.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, helpToolStripMenuItem });
            mainMenu.Location = new Point(0, 0);
            mainMenu.Name = "mainMenu";
            mainMenu.Size = new Size(800, 24);
            mainMenu.TabIndex = 1;
            mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newToolStripMenuItem.Size = new Size(186, 22);
            newToolStripMenuItem.Text = "&New";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openToolStripMenuItem.Size = new Size(186, 22);
            openToolStripMenuItem.Text = "&Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(186, 22);
            saveToolStripMenuItem.Text = "&Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            saveAsToolStripMenuItem.Size = new Size(186, 22);
            saveAsToolStripMenuItem.Text = "Save &As";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(183, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(186, 22);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cutToolStripMenuItem, copyToolStripMenuItem, pasteToolStripMenuItem, toolStripSeparator2, findToolStripMenuItem, replaceToolStripMenuItem, goToLineMenuItem, toolStripSeparator3, wrapLinesMenuItem, encodingToolStripMenuItem, toolStripSeparator4, preferencesToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "&Edit";
            // 
            // cutToolStripMenuItem
            // 
            cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            cutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            cutToolStripMenuItem.Size = new Size(180, 22);
            cutToolStripMenuItem.Text = "Cu&t";
            cutToolStripMenuItem.Click += cutToolStripMenuItem_Click;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            copyToolStripMenuItem.Size = new Size(180, 22);
            copyToolStripMenuItem.Text = "&Copy";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;
            pasteToolStripMenuItem.Size = new Size(180, 22);
            pasteToolStripMenuItem.Text = "&Paste";
            pasteToolStripMenuItem.Click += pasteToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(177, 6);
            // 
            // findToolStripMenuItem
            // 
            findToolStripMenuItem.Name = "findToolStripMenuItem";
            findToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;
            findToolStripMenuItem.Size = new Size(180, 22);
            findToolStripMenuItem.Text = "&Find";
            findToolStripMenuItem.Click += findToolStripMenuItem_Click;
            // 
            // replaceToolStripMenuItem
            // 
            replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            replaceToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.H;
            replaceToolStripMenuItem.Size = new Size(180, 22);
            replaceToolStripMenuItem.Text = "&Replace";
            replaceToolStripMenuItem.Click += replaceToolStripMenuItem_Click;
            // 
            // goToLineMenuItem
            // 
            goToLineMenuItem.Name = "goToLineMenuItem";
            goToLineMenuItem.ShortcutKeys = Keys.Control | Keys.G;
            goToLineMenuItem.Size = new Size(180, 22);
            goToLineMenuItem.Text = "Go to Line";
            goToLineMenuItem.Click += goToLineMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(177, 6);
            // 
            // wrapLinesMenuItem
            // 
            wrapLinesMenuItem.Name = "wrapLinesMenuItem";
            wrapLinesMenuItem.Size = new Size(180, 22);
            wrapLinesMenuItem.Text = "&Wrap Lines";
            wrapLinesMenuItem.Click += wrapLinesMenuItem_Click;
            // 
            // encodingToolStripMenuItem
            // 
            encodingToolStripMenuItem.Name = "encodingToolStripMenuItem";
            encodingToolStripMenuItem.Size = new Size(180, 22);
            encodingToolStripMenuItem.Text = "&Encoding";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(177, 6);
            // 
            // preferencesToolStripMenuItem
            // 
            preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            preferencesToolStripMenuItem.Size = new Size(180, 22);
            preferencesToolStripMenuItem.Text = "Preference&s";
            preferencesToolStripMenuItem.Click += preferencesToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.ShortcutKeys = Keys.F1;
            aboutToolStripMenuItem.Size = new Size(180, 22);
            aboutToolStripMenuItem.Text = "&About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(text);
            panel1.Location = new Point(3, 24);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(794, 399);
            panel1.TabIndex = 3;
            // 
            // text
            // 
            text.AcceptsReturn = true;
            text.AcceptsTab = true;
            text.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            text.BorderStyle = BorderStyle.None;
            text.Location = new Point(-2, 1);
            text.Multiline = true;
            text.Name = "text";
            text.ScrollBars = ScrollBars.Both;
            text.Size = new Size(794, 396);
            text.TabIndex = 3;
            text.SelectionChanged += Text_SelectionChanged;
            text.TextChanged += textBox1_TextChanged;
            // 
            // blinkingTimer
            // 
            blinkingTimer.Interval = 750;
            blinkingTimer.Tick += blinkingTimer_Tick;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Controls.Add(statusBar);
            Controls.Add(mainMenu);
            KeyPreview = true;
            MainMenuStrip = mainMenu;
            Name = "MainWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "F-Pad";
            FormClosing += MainWindow_FormClosing;
            Load += MainWindow_Load;
            LocationChanged += MainWindow_LocationChanged;
            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;
            Resize += MainWindow_Resize;
            statusBar.ResumeLayout(false);
            statusBar.PerformLayout();
            mainMenu.ResumeLayout(false);
            mainMenu.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusBar;
        private MenuStrip mainMenu;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem findToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem preferencesToolStripMenuItem;
        private ToolStripMenuItem encodingToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem wrapLinesMenuItem;
        private ToolStripStatusLabel encodingLabel;
        private ToolStripStatusLabel msgLabel;
        private ToolStripStatusLabel lineAndColLabel;
        private ToolStripStatusLabel wrapLabel;
        private ToolStripStatusLabel modifiedLabel;
        private ToolStripStatusLabel labelSelection;
        private Panel panel1;
        private GoodTextBox text;
        private ToolStripStatusLabel labelExternallyModified;
        private ToolStripStatusButton toolStripButtonReload;
        private Timer blinkingTimer;
        private ToolStripMenuItem goToLineMenuItem;
    }
}
