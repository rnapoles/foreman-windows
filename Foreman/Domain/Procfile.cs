using DotEnv.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;


namespace Foreman.Domain
{
    class Procfile
    {
        public delegate void TextReceivedHandler(ProcfileEntry objEntry, string strText);
        public delegate void StatusRecievedHandler(string strText);

        private string m_strFilename = null;
        private List<ProcfileEntry> m_arrProcfileEntries = null;
        private bool m_blnStarted = false;
        private Dictionary<string, string> m_envVariables = new();

        public Procfile(string strFilename)
        {
            m_strFilename = strFilename;
            m_arrProcfileEntries = new List<ProcfileEntry>();
            this.OpenEnvFile(strFilename);

            int intCurrent = 1;
            string strContents = File.ReadAllText(strFilename);
            char[] charSeparators = new char[] { ':' };

            foreach (string strLine in strContents.Split('\n'))
            {

                if (strLine.StartsWith("#"))
                {
                    continue;
                }

                string[] arrLine = strLine.Split(charSeparators, 2);
                if (arrLine.Length >= 2)
                {
                    
                    ProcfileEntry objProcfileEntry = new ProcfileEntry(this, intCurrent, arrLine[0].Trim(), arrLine[1].Trim(), m_envVariables);
                    objProcfileEntry.TextReceived += delegate(ProcfileEntry objEntry, string strData)
                    {
                        if(TextReceived != null)
                        {
                            TextReceived(objEntry, strData);
                        }
                    };
                    m_arrProcfileEntries.Add(objProcfileEntry);
                    intCurrent += 1;
                }
            }
        }

        private void OpenEnvFile(string strFilename)
        {
            var directory = new FileInfo(strFilename).DirectoryName + Path.DirectorySeparatorChar;
            List<string> routes = new() { ".env", ".env.dev.local", ".env.local" };
            var envLoader = new EnvLoader().AllowOverwriteExistingVars();
            m_envVariables.Clear();

            foreach(string route in routes)
            {
                var path = directory + route;
                envLoader
                   .AddEnvFile(path);
            }

            var environmentVariables = envLoader.Load();
            if (environmentVariables.Count() > 0)
            {
                m_envVariables = environmentVariables.ToDictionary();
            }


        }

        public string Header()
        {
            return (String.Format(@"{{{0} {1,-" + LongestNameLength() + "} |}} ", "00:00:00", "system"));
        }

        public void Start()
        {
            if (m_blnStarted)
                return;

            m_blnStarted = true;

            foreach (ProcfileEntry objProcfileEntry in m_arrProcfileEntries)
            {
                Thread objThread = new Thread(new ThreadStart(objProcfileEntry.Start));
                objThread.Start();
            }
        }

        public void Stop()
        {
            if (!m_blnStarted)
                return;

            StatusReceived("Stopping all processes");

            m_blnStarted = false;

            foreach (ProcfileEntry objProcfileEntry in m_arrProcfileEntries)
            {
                objProcfileEntry.Stop();
            }
        }

        public void Info(ProcfileEntry objEntry, string strText)
        {
            if(TextReceived != null)
            {
                TextReceived(objEntry, strText);
            }
        }

        public int LongestNameLength()
        {
            int intLongestName = 0;
            if (m_arrProcfileEntries.Count > 0)
            {
                intLongestName = m_arrProcfileEntries.Select(objEntry => objEntry.Name.Length).Max();
            }

            return ((intLongestName > 6) ? intLongestName : 6);
        }

        public event TextReceivedHandler TextReceived;

        public event StatusRecievedHandler StatusReceived;

        public string FileName
        {
            get
            {
                return (m_strFilename);
            }
        }

        public IReadOnlyList<ProcfileEntry> ProcfileEntries
        {
            get { return m_arrProcfileEntries.AsReadOnly(); }
        }
    }
}
