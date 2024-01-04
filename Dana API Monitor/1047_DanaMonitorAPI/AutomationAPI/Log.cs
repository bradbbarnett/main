using System;
using System.IO;
using System.Net.NetworkInformation;

namespace AutomationAPI
{
    static class Log
    {
        public static void Update(string text)
        {
            string file = @"C:\Automation Service\log.txt";
            string date = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss:fftt");
            File.AppendAllText(file, date + " | " + text + "\n");
        }

        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }
    }
}
