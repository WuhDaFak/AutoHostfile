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

namespace AutoHostfileLib
{
    internal class BroadcastReplyMessage : MessageBase
    {
        internal const string TypeString = "BROADCAST_REPLY";
        internal string Name { get; private set; }
        internal string Address { get; private set;}

        internal BroadcastReplyMessage(string name, string address)
        {
            this.Name = name;
            this.Address = address;
        }

        internal override string GetMessageString()
        {
            return string.Format("{0} {1} {2}", TypeString, Name, Address);
        }

        internal override Type GetMessageType()
        {
            return Type.BroadcastReply;
        }
    }
}
