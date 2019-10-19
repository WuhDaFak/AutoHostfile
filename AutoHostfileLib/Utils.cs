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
using System.Threading;

namespace AutoHostfileLib
{
    public static class Utils
    {
        internal static string GetDefaultFriendlyHostname()
        {
            // or alternately System.Net.Dns.GetHostName();
            return System.Environment.GetEnvironmentVariable("COMPUTERNAME");
        }

        public static void WaitForKeyPress()
        {
            Console.WriteLine("Press any key to quit..");
            while (!Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }
        }
    }
}
