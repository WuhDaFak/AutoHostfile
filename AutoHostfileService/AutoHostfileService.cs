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
