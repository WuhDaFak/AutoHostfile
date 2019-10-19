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
