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
                Logger.Debug("Listening on port {0}", Port);
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
                            Logger.Debug("Unexpected message recieved, possibly using different shared key");
                        }
                        catch (Exception ex)
                        {
                            Logger.Debug("Listener thread, Exception: {0}", ex);
                        }
                    }
                }
            });
        }
    }
}
