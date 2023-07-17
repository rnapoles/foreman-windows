using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Foreman.Service
{

    internal class PortFinderService
    {
        private static Dictionary<int, bool> m_ports = new();
        private static Object m_lockObj = new Object();

        public static int GetPort(int port = 5000)
        {

            lock (m_lockObj)
            {
                while (IsPortOpen("localhost", port))
                {
                    port++;
                }

                ReservePort(port);
            }

            return port;
        }

        public static void ReservePort(int port)
        {
            lock (m_lockObj)
            {
                if (m_ports.ContainsKey(port))
                {
                    return;
                }
                m_ports.Add(port, true);
            }

        }

        public static void ReleasePort(int port)
        {
            lock (m_lockObj)
            {
                if (!m_ports.ContainsKey(port))
                {
                    return;
                }
                m_ports.Remove(port);
            }
        }

        public static bool IsPortOpen(string host, int port)
        {

            lock (m_lockObj)
            {
                if (m_ports.ContainsKey(port))
                {
                    return false;
                }
            }

            try
            {
                using (var client = new TcpClient())
                {
                    client.Connect(host, port);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
