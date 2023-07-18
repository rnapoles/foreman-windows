using Foreman.Infrastructure;
using Foreman.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Foreman.Domain
{
    class ProcfileEntry
    {
        private Procfile m_objProcfile = null;

        private int m_port = 0;

        private int m_intIndex = 0;

        private string m_strName = null;

        private string m_strCommand = null;

        private Process m_objProcess = null;

        public event Procfile.TextReceivedHandler TextReceived;

        private Dictionary<string, string> m_envVariables = new();

        public bool Active { private set; get; }

        public string Id { private set; get; }

        public string Name
        {
            get
            {
                return m_strName;
            }
        }

        public Dictionary<string, string> EnvVariables
        {
            get
            {
                return m_envVariables;
            }
        }

        public ProcfileEntry(Procfile objProcfile, int intIndex, string strName, string strCommand, Dictionary<string, string> envVariables)
        {

            this.Id = Guid.NewGuid().ToString();

            m_objProcfile = objProcfile;
            m_intIndex = intIndex;
            m_strName = strName;
            m_strCommand = strCommand;
            m_envVariables = envVariables;

            if (!envVariables.ContainsKey("PORT"))
            {
                m_strCommand = m_strCommand.Replace("$PORT", Port().ToString());
            } else
            {
                int port = 0;
                bool result = int.TryParse(envVariables["PORT"], out port);

                if (result)
                {
                    port = Port(port);
                    m_strCommand = m_strCommand.Replace("$PORT", port.ToString());
                }
            }

            m_strCommand = ReplaceVariables(envVariables, m_strCommand);
        }

        public string ReplaceVariables(Dictionary<string, string> dictionary, string text)
        {
            foreach (var kvp in dictionary)
            {
                text = text.Replace($"${kvp.Key}", kvp.Value);
            }
            return text;
        }

        public int Port(int port = 5000)
        {

            port = (port + (100 * (m_intIndex - 1)));
            port = PortFinderService.GetPort(port);
            this.m_port = port;

            return port;
        }

        public void Start()
        {

            string[] parts = m_strCommand.Split(' ');
            string fileName = parts.First();
            string arguments = string.Join(" ", parts.Skip(1));
            string directory = new FileInfo(m_objProcfile.FileName).DirectoryName;
            string fullPath = this.WhereIs(fileName, directory);

            if (fullPath is null)
            {
                TextReceived(this, "Not found: " + fileName);
                return;
            }

            fileName = fullPath;

            m_objProcess = new Process();

            if (m_envVariables.Count > 0)
            {
                foreach (var variable in m_envVariables)
                {
                    var key = variable.Key;
                    var value = variable.Value;

                    var environmentVariables = m_objProcess.StartInfo.EnvironmentVariables;
                    if (!environmentVariables.ContainsKey(key))
                    {
                        environmentVariables.Add(key, value);
                    } else
                    {
                        environmentVariables[key] = value;
                    }

                    //TextReceived(this, $"{key} = {value}");
                }
            }

            m_objProcess.StartInfo.CreateNoWindow = true;
            m_objProcess.StartInfo.UseShellExecute = false;
            m_objProcess.StartInfo.RedirectStandardOutput = true;
            m_objProcess.StartInfo.RedirectStandardError = true;

            //m_objProcess.StartInfo.FileName = "cmd.exe";
            //m_objProcess.StartInfo.Arguments = "/interactive /c " + m_strCommand;
            m_objProcess.StartInfo.FileName = fileName;
            m_objProcess.StartInfo.Arguments = arguments;
            m_objProcess.StartInfo.WorkingDirectory = directory;

            m_objProcess.EnableRaisingEvents = true;
            m_objProcess.OutputDataReceived += DataReceived;
            m_objProcess.ErrorDataReceived += DataReceived;
            m_objProcess.Exited += ProcessExited;

            TextReceived(this, "Starting: " + m_strCommand);

            m_objProcess.Start();
            //Process process = m_objProcess.Parent();
            //TextReceived(this, "Parent: " + process.ProcessName);

            m_objProcess.BeginOutputReadLine();
            m_objProcess.BeginErrorReadLine();
            this.Active = !m_objProcess.HasExited;

        }

        public void Stop()
        {

            if(m_objProcess is null)
            {
                return;
            }

            m_objProcess.KillAllSubProcesses();
            if (!m_objProcess.HasExited)
            {
                m_objProcfile.Info(this, "Stopping process");
                m_objProcess.Kill();
            }
        }

        public string WhereIs(string command, string directory)
        {

            if (File.Exists(command))
            {
                return command;
            }

            string paths = directory + ';' + Environment.GetEnvironmentVariable("PATH");
            string[] folders = paths.Split(';');

            List<string> names = new List<string>();
            names.Add(command);

            if(!(command.EndsWith(".exe") || command.EndsWith(".bat")))
            {
                names.Add($"{command}.bat");
                names.Add($"{command}.exe");
            }

            foreach(string name in names)
            {
                foreach (string folder in folders)
                {

                    if (!Directory.Exists(folder))
                    {
                        continue;
                    }

                    string path = Path.Combine(folder, name); ;
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }
            }
 

            return null;
        }

        private void DataReceived(object objSender, DataReceivedEventArgs args)
        {
            TextReceived(this, args.Data);
        }

        private void ProcessExited(object objSender, EventArgs args)
        {
            this.Active = false;
            PortFinderService.ReleasePort(this.m_port);
            m_objProcfile.Info(this, "Process terminated");
        }
    }
}
