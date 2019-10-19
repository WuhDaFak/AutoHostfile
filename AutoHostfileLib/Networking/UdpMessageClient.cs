using AutoHostfileLib.Networking;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace AutoHostfileLib
{
    public class UdpMessageClient
    {
        private int Port;

        public UdpMessageClient(int port)
        {
            this.Port = port;
        }

        private IPAddress GetLocalIP(IPAddress destIp)
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var adapter in interfaces)
            {
                foreach (var address in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                    {
                        // Ignore IPv6 for now
                        continue; 
                    }

                    var ip4Addres = address.Address;
                    if(ip4Addres.IsInSameSubnet(destIp, address.IPv4Mask))
                    {
                        // We favour returning an IP which is on the same subnet as the one
                        // we've heard a broadcast from
                        return ip4Addres;
                    }
                }
            }

            foreach (var adapter in interfaces)
            {
                if (adapter.OperationalStatus == OperationalStatus.Up && adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    foreach (var address in adapter.GetIPProperties().UnicastAddresses)
                    {
                        if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            // Return the first valid internet connect IP4 address we have
                            return address.Address;
                        }
                    }
                }
            }

            throw new InvalidOperationException("No local IP corresponding to destination IP: " + destIp.ToString());
        }

        public void Send(string address, string str)
        {
            var destIp = IPAddress.Parse(address);
            var localIp = GetLocalIP(destIp);
            var destination = new IPEndPoint(destIp, Port);

            string toSend = str.Replace("<LOCALIP>", localIp.ToString());
            var data = TrafficEncryptor.Instance.Encrypt(toSend);

            using (var client = new UdpClient())
            {
                if (client.Send(data, data.Length, destination) != data.Length)
                {
                    throw new InvalidOperationException("Send failed");
                }
            }

            Logger.Debug("Replied to {0} with {1}", destIp, toSend);
        }
    }
}