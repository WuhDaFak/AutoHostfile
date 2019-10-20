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
        private List<MultiInterfaceUdpClient> _udpClients = new List<MultiInterfaceUdpClient>();
        private Dictionary<MultiInterfaceUdpClient, string> _clientToAddress = new Dictionary<MultiInterfaceUdpClient, string>();
        private int _port;

        internal UdpBroadcaster(int port)
        {
            this._port = port;
            Populate();
        }

        private void Populate()
        {
            _udpClients.Clear();

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
                        _clientToAddress.Add(client, address.Address.ToString());
                        _udpClients.Add(client);
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
            var endpoint = new IPEndPoint(IPAddress.Broadcast, _port);

            foreach(var client in _udpClients)
            {
                string toSend = str.Replace("<LOCALIP>", _clientToAddress[client]);

                var data = TrafficEncryptor.Instance.Encrypt(toSend);
                client.Send(data, data.Length, endpoint);
            }
        }
    }
}
