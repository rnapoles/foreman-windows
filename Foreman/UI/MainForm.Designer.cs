using Foreman.UI.Components;
using GrayIris.Utilities.UI.Controls;

namespace Foreman.UI
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mnsMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProcfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgOpenProcfile = new System.Windows.Forms.OpenFileDialog();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.tbiStart = new System.Windows.Forms.ToolStripButton();
            this.tbiStop = new System.Windows.Forms.ToolStripButton();
            this.tbiInfo = new System.Windows.Forms.ToolStripButton();
            this.tbiClear = new System.Windows.Forms.ToolStripButton();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl = new GrayIris.Utilities.UI.Controls.YaTabControl();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.systrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.mnsMain.SuspendLayout();
            this.toolBar.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnsMain
            // 
            this.mnsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.mnsMain.Location = new System.Drawing.Point(0, 0);
            this.mnsMain.Name = "mnsMain";
            this.mnsMain.Size = new System.Drawing.Size(806, 24);
            this.mnsMain.TabIndex = 1;
            this.mnsMain.Text = "menuStrip1";
            this.mnsMain.Resize += new System.EventHandler(this.frmMain_Resize);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openProcfileToolStripMenuItem,
            this.stopStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openProcfileToolStripMenuItem
            // 
            this.openProcfileToolStripMenuItem.Name = "openProcfileToolStripMenuItem";
            this.openProcfileToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.openProcfileToolStripMenuItem.Text = "&Open Procfile...";
            this.openProcfileToolStripMenuItem.Click += new System.EventHandler(this.openProcfileToolStripMenuItem_Click);
            // 
            // stopStripMenuItem
            // 
            this.stopStripMenuItem.Name = "stopStripMenuItem";
            this.stopStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.stopStripMenuItem.Text = "Stop";
            this.stopStripMenuItem.Click += new System.EventHandler(this.stopStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // dlgOpenProcfile
            // 
            this.dlgOpenProcfile.Filter = "Procfiles|Procfile*";
            // 
            // toolBar
            // 
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbiStart,
            this.tbiStop,
            this.tbiInfo,
            this.tbiClear});
            this.toolBar.Location = new System.Drawing.Point(0, 24);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(806, 25);
            this.toolBar.TabIndex = 3;
            // 
            // tbiStart
            // 
            this.tbiStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbiStart.Image = ((System.Drawing.Image)(resources.GetObject("tbiStart.Image")));
            this.tbiStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbiStart.Name = "tbiStart";
            this.tbiStart.Size = new System.Drawing.Size(23, 22);
            this.tbiStart.Text = "Start";
            this.tbiStart.Click += new System.EventHandler(this.tbiStart_Click);
            // 
            // tbiStop
            // 
            this.tbiStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbiStop.Image = ((System.Drawing.Image)(resources.GetObject("tbiStop.Image")));
            this.tbiStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbiStop.Name = "tbiStop";
            this.tbiStop.Size = new System.Drawing.Size(23, 22);
            this.tbiStop.Text = "Stop";
            this.tbiStop.Click += new System.EventHandler(this.tbiStop_Click);
            // 
            // tbiInfo
            // 
            this.tbiInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbiInfo.Image = ((System.Drawing.Image)(resources.GetObject("tbiInfo.Image")));
            this.tbiInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbiInfo.Name = "tbiInfo";
            this.tbiInfo.Size = new System.Drawing.Size(23, 22);
            this.tbiInfo.Text = "toolStripButton1";
            this.tbiInfo.ToolTipText = "Enviroment vars";
            this.tbiInfo.Click += new System.EventHandler(this.tbiInfo_Click);
            // 
            // tbiClear
            // 
            this.tbiClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbiClear.Image = ((System.Drawing.Image)(resources.GetObject("tbiClear.Image")));
            this.tbiClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbiClear.Name = "tbiClear";
            this.tbiClear.Size = new System.Drawing.Size(23, 22);
            this.tbiClear.Text = "Clear console";
            this.tbiClear.ToolTipText = "Clear console (Ctrl + L)";
            this.tbiClear.Click += new System.EventHandler(this.tbiClear_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "start.png");
            this.imageList.Images.SetKeyName(1, "stop.png");
            this.imageList.Images.SetKeyName(2, "information.png");
            this.imageList.Images.SetKeyName(3, "clear.png");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(806, 462);
            this.panel1.TabIndex = 4;
            // 
            // tabControl
            // 
            this.tabControl.ActiveColor = System.Drawing.SystemColors.Control;
            this.tabControl.BackColor = System.Drawing.SystemColors.Control;
            this.tabControl.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tabControl.CloseButton = false;
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.HoverColor = System.Drawing.Color.Silver;
            this.tabControl.ImageIndex = -1;
            this.tabControl.ImageList = null;
            this.tabControl.InactiveColor = System.Drawing.SystemColors.Window;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.NewTabButton = false;
            this.tabControl.OverIndex = -1;
            this.tabControl.ScrollButtonStyle = GrayIris.Utilities.UI.Controls.YaScrollButtonStyle.Always;
            this.tabControl.SelectedIndex = -1;
            this.tabControl.SelectedTab = null;
            this.tabControl.Size = new System.Drawing.Size(806, 462);
            this.tabControl.TabDock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.TabDrawer = null;
            this.tabControl.TabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.TabIndex = 3;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // systrayIcon
            // 
            this.systrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("systrayIcon.Icon")));
            this.systrayIcon.Text = "Foreman";
            this.systrayIcon.Visible = true;
            this.systrayIcon.DoubleClick += new System.EventHandler(this.systrayIcon_DoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 511);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.mnsMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnsMain;
            this.Name = "MainForm";
            this.Text = "Foreman";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.mnsMain.ResumeLayout(false);
            this.mnsMain.PerformLayout();
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnsMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProcfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog dlgOpenProcfile;
        private System.Windows.Forms.ToolStripMenuItem stopStripMenuItem;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton tbiStart;
        private System.Windows.Forms.ToolStripButton tbiStop;
        private System.Windows.Forms.ToolStripButton tbiClear;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.NotifyIcon systrayIcon;
        private YaTabControl tabControl;
        private System.Windows.Forms.ToolStripButton tbiInfo;
    }
}

