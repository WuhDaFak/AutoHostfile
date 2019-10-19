namespace AutoHostfileLib
{
    internal class BroadcastNameMessage : MessageBase
    {
        internal const string TypeString = "BROADCAST_NAME";

        internal string Name { get; private set; }
        internal string Address { get; private set;}

        internal BroadcastNameMessage(string name, string address)
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
            return Type.BroadcastName;
        }
    }
}