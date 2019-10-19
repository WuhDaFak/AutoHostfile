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
            Debug,
            Info
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

        internal static void Info(string stringToLog, params object[] formatStrings)
        {
            Logger.Instance.Write(LogLevel.Info, stringToLog, formatStrings);
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
