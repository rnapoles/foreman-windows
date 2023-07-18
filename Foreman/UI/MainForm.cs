using FastColoredTextBoxNS;
using Foreman.Domain;
using Foreman.Infrastructure;
using Foreman.UI.Components;
using GrayIris.Utilities.UI.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Foreman.UI
{
    public partial class MainForm : Form
    {

        enum ToolbarAction
        {
            Start, Stop, Info, Clear
        }

        private Procfile m_objProcfile = null;
        private Dictionary<string, ProcfileEntry> m_processes = new();
        private Dictionary<string, FastColoredTextBox> m_consoles = new();

        private TextStyle blueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Underline);
        private TextStyle greenStyle = new TextStyle(Brushes.Green, null, FontStyle.Bold);
        private TextStyle yellowStyle = new TextStyle(Brushes.Yellow, null, FontStyle.Bold);
        private TextStyle redStyle = new TextStyle(Brushes.Red, null, FontStyle.Bold);

        private string m_urlRegex = @"([a-zA-Z0-9]+:\/\/)(([^:@/?#\[\]\n\r]*)(:([^@/?#\[\]\n\r]*))?@)?(([^/:?#\[\]\n\r]+)|(\[[^\[\]]+\\n\r]))?(:([0-9]*))?(/[^\?#\n\r]*)?(\?([^#\n\r]+)?)?(#([^\n\r]))?";
        private string m_datetimeRegex = @"\[[a-zA-Z]+\s+[a-zA-Z]+\s+\d+\s+\d+:\d+:\d+\s\d+\]";
        private string m_httpVerbRegex = @"\bGET|POST|PUT|PATCH|DELETE\b";
        private string m_httpStatusRegex = @"\[\d+\]";
        //private string m_logLevelRegex = @"ERROR|WARN|DEBUG|TRACE";

        public MainForm()
        {
            InitializeComponent();
            ConfigureTabControl();

            this.KeyPreview = true;
            toolBar.ImageScalingSize = new Size(24, 24);
            toolBar.ImageList = imageList;
            toolBar.Height = 32;

            int i = 0;
            foreach (var item in toolBar.Items)
            {
                if(item is ToolStripButton button)
                {
                    button.AutoSize = false;
                    button.Height = 24;
                    button.Width = 24;
                    button.ImageIndex = i++;
                }
            }

            this.FormClosing += (s, e) =>
            {
                if (m_objProcfile != null)
                {
                    m_objProcfile.Stop();
                }
            };

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && File.Exists(args[1]))
            {
                OpenProcfile(args[1]);
            }
            else if (File.Exists("Procfile"))
            {
                OpenProcfile("Procfile");
            }

        }

        private void ConfigureTabControl()
        {
            tabControl.ActiveColor = System.Drawing.SystemColors.Control;
            tabControl.BackColor = System.Drawing.SystemColors.Control;
            tabControl.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            tabControl.CloseButton = false;
            tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl.HoverColor = System.Drawing.Color.Azure;
            //tabControl.ImageIndex = 0;
            //tabControl.ImageList = this._images;
            //tabControl.NewTabButton = true;
            //tabControl.SelectedTab = this._tabpage1;
            //tabControl.SelectedIndex = 0;
            tabControl.InactiveColor = System.Drawing.SystemColors.Window;
            tabControl.Location = new System.Drawing.Point(0, 24);
            tabControl.Name = "_tabs";
            tabControl.OverIndex = -1;
            tabControl.ScrollButtonStyle = GrayIris.Utilities.UI.Controls.YaScrollButtonStyle.Never;
            tabControl.Size = new System.Drawing.Size(838, 491);
            tabControl.TabDock = System.Windows.Forms.DockStyle.Top;
            tabControl.TabDrawer = new XlTabDrawer();
            tabControl.TabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        }

        private void openProcfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dlgOpenProcfile.ShowDialog() == DialogResult.OK)
            {
                OpenProcfile(dlgOpenProcfile.FileName);
            }
        }

        private void OpenProcfile(string strFilename)
        {
            if (m_objProcfile != null)
            {
                m_objProcfile.Stop();
            }

            tabControl.Controls.Clear();
            m_consoles.Clear();
            m_processes.Clear();

            m_objProcfile = new Procfile(strFilename);
            m_objProcfile.TextReceived += (ProcfileEntry objEntry, string strText) =>
            {
                AppendText(objEntry, strText);
            };

            m_objProcfile.StatusReceived += delegate(string strText)
            {
                //AppendText(m_objProcfile, strText);
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        Text = strText;
                    });
                }
                
            };

            int c = 0;
            tabControl.SuspendLayout();
            this.SuspendLayout();

            foreach (var item in m_objProcfile.ProcfileEntries)
            {

                var id = item.Id;
                var console = new FastColoredTextBox();
                var tabPage = new YaTabPage();
                var propertyGrid = new ReadOnlyPropGrid();
                var splitContainer = new System.Windows.Forms.SplitContainer();

                tabPage.SuspendLayout();
                splitContainer.Panel1.SuspendLayout();
                splitContainer.Panel2.SuspendLayout();


                splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
                splitContainer.Name = "s" + id;

                console.Dock = DockStyle.Fill;
                //txtConsole.Name = "txtConsole";
                console.Tag = id;
                console.Text = "";
                console.ReadOnly = true;
                console.ForeColor = Color.White;
                console.BackColor = Color.Black;
                console.TextChangedDelayed += Console_TextChangedDelayed;
                console.MouseDown += Console_MouseDown;
                console.MouseMove += Console_MouseMove;
                console.BorderStyle = BorderStyle.Fixed3D;

                propertyGrid.Dock = DockStyle.Fill;
                propertyGrid.SelectedObject = new DictionaryPropertyGridAdapter(item.EnvVariables);
                propertyGrid.PropertySort = PropertySort.Alphabetical;
                propertyGrid.Name = "p" + id;
                splitContainer.SplitterDistance = 30;

                //tabPage.BackColor = Color.Violet;
                var name = item.Name;
                tabPage.Tag = id;
                tabPage.Text = $"   {name}   ";
                tabPage.Dock = System.Windows.Forms.DockStyle.Fill;
                tabPage.Location = new System.Drawing.Point(4, 31);
                tabPage.Size = new System.Drawing.Size(830, 456);
                tabPage.TabIndex = c++;
                tabPage.Width = 100;

                splitContainer.Panel1.Controls.Add(propertyGrid);
                splitContainer.Panel2.Controls.Add(console);
                tabPage.Controls.Add(splitContainer);
                m_consoles.Add(id, console);
                m_processes.Add(id, item);
                tabControl.Controls.Add(tabPage);

                splitContainer.Panel1.ResumeLayout(true);
                splitContainer.Panel2.ResumeLayout(true);
                tabPage.ResumeLayout(true);

                splitContainer.Panel1.PerformLayout();
                splitContainer.Panel2.PerformLayout();
                tabPage.PerformLayout();

            }

            tabControl.ResumeLayout(false);
            tabControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            m_objProcfile.Start();
        }

        private void Console_MouseMove(object sender, MouseEventArgs e)
        {
            var fctb = (FastColoredTextBox)sender;
            var p = fctb.PointToPlace(e.Location);
            if (GetUrl(fctb, p) != null)
                fctb.Cursor = Cursors.Hand;
            else
                fctb.Cursor = Cursors.IBeam;
        }

        private string GetUrl(FastColoredTextBox fctb, Place p)
        {

            var url = fctb.GetRange(p, p).GetFragment(@"[\S]").Text;
            Match match = Regex.Match(url, m_urlRegex);

            if (match.Success)
            {
                return match.Value;
            }

            return null;
        }

        private void Console_MouseDown(object sender, MouseEventArgs e)
        {

            var fctb = (FastColoredTextBox)sender;
            var p = fctb.PointToPlace(e.Location);
            var url = GetUrl(fctb, p);

            if (url != null)
            {
                //MessageBox.Show(match.Value);
                Process.Start(url);
            }
        }

        private void Console_TextChangedDelayed(object sender, TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle(blueStyle);
            e.ChangedRange.SetStyle(blueStyle, m_urlRegex);

            e.ChangedRange.ClearStyle(greenStyle);
            e.ChangedRange.SetStyle(greenStyle, m_datetimeRegex);

            e.ChangedRange.ClearStyle(yellowStyle);
            e.ChangedRange.SetStyle(yellowStyle, m_httpVerbRegex);

            e.ChangedRange.ClearStyle(redStyle);
            e.ChangedRange.SetStyle(redStyle, m_httpStatusRegex);
        }

        private void AppendText(ProcfileEntry entry, string strText)
        {
            if (strText == null)
                return;

            FastColoredTextBox txtConsole = null;
            bool hasKey = m_consoles.TryGetValue(entry.Id, out txtConsole);
            if (!hasKey)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker) delegate
                {
                    this.AppendText(strText, txtConsole);
                });
            } else
            {
                this.AppendText(strText, txtConsole);
            }
        }

        private void AppendText(string strText, FastColoredTextBox txtConsole)
        {

            foreach (string strLine in strText.Split('\n'))
            {
                //var text = String.Format(@"{{\rtf1\ansi {0} {1} {2}\line}}", ColorTable(), strHeader, strLine);
                //txtConsole.Select(txtConsole.TextLength, 0);
                //txtConsole.SelectedRtf = text;
                txtConsole.AppendText($"{strLine}\n");
            }

            //txtConsole.SelectionStart = txtConsole.Text.Length;
            //txtConsole.ScrollToCaret();
        }

        private void stopStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_objProcfile != null)
            {
                m_objProcfile.Stop();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            Process.GetCurrentProcess().KillAllSubProcesses();
        }

        private void tbiStart_Click(object sender, EventArgs e)
        {
            this.ExecuteAction(ToolbarAction.Start);
        }

        private void tbiStop_Click(object sender, EventArgs e)
        {
            this.ExecuteAction(ToolbarAction.Stop);
        }

        private void tbiInfo_Click(object sender, EventArgs e)
        {
            this.ExecuteAction(ToolbarAction.Info);
        }

        private void tbiClear_Click(object sender, EventArgs e)
        {
            this.ExecuteAction(ToolbarAction.Clear);
        }

        private void ExecuteAction(ToolbarAction action)
        {
            var active = tabControl.Controls.Count > 0;
            active = active && m_processes.Count > 0;

            if (!active)
            {
                return;
            }

            int index = tabControl.SelectedIndex;

            Control tabPage = tabControl.Controls[index];
            string id = (string) tabPage.Tag;
            ProcfileEntry proc = null;
            bool hasKey = m_processes.TryGetValue(id, out proc);
            if (!hasKey)
            {
                return;
            }

            if (action == ToolbarAction.Start)
            {
                proc.Start();
            } else if(action == ToolbarAction.Stop)
            {
                proc.Stop();
            } else if(action == ToolbarAction.Info)
            {
                SplitContainer splitContainer = (SplitContainer)Controls.Find("s" + id, true)[0];
                splitContainer.Panel1Collapsed = !splitContainer.Panel1Collapsed;
            }
            {
                FastColoredTextBox console;
                hasKey = m_consoles.TryGetValue(id, out console);
                if (hasKey)
                {
                    console.Clear();
                }
            }

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            var active = tabControl.Controls.Count > 0;
            active = active && m_processes.Count > 0;

            if (active)
            {
                int index = tabControl.SelectedIndex;
                Control tabPage = tabControl.Controls[index];
                string id = tabPage.Tag?.ToString();
                active = id != null;

                if (active)
                {
                    ProcfileEntry proc = null;
                    bool hasKey = m_processes.TryGetValue(id, out proc);
                    if (hasKey)
                    {
                        tbiStart.Enabled = !proc.Active;
                        tbiStop.Enabled = proc.Active;
                    }
                }


            }

            toolBar.Enabled = active;
        }

        private void systrayIcon_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            } else
            {
                this.ShowInTaskbar = true;
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode.ToString();
            if (e.Control == true &&  key == "L")
            {
                int index = tabControl.SelectedIndex;
                Control tabPage = tabControl.Controls[index];
                string id = tabPage.Tag.ToString();

                FastColoredTextBox console;
                var hasKey = m_consoles.TryGetValue(id, out console);
                if (hasKey)
                {
                    console.Clear();
                }
            }
        }

    }
}
