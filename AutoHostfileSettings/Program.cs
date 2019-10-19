using AutoHostfileLib;
using System;
using System.Windows.Forms;

namespace AutoHostfileSettings
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logger.LogStdout = true;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AutoHostfileSettingsForm());
        }
    }
}
