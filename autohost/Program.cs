using AutoHostfileLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace autohost
{
    class Program
    {
        enum Command { ListHosts, TailLog };

        static Command ProcessArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-log")
                {
                    return Command.TailLog;
                }
            }

            return Command.ListHosts;
        }

        static void Main(string[] args)
        {
            switch(ProcessArgs(args))
            {
                case Command.ListHosts:
                    ListHosts();
                    break;

                case Command.TailLog:
                    TailLog();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Tails the log file continuously
        /// </summary>
        private static void TailLog()
        {
            var logFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\AutoHostfile";
            var logFilename = @"AutoHostfile.txt";
            var logFile = logFolder + @"\" + logFilename;

            var fileChangedEvent = new AutoResetEvent(false);
            var fsWatcher = new FileSystemWatcher(logFolder);
            fsWatcher.Filter = logFilename;
            fsWatcher.EnableRaisingEvents = true;
            fsWatcher.Changed += (s, e) => fileChangedEvent.Set();

            var fileStream = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (var reader = new StreamReader(fileStream))
            {
                var line = "";
                while (true)
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        Console.WriteLine(line);
                    }
                    else
                    {
                        fileChangedEvent.WaitOne(1000);
                    }
                }
            }
        }

        /// <summary>
        /// Shows a list of hosts known to this host
        /// </summary>
        private static void ListHosts()
        {
            var parser = new HostsFileParser();
            var oldEntries = new List<HostEntry>();
            var originalLines = new List<string>();

            parser.Parse(oldEntries, originalLines);

            foreach(var entry in oldEntries)
            {
                Console.WriteLine("{0} => {1}", entry.Address, entry.HostName);
            }
        }
    }
}
