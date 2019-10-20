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

using System.Collections.Generic;
using System.Linq;

namespace AutoHostfileLib
{
    internal class HostFileDebounceWriter
    {
        public Dictionary<string, HostEntry> HostnameToAddress = new Dictionary<string, HostEntry>();

        private Debouncer _debouncer = new Debouncer(3000);

        internal HostFileDebounceWriter()
        {
            _debouncer.DebounceFiredEvent += WriteFile;
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

                Logger.Info("Adding mapping {0} -> {1}", hostname, address);
                HostnameToAddress[hostname] = new HostEntry(hostname, address);

                // Write the hosts file once events have gone quiet
                _debouncer.Trigger();
            }
        }

        internal void WriteFile()
        {
            var hostsFile = new HostsFileParser(HostnameToAddress);
            hostsFile.Rewrite();
        }
    }
}
