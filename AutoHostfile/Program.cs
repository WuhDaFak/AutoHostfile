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

using AutoHostfileLib;

namespace AutoHostfile
{
    class Program
    {
        // https://starbeamrainbowlabs.com/blog/article.php?article=posts/200-Picking-the-right-interface-multicast.html
        // https://stackoverflow.com/questions/1096142/broadcasting-udp-message-to-all-the-available-network-cards

        // Tutorial on developing Wix installer for Windows service
        // https://developingsoftware.com/wix-toolset-install-windows-service/

        static void Main(string[] args)
        {
            // We'll want to log to stdout since we're a normal binary
            Logger.LogStdout = true;

            var handler = new EventHandler();
            handler.OnStartup();
            Utils.WaitForKeyPress();
        }
    }
}
