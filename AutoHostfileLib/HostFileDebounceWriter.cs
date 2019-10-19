using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutoHostfileLib
{
    internal class HostFileDebounceWriter
    {
        public Dictionary<string, HostEntry> HostnameToAddress = new Dictionary<string, HostEntry>();
        private Debouncer debouncer = new Debouncer(3000);

        internal HostFileDebounceWriter()
        {
            debouncer.DebounceFiredEvent += WriteFile;
        }

        internal void AddMapping(string hostname, string address)
        {
            lock (this)
            {
                if (HostnameToAddress.ContainsKey(hostname) && HostnameToAddress[hostname].Address == address)
                {
                    // We already have this mapping
                    return;
                }

                // Check the reverse mapping to see if we're updating an existing address
                HostnameToAddress = HostnameToAddress.Where(pair => pair.Value.Address != address)
                                                     .ToDictionary(pair => pair.Key,
                                                                   pair => pair.Value);

                Logger.Debug("Adding mapping {0} -> {1}", hostname, address);
                HostnameToAddress[hostname] = new HostEntry(hostname, address);

                // Write the hosts file once events have gone quiet
                debouncer.Trigger();
            }
        }

        internal void WriteFile()
        {
            var hostsFile = new HostsFileParser(HostnameToAddress);
            hostsFile.Rewrite();
        }
    }
}
