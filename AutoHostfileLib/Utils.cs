using System;
using System.Threading;

namespace AutoHostfileLib
{
    public static class Utils
    {
        internal static string GetDefaultFriendlyHostname()
        {
            // or alternately System.Net.Dns.GetHostName();
            return System.Environment.GetEnvironmentVariable("COMPUTERNAME");
        }

        public static void WaitForKeyPress()
        {
            Console.WriteLine("Press any key to quit..");
            while (!Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }
        }
    }
}
