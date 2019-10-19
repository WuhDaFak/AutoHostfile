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
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutoHostfileLib
{
    internal class UdpListener
    {
        private int Port;
        private EventHandler Handler;

        internal UdpListener(int port, EventHandler eventHandler)
        {
            Port = port;
            Handler = eventHandler;
            StartListening();
        }

        private void StartListening()
        {
            var from = new IPEndPoint(0, 0);
            Task.Run(() =>
            {
                Logger.Info("Listening on port {0}", Port);
                using (var client = new UdpClient())
                {
                    client.Client.Bind(new IPEndPoint(IPAddress.Any, Port));
                    while (true)
                    {
                        try
                        {
                            var recvBuffer = TrafficEncryptor.Instance.Decrypt(client.Receive(ref from));
                            Handler.OnMessageRecieved(recvBuffer);
                        }
                        catch(CryptographicException)
                        {
                            Logger.Warn("Unexpected message recieved, possibly using different shared key");
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("Listener thread, Exception: {0}", ex);
                        }
                    }
                }
            });
        }
    }
}
