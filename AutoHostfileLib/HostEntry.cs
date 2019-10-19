using System.Collections.Generic;

namespace AutoHostfileLib
{
    public class HostEntry
    {
        public string HostName { get; private set; }
        public string Address { get; private set; }

        public HostEntry(string hostname, string address)
        {
            HostName = hostname;
            Address = address;
        }

        public static bool operator ==(HostEntry lhs, HostEntry rhs) => (lhs.Address == rhs.Address && lhs.HostName == rhs.HostName);
        public static bool operator !=(HostEntry lhs, HostEntry rhs) => !(lhs == rhs);

        public override bool Equals(object obj)
        {
            return obj is HostEntry entry &&
                   HostName == entry.HostName &&
                   Address == entry.Address;
        }

        public override int GetHashCode()
        {
            var hashCode = -744324225;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(HostName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Address);
            return hashCode;
        }
    }
}