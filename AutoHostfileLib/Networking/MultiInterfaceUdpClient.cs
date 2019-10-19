using System;
using System.Net;
using System.Net.Sockets;

internal class MultiInterfaceUdpClient : UdpClient
{
    internal MultiInterfaceUdpClient() : base()
    {
        //Calls the protected Client property belonging to the UdpClient base class.
        Socket s = this.Client;
        //Uses the Socket returned by Client to set an option that is not available using UdpClient.
        s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
        s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 0);
    }

    internal MultiInterfaceUdpClient(IPEndPoint ipLocalEndPoint) : base(ipLocalEndPoint)
    {
        //Calls the protected Client property belonging to the UdpClient base class.
        Socket s = this.Client;
        //Uses the Socket returned by Client to set an option that is not available using UdpClient.
        s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
        s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 0);
    }

}
