using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace AutoHostfileLib
{
    /// <summary>
    /// The UdpBroadcaster can send to all network interfaces
    /// </summary>
    internal class UdpBroadcaster
    {
        private List<MultiInterfaceUdpClient> UdpClients = new List<MultiInterfaceUdpClient>();
        private Dictionary<MultiInterfaceUdpClient, string> ClientToAddress = new Dictionary<MultiInterfaceUdpClient, string>();
        private int Port;

        internal UdpBroadcaster(int port)
        {
            this.Port = port;
            Populate();
        }

        private void Populate()
        {
            UdpClients.Clear();

            foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (var address in adapter.GetIPProperties().UnicastAddresses)
                {
                    try
                    {
                        if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        {
                            // Ignore IPv6
                            continue;
                        }

                        var client = new MultiInterfaceUdpClient();
                        client.Client.Bind(new IPEndPoint(address.Address, 0));
                        ClientToAddress.Add(client, address.Address.ToString());
                        UdpClients.Add(client);
                    }
                    catch(SocketException)
                    {
                        // Suppress, not all interfaces support udp
                    }
                }
            }
        }

        internal void Send(string str)
        {
            var endpoint = new IPEndPoint(IPAddress.Broadcast, Port);

            foreach(var client in UdpClients)
            {
                string toSend = str.Replace("<LOCALIP>", ClientToAddress[client]);

                var data = TrafficEncryptor.Instance.Encrypt(toSend);
                client.Send(data, data.Length, endpoint);
            }
        }
    }
}
