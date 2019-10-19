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
using System.Diagnostics;
using System.ServiceProcess;

namespace AutoHostfileService
{
    public partial class AutoHostfileService : ServiceBase
    {
        private EventHandler handler;

        public AutoHostfileService()
        {
            InitializeComponent();
            handler = new EventHandler();
        }

        protected override void OnStart(string[] args)
        {
            handler.OnStartup();
        }

        protected override void OnStop()
        {
            // Ensure auto auto hosts are dead since they may be tailing the log file
            var autoHosts = Process.GetProcessesByName("autohost");
            foreach(var autoHost in autoHosts)
            {
                autoHost.Kill();
            }
        }
    }
}
