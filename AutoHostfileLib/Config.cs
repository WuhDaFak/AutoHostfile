using Microsoft.Win32;
using System;

namespace AutoHostfileLib
{
    public sealed class Config
    {
        // Config is a singleton
        private static readonly Lazy<Config> config = new Lazy<Config>(() => new Config());
        public static Config Instance { get { return config.Value; } }

        internal const string ServiceName = "AutoHostfileService";

        private const int DefaultPort = 9976;
        private const int DefaultRepollIntervalSecs = (5 * 60);
        private const string DefaultSharedKey = "(DEFAULT)";
        private const string DefaultFriendlyHostname = "(HOSTNAME)";

        private Config()
        {
        }

        public int GetPort()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + ServiceName))
            {
                if (key != null)
                {
                    return (int)key.GetValue("Port", DefaultPort);
                }
            }

            return DefaultPort;
        }

        public int GetRepollIntervalSecs()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + ServiceName))
            {
                if (key != null)
                {
                    return (int)key.GetValue("RepollIntervalSecs", DefaultRepollIntervalSecs);
                }
            }

            return DefaultRepollIntervalSecs;
        }

        public string GetSharedKey()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + ServiceName))
            {
                if (key != null)
                {
                    return (string)key.GetValue("SharedKey", DefaultSharedKey);
                }
            }

            return DefaultSharedKey;
        }

        public string GetFriendlyHostname()
        {
            string friendlyHostname = DefaultFriendlyHostname;

            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + ServiceName))
            {
                if (key != null)
                {
                    friendlyHostname = (string)key.GetValue("FriendlyHostname", DefaultFriendlyHostname);
                }
            }

            return friendlyHostname.Replace(DefaultFriendlyHostname, Utils.GetDefaultFriendlyHostname());
        }

        public void SetFriendlyHostname(string friendlyName)
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + ServiceName, true))
            {
                if (key != null)
                {
                    key.SetValue("FriendlyHostname", friendlyName, RegistryValueKind.String);
                }
            }
        }

        public void SetSharedKey(string sharedKey)
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + ServiceName, true))
            {
                if (key != null)
                {
                    key.SetValue("SharedKey", sharedKey, RegistryValueKind.String);
                }
            }
        }

        public void SetPort(int port)
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + ServiceName, true))
            {
                if (key != null)
                {
                    key.SetValue("Port", port, RegistryValueKind.DWord);
                }
            }
        }
    }
}