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
using NetFwTypeLib;

namespace AutoHostfileLib
{
    internal class Firewall
    {
        internal Firewall()
        {
        }

        internal void Configure(int port)
        {
            INetFwMgr icfMgr = null;
            try
            {
                Type TicfMgr = Type.GetTypeFromProgID("HNetCfg.FwMgr");
                icfMgr = (INetFwMgr)Activator.CreateInstance(TicfMgr);
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to get firewall COM object: {0}", ex.Message);
                return;
            }

            try
            {
                INetFwProfile profile;
                INetFwOpenPort portClass;
                Type TportClass = Type.GetTypeFromProgID("HNetCfg.FWOpenPort");
                portClass = (INetFwOpenPort)Activator.CreateInstance(TportClass);

                // Get the current profile
                profile = icfMgr.LocalPolicy.CurrentProfile;

                // Set the port properties
                portClass.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
                portClass.Enabled = true;
                portClass.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP;
                portClass.Name = "AutoHostfileService";
                portClass.Port = port;

                // Add the port to the ICF Permissions List
                profile.GloballyOpenPorts.Add(portClass);
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to add firewall rule: {0}", ex.Message);
            }

            return;
        }
    }
}
