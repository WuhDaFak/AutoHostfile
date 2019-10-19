using System;
using System.Net.NetworkInformation;
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
            Logger.Debug("Sending broadcast on port {0}", Config.Instance.GetPort());

            // Discover new hosts
            UdpBroadcaster broadcaster = new UdpBroadcaster(Config.Instance.GetPort());
            broadcaster.Send(Messages.BuildBroadcastNameMessage().ToString());
        }

        internal void OnNetworkChanged()
        {
            bool networkUp = false;
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            Logger.Debug("Network changed event has occured, interface count = {0}", interfaces.Length);
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
                case MessageBase.Type.BroadcastName:
                    OnBroadcastNameRecieved((BroadcastNameMessage)message);
                    break;
                case MessageBase.Type.BroadcastReply:
                    OnBroadcastReplyRecieved((BroadcastReplyMessage)message);
                    break;
                default:
                    throw new ArgumentException("Unsupported message: " + messageStr);
            }
        }

        internal void OnBroadcastNameRecieved(BroadcastNameMessage message)
        {
            if (message.Name == Config.Instance.GetFriendlyHostname())
            {
                // Ignore broadcasts from ourselves
                return;
            }

            Logger.Debug("Broadcast name recieved from: {0} {1}", message.Name, message.Address);

            // Reply so the broadcaster knows about us
            var client = new UdpMessageClient(Config.Instance.GetPort());
            client.Send(message.Address, Messages.BuildBroadcastReplyMessage().ToString());

            // Add the broadcaster to our own hosts file
            HostFile.AddMapping(message.Name, message.Address);
        }

        private void OnBroadcastReplyRecieved(BroadcastReplyMessage message)
        {
            Logger.Debug("Broadcast reply recieved from: {0} {1}", message.Name, message.Address);
            
            // Add the reply to our hosts file
            HostFile.AddMapping(message.Name, message.Address);
        }
    }
}
