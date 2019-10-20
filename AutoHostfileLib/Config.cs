//  Copyright (C) 2019 Ben Staniford
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

using AutoHostfile;
using Microsoft.Win32;
using System;

namespace AutoHostfileLib
{
    public sealed class Config
    {
        // Config is a singleton
        private static readonly Lazy<Config> _config = new Lazy<Config>(() => new Config());

        public static Config Instance { get { return _config.Value; } }

        internal const string ServiceName = "AutoHostfileService";

        private const int DefaultPort = 9976;
        private const int DefaultRepollIntervalSecs = (5 * 60);
        private const string DefaultSharedKey = "(DEFAULT)";
        private const string DefaultFriendlyHostname = "(HOSTNAME)";
        private const int DefaultLoggingLevel = (int)Logger.LogLevel.Debug;

        private Config()
        {
        }

        public string GetShortVersion()
        {
            return Common.ShortVersion;
        }

        public string GetLongVersion()
        {
            return Common.LongVersion;
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

        public Logger.LogLevel GetLoggingLevel()
        {
            int loggingLevel = DefaultLoggingLevel;

            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + ServiceName))
            {
                if (key != null)
                {
                    loggingLevel = (int)key.GetValue("LoggingLevel", DefaultLoggingLevel);
                }
            }

            return (Logger.LogLevel)loggingLevel;
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
