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
