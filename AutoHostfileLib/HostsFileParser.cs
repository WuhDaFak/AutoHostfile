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
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AutoHostfileLib
{
    public class HostsFileParser
    {
        private Dictionary<string, HostEntry> HostnameToAddress;
        private const string CommentToken = "HostFileRewriter";
        private const string CommentFormat = " # " + CommentToken;
        private string HostsFilePath;

        /// <summary>
        /// Used by the command line tool
        /// </summary>
        public HostsFileParser()
        {
            HostsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\drivers\etc\hosts";
        }

        /// <summary>
        /// Used by the service
        /// </summary>
        /// <param name="hostnameToAddress"></param>
        internal HostsFileParser(Dictionary<string, HostEntry> hostnameToAddress) : this()
        {
            HostnameToAddress = hostnameToAddress;
        }

        /// <summary>
        /// Parse the hosts file
        /// </summary>
        /// <param name="oldEntries">Any host entries previously added by us</param>
        /// <param name="originalLines">The original lines of the file as if we never edited it</param>
        public void Parse(List<HostEntry> oldEntries, List<string> originalLines)
        {
            var blankLines = new List<string>();
            using (var reader = new StreamReader(HostsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (Regex.Match(line, @"^\s*$").Success)
                    {
                        // Blank line
                        blankLines.Add(line);
                    }
                    else if (Regex.Match(line, @"^\s*\#.*$", RegexOptions.None).Success || line.IndexOf(CommentToken) == -1)
                    {
                        // Keep any blank lines that the user added
                        originalLines.AddRange(blankLines);
                        blankLines.Clear();

                        // User edited line or comment
                        originalLines.Add(line);
                    }
                    else
                    {
                        // It's one of our lines
                        var tokens = line.Split();
                        oldEntries.Add(new HostEntry(tokens[1], tokens[0]));
                    }
                }
            }
        }

        internal void Rewrite()
        {
            var oldEntries = ScrubFile();
            if (HostnameToAddress.Count > 0 || oldEntries.Count > 0)
            {
                Logger.Info("Rewriting {0}", HostsFilePath);

                // We always want our own friendly name to be self pingable
                var ourName = Config.Instance.GetFriendlyHostname();
                HostnameToAddress[ourName] = new HostEntry(ourName, "127.0.0.1");

                var addresses = new HashSet<string>();
                foreach(var value in HostnameToAddress.Values)
                {
                    addresses.Add(value.Address);
                }

                foreach(var oldEntry in oldEntries)
                {
                    // Add back any old host entries which don't conflict with the latest information we got from broadcasts
                    if(!HostnameToAddress.ContainsKey(oldEntry.HostName) && !addresses.Contains(oldEntry.Address))
                    {
                        HostnameToAddress.Add(oldEntry.HostName, oldEntry);
                    }
                }

                try
                {
                    using (var writer = new StreamWriter(HostsFilePath, true))
                    {
                        writer.WriteLine();
                        writer.WriteLine();

                        foreach (var originalLine in HostnameToAddress.Values)
                        {
                            writer.WriteLine("{0} {1} {2,25} {3}", originalLine.Address, originalLine.HostName, CommentFormat, DateTime.Now);
                        }
                    }
                }
                catch(IOException ex)
                {
                    Logger.Error("Error reading/writing hosts file: " + ex);
                }
            }
            else
            {
                Logger.Debug("No changes required");
            }
        }


        /// <summary>
        /// Scrubs the file, leaving the lines which have nothing to do with us and returns a list of our old records
        /// </summary>
        private List<HostEntry> ScrubFile()
        {
            var oldEntries = new List<HostEntry>();

            try
            {
                Logger.Debug("Reading {0}", HostsFilePath);

                // Parse the file
                var originalLines = new List<string>();
                Parse(oldEntries, originalLines);

                if (oldEntries.Count > 0)
                {
                    // Rewrite the file without our entries
                    using (var writer = new StreamWriter(HostsFilePath, false))
                    {
                        foreach (var originalLine in originalLines)
                        {
                            writer.WriteLine(originalLine);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Logger.Error("Error reading/writing hosts file: " + ex);
            }

            return oldEntries;
        }

    }
}
