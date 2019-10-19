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

        private const int DebouncePeriod = 3000;
        private Debouncer NetworkChangeDebouncer = new Debouncer(DebouncePeriod);

        public NetworkEventSink()
        {
            NetworkChangeDebouncer.DebounceFiredEvent += DebounceFired;

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
            NetworkChangeDebouncer.Trigger();
        }

        private void OnNetworkAddressChanged(object sender, EventArgs e)
        {
            NetworkChangeDebouncer.Trigger();
        }

        private void OnNetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            NetworkChangeDebouncer.Trigger();
        }
    }
}