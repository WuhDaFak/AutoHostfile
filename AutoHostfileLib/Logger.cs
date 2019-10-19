using System;
using System.IO;

namespace AutoHostfileLib
{
    public sealed class Logger
    {
        private static readonly Lazy<Logger> logger = new Lazy<Logger>(() => new Logger());

        public static Logger Instance { get { return logger.Value; } }

        private string LogFile;

        public static bool LogStdout { get; set; } = false;

        private enum LogLevel
        {
            Error,
            Warn,
            Debug
        };

        private Logger()
        {
            if (!LogStdout)
            {
                var logPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\AutoHostfile";
                LogFile = logPath + @"\AutoHostfile.txt";
                try
                {
                    Directory.CreateDirectory(logPath);
                    if (File.Exists(LogFile))
                    {
                        File.Delete(LogFile);
                    }
                }
                catch(IOException)
                {
                    // Not much we can do
                }
            }
        }

        public static void Debug(string stringToLog, params object[] formatStrings)
        {
            Logger.Instance.Write(LogLevel.Debug, stringToLog, formatStrings);
        }

        public static void Warn(string stringToLog, params object[] formatStrings)
        {
            Logger.Instance.Write(LogLevel.Warn, stringToLog, formatStrings);
        }

        public static void Error(string stringToLog, params object[] formatStrings)
        {
            Logger.Instance.Write(LogLevel.Error, stringToLog, formatStrings);
        }

        private void Write(LogLevel level, string stringToLog, params object[] formatStrings)
        {
            lock(this)
            {
                try
                {
                    if (LogStdout)
                    {
                        Console.WriteLine(DateTime.Now.ToString("dd-MM-yyyy:HH:mm:ss.fff") + " " + level.ToString("G") + ": " + stringToLog, formatStrings);
                    }
                    else
                    {
                        using (var writer = new StreamWriter(LogFile, true))
                        {
                            writer.WriteLine(DateTime.Now.ToString("dd-MM-yyyy:HH:mm:ss.fff") + " " + level.ToString("G") + ": " + stringToLog, formatStrings);
                        }
                    }
                }
                catch
                {
                    // Not much we can do
                }
            }
        }
    }
}
