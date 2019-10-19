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
