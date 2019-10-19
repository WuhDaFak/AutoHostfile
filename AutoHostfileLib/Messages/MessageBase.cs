using System;

namespace AutoHostfileLib
{
    public abstract class MessageBase
    {
        internal enum Type { BroadcastName, BroadcastReply };

        internal abstract string GetMessageString();

        internal abstract Type GetMessageType();

        public override string ToString()
        {
            return GetMessageString();
        }
    }
}