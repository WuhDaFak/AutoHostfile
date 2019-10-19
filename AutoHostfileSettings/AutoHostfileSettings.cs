using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Windows.Forms;
using AutoHostfileLib;

namespace AutoHostfileSettings
{
    public partial class AutoHostfileSettingsForm : Form
    {
        public AutoHostfileSettingsForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var config = Config.Instance;

            // TODO These require validated text fields
            config.SetFriendlyHostname(txtFriendlyName.Text);
            int port = int.Parse(txtPort.Text);
            config.SetPort(port);
            config.SetSharedKey(txtSharedKey.Text);

            RestartService("AutoHostfileService");

            Close();
        }

        public static void RestartService(string serviceName, int timeoutMilliseconds = 10000)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                MessageBox.Show("Configuration loaded successfully", "Service restarted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Failed to restart service", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AutoHostfileSettingsForm_Load(object sender, EventArgs e)
        {
            var config = Config.Instance;

            txtFriendlyName.Text = config.GetFriendlyHostname();
            txtPort.Text = config.GetPort().ToString();
            txtSharedKey.Text = config.GetSharedKey();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLblSupport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://paypal.me/AutoHostfile");
        }
    }
}
