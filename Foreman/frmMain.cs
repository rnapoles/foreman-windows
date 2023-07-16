using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using FastColoredTextBoxNS;

namespace Foreman
{
    public partial class frmMain : Form
    {

        enum ToolbarAction
        {
            Start, Stop, Clear
        }

        private Procfile m_objProcfile = null;
        private Dictionary<string, ProcfileEntry> m_processes = new Dictionary<string, ProcfileEntry>();
        private Dictionary<string, FastColoredTextBox> m_consoles = new Dictionary<string, FastColoredTextBox>();

        public frmMain()
        {
            InitializeComponent();

            toolBar.ImageList = imageList;
            tbiStart.ImageIndex = 0;
            tbiStop.ImageIndex = 1;
            tbiClear.ImageIndex = 2;

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

            tabControl.TabPages.Clear();
            m_consoles.Clear();
            m_processes.Clear();

            m_objProcfile = new Procfile(strFilename);
            m_objProcfile.TextReceived += (ProcfileEntry objEntry, string strText) =>
            {
                //Debug.WriteLine(strText);
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

            foreach(var item in m_objProcfile.ProcfileEntries)
            {
                var richTextBox = new FastColoredTextBox();
                var tabPage = new System.Windows.Forms.TabPage();

                richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
                //txtConsole.Name = "txtConsole";
                richTextBox.Text = "";
                richTextBox.ForeColor = Color.White;
                richTextBox.BackColor = Color.Black;

                //tabPage.SuspendLayout();
                //tabPage.BackColor = Color.Violet;
                var name = item.Name;
                tabPage.Text = name;
                tabPage.Controls.Add(richTextBox);
                m_consoles.Add(name, richTextBox);
                m_processes.Add(name, item);
                tabControl.Controls.Add(tabPage);
            }

            m_objProcfile.Start();
        }

        private void AppendText(ProcfileEntry entry, string strText)
        {
            if (strText == null)
                return;

            Debug.WriteLine($"===> {strText}");

            FastColoredTextBox txtConsole = null;
            bool hasKey = m_consoles.TryGetValue(entry.Name, out txtConsole);
            if (!hasKey)
            {
                return;
            }

            var strHeader = entry.Header();

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
            Debug.WriteLine($"**** {strText}");
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

        private string ColorTable()
        {
            return (@"{\colortbl;\red0\green204\blue204;\red255\green255\blue0;\red0\green204\blue0;\red204\green0\blue204;\red204\green0\blue0;}");
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
            /*if (m_objProcfile != null)
            {
                m_objProcfile.Stop();
            }*/

            Application.Exit();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            Process.GetCurrentProcess().KillAllSubProcesses();
        }

        private void tabControl_TabIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(tabControl.TabPages[tabControl.TabIndex].Name);
        }

        private void tbiStart_Click(object sender, EventArgs e)
        {
            this.executeAction(ToolbarAction.Start);
        }

        private void tbiStop_Click(object sender, EventArgs e)
        {
            this.executeAction(ToolbarAction.Stop);
        }

        private void tbiClear_Click(object sender, EventArgs e)
        {
            this.executeAction(ToolbarAction.Clear);
        }

        private void executeAction(ToolbarAction action)
        {
            var active = tabControl.TabPages.Count > 0;
            active = active && m_processes.Count > 0;

            if (!active)
            {
                return;
            }

            int index = tabControl.SelectedIndex;
            TabPage tabPage = tabControl.TabPages[index];
            string text = tabPage.Text;
            ProcfileEntry proc = null;
            bool hasKey = m_processes.TryGetValue(text, out proc);
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
            } else
            {
                FastColoredTextBox console;
                hasKey = m_consoles.TryGetValue(text, out console);
                if (hasKey)
                {
                    console.Clear();
                }
            }

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            var active = tabControl.TabPages.Count > 0;
            active = active && m_processes.Count > 0;

            if (active)
            {
                int index = tabControl.SelectedIndex;
                TabPage tabPage = tabControl.TabPages[index];
                string text = tabPage.Text;
                ProcfileEntry proc = null;
                bool hasKey = m_processes.TryGetValue(text, out proc);
                if (hasKey)
                {
                    tbiStart.Enabled = !proc.Active;
                    tbiStop.Enabled = proc.Active;
                }
            }

            toolBar.Enabled = active;


            //Text = $"{counter++}";

        }

    }
}
