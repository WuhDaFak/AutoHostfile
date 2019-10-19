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

namespace AutoHostfileLib
{
    public static class Messages
    {
        internal static MessageBase ParseMessage(string message)
        {
            var tokens = message.Split(' ');

            if (tokens[0] == BroadcastNameMessage.TypeString)
            {
                return new BroadcastNameMessage(tokens[1], tokens[2]);
            }
            else if (tokens[0] == BroadcastReplyMessage.TypeString)
            {
                return new BroadcastReplyMessage(tokens[1], tokens[2]);
            }

            throw new InvalidOperationException("Unsupported message: " + message);
        }

        internal static MessageBase BuildBroadcastNameMessage()
        {
            return new BroadcastNameMessage(Config.Instance.GetFriendlyHostname(), "<LOCALIP>");
        }

        internal static object BuildBroadcastReplyMessage()
        {
            return new BroadcastReplyMessage(Config.Instance.GetFriendlyHostname(), "<LOCALIP>");
        }
    }
}
