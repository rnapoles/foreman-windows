﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Foreman
{
    public partial class frmMain : Form
    {
        private Procfile m_objProcfile = null;

        public frmMain()
        {
            InitializeComponent();
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

            txtConsole.Clear();

            m_objProcfile = new Procfile(strFilename);

            m_objProcfile.TextReceived += delegate(ProcfileEntry objEntry, string strText)
            {
                AppendText(objEntry.Header(), strText);
            };

            m_objProcfile.StatusReceived += delegate(string strText)
            {
                AppendText(m_objProcfile.Header(), strText);
            };

            m_objProcfile.Start();
        }

        private void AppendText(string strHeader, string strText)
        {
            if (strText == null)
                return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    foreach (string strLine in strText.Split('\n'))
                    {
                        txtConsole.SelectedRtf = String.Format(@"{{\rtf1\ansi {0} {1} {2}\line}}", ColorTable(), strHeader, strLine);
                    }
                    txtConsole.SelectionStart = txtConsole.Text.Length;
                    txtConsole.ScrollToCaret();
                });
            }
        }

        private string ColorTable()
        {
            return (@"{\colortbl;\red0\green204\blue204;\red255\green255\blue0;}");
        }
    }
}
