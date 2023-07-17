using System;
using System.Diagnostics;
using System.Management;

namespace Foreman.Infrastructure
{
    public static class ProcessExtensions
    {

        public static void KillAllSubProcesses(this Process process)
        {

            if(process is null)
            {
                return;
            }

            if (process.HasExited)
            {
                return;
            }

            int parentProcessId = process.Id;

            //Console.WriteLine($"Finding processes spawned by process {process.ProcessName}[{parentProcessId}]");

            // NOTE: Process Ids are reused!
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                "SELECT * " +
                "FROM Win32_Process " +
                "WHERE ParentProcessId=" + parentProcessId);
            ManagementObjectCollection collection = searcher.Get();
            if (collection.Count > 0)
            {
                //Console.WriteLine("Killing [" + collection.Count + "] processes spawned by process with Id [" + process.ProcessName + "]");
                foreach (var item in collection)
                {
                    UInt32 childProcessId = (UInt32)item["ProcessId"];
                    if ((int)childProcessId != Process.GetCurrentProcess().Id)
                    {

                        Process childProcess = null;
                        try
                        {
                            childProcess = Process.GetProcessById((int)childProcessId);
                        }
                        catch (Exception)
                        {

                        }

                        //Console.WriteLine("Killing child process [" + childProcess.ProcessName + "] with Id [" + childProcessId + "]");
                        if(childProcess != null)
                        {
                            childProcess.KillAllSubProcesses();
                            if (!childProcess.HasExited)
                            {
                                childProcess.Kill();
                            }
                        }

                    }
                }
            }
        }

    }
}
