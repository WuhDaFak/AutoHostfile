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
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AutoHostfileLib;

namespace AutoHostfileSettings
{
    public partial class AutoHostfileSettingsForm : Form
    {
        private Regex rgxValidHost = new Regex(@"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$");

        private static Random random = new Random();

        private string originalFriendlyName = Config.Instance.GetFriendlyHostname();
        private int originalPort = Config.Instance.GetPort();
        private string originalSharedKey = Config.Instance.GetSharedKey();

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

            lblVersion.Text = "Version: " + config.GetShortVersion();
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

        private void linkLblUpdates_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/benstaniford/AutoHostfile");
        }

        private bool txtFriendlyNameValid()
        {
            return txtFriendlyName.Text.Length > 0 && rgxValidHost.IsMatch(txtFriendlyName.Text);
        }

        private void txtFriendlyName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            validationErrorProvider.SetError(txtFriendlyName, txtFriendlyNameValid() ? "" : "Not a valid hostname");
        }

        private bool txtPortValid()
        {
            int port;
            return int.TryParse(txtPort.Text, out port) && port < IPEndPoint.MaxPort;
        }

        private void txtPort_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            validationErrorProvider.SetError(txtPort, txtPortValid() ? "" : "Not a valid port");
        }

        private bool txtSharedKeyValid()
        {
            return txtSharedKey.Text.Length >= 8;
        }

        private void txtSharedKey_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            validationErrorProvider.SetError(txtSharedKey, txtSharedKeyValid() ? "" : "Shared key must be at least 8 characters");
        }

        private void allFields_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = txtFriendlyNameValid() && txtPortValid() && txtSharedKeyValid();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            txtSharedKey.Text = RandomString(12);
        }

        public static string RandomString(int length)
        {
            const string chars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!""£$%^&()";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
