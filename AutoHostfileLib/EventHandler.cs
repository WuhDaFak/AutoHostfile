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

using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Timers;

namespace AutoHostfileLib
{
    public class EventHandler
    {
        HostFileDebounceWriter HostFile = new HostFileDebounceWriter();
        private NetworkEventSink NetEventSink = new NetworkEventSink();
        private Timer RepollTimer;

        public EventHandler()
        {
            RepollTimer = new Timer(Config.Instance.GetRepollIntervalSecs() * 1000);
            RepollTimer.Elapsed += OnRepoll;
            RepollTimer.AutoReset = true;
            RepollTimer.Enabled = true;

            NetEventSink.NetworkSinkDebounceEvent += OnNetworkChanged;
        }

        public void OnStartup()
        {
            var config = Config.Instance;
            Logger.Info("AutoHostfile starting for {0}, version: {1}", config.GetFriendlyHostname(), config.GetShortVersion());

            var port = Config.Instance.GetPort();

            // Add inbound rules to the filewall
            Firewall firewall = new Firewall();
            firewall.Configure(port);

            // Start listening for broadcasts
            UdpListener listener = new UdpListener(port, this);

            OnRepoll(this, null);
        }

        internal void OnRepoll(Object source, ElapsedEventArgs e)
        {
            Logger.Info("Sending broadcast on port {0}", Config.Instance.GetPort());

            // Discover new hosts
            UdpBroadcaster broadcaster = new UdpBroadcaster(Config.Instance.GetPort());
            broadcaster.Send(Messages.BuildBroadcastNameMessage().ToString());
        }

        internal void OnNetworkChanged()
        {
            bool networkUp = false;
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            Logger.Info("Network changed event has occured, interface count = {0}", interfaces.Length);
            foreach (NetworkInterface nic in interfaces)
            {
                Logger.Debug("   {0} is {1}", nic.Name, nic.OperationalStatus);
                if (nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    networkUp = true;
                }
            }

            RepollTimer.Stop();

            if (networkUp)
            {
                // The network configuration has changed, time to discover new hosts
                OnRepoll(this, null);

                // Restart the repoll timer
                RepollTimer.Start();
            }
        }

        internal void OnMessageRecieved(string messageStr)
        {
            var message = Messages.ParseMessage(messageStr);

            switch (message.GetMessageType())
            {
                case MessageBase.Type.Broadcast:
                    OnBroadcastRecieved((BroadcastMessage)message);
                    break;
                case MessageBase.Type.Ping:
                    OnPingRecieved((PingMessage)message);
                    break;
                case MessageBase.Type.Pong:
                    OnPongRecieved((PongMessage)message);
                    break;
                default:
                    throw new ArgumentException("Unsupported message: " + messageStr);
            }
        }

        internal void OnBroadcastRecieved(BroadcastMessage message)
        {
            if (message.Name == Config.Instance.GetFriendlyHostname())
            {
                // Ignore broadcasts from ourselves
                return;
            }

            Logger.Debug("Recieved BROADCAST from {0}({1})", message.Address, message.Name);

            // Ping back to the broadcaster, so they know we're alive
            var client = new UdpMessageClient(Config.Instance.GetPort());
            client.Send(message.Address, Messages.BuildPingMessage().ToString());
        }

        private void OnPingRecieved(PingMessage message)
        {
            Logger.Debug("Recieved PING from {0}({1})", message.Address, message.Name);
            
            // Add the reply to our hosts file
            HostFile.AddMapping(message.Name, message.Address);

            try
            {
                // Send the pong message, we do this as an extra hand shake since sometimes we're told
                // about a secondary address which only route in one direction
                var client = new UdpMessageClient(Config.Instance.GetPort());
                client.Send(message.Address, Messages.BuildPongMessage().ToString());
            }
            catch(SocketException ex)
            {
                Logger.Warn("Address {0} is unroutable", message.Address);
            }
        }

        private void OnPongRecieved(PongMessage message)
        {
            Logger.Debug("Recieved PONG from {0}({1})", message.Address, message.Name);
            HostFile.AddMapping(message.Name, message.Address);
        }
    }
}
