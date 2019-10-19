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
