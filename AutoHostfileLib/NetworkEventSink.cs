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

using Microsoft.Win32;
using System;
using System.Net.NetworkInformation;

namespace AutoHostfileLib
{
    public delegate void NetworkSinkEventHandler();

    internal class NetworkEventSink
    {
        /// <summary>
        /// This event handler will fire when the debounce period expires caused by any of the network/power events
        /// </summary>
        public event NetworkSinkEventHandler NetworkSinkDebounceEvent;

        private const int _debouncePeriod = 3000;
        private Debouncer _networkChangeDebouncer = new Debouncer(_debouncePeriod);

        public NetworkEventSink()
        {
            _networkChangeDebouncer.DebounceFiredEvent += DebounceFired;

            // Register for network changed events
            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(OnNetworkAvailabilityChanged);
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(OnNetworkAddressChanged);
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(OnPowerModeChanged);
        }

        private void DebounceFired()
        {
            // Call our own event handler in turn
            NetworkSinkDebounceEvent.Invoke();
        }

        private void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            Logger.Debug("Power event triggered");
            _networkChangeDebouncer.Trigger();
        }

        private void OnNetworkAddressChanged(object sender, EventArgs e)
        {
            _networkChangeDebouncer.Trigger();
        }

        private void OnNetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            _networkChangeDebouncer.Trigger();
        }
    }
}
