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
        private Dictionary<string, HostEntry> _hostnameToAddress;
        private const string _commentToken = "HostFileRewriter";
        private const string _commentFormat = " # " + _commentToken;
        private string _hostsFilePath;

        /// <summary>
        /// Used by the command line tool
        /// </summary>
        public HostsFileParser()
        {
            _hostsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\drivers\etc\hosts";
        }

        /// <summary>
        /// Used by the service
        /// </summary>
        /// <param name="hostnameToAddress"></param>
        internal HostsFileParser(Dictionary<string, HostEntry> hostnameToAddress) : this()
        {
            _hostnameToAddress = hostnameToAddress;
        }

        /// <summary>
        /// Parse the hosts file
        /// </summary>
        /// <param name="oldEntries">Any host entries previously added by us</param>
        /// <param name="originalLines">The original lines of the file as if we never edited it</param>
        public bool Parse(List<HostEntry> oldEntries, List<string> originalLines)
        {
            bool foundOurContent = false;

            var blankLines = new List<string>();
            using (var reader = new StreamReader(_hostsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (Regex.Match(line, @"^\s*$").Success)
                    {
                        // Blank line
                        blankLines.Add(line);
                    }
                    else if (Regex.Match(line, @"^\s*\#.*$", RegexOptions.None).Success || line.IndexOf(_commentToken) == -1)
                    {
                        // Keep any blank lines that the user added, but we'll leave any blank lines that we added
                        originalLines.AddRange(blankLines);
                        blankLines.Clear();

                        // User edited line or comment
                        originalLines.Add(line);
                    }
                    else
                    {
                        // It's one of our lines
                        foundOurContent = true;

                        var tokens = line.Split();
                        DateTime timeStamp;

                        // Find the timestamp, it's made of two tokens
                        string prevToken = "";
                        foreach (var token in tokens)
                        {
                            if (prevToken.Length != 0 && 
                                DateTime.TryParse(prevToken + " " + token, out timeStamp))
                            {
                                if ((DateTime.Now - timeStamp).TotalDays < Config.Instance.GetOldHostRetentionDays())
                                {
                                    oldEntries.Add(new HostEntry(tokens[1], tokens[0], timeStamp));
                                }
                                else
                                {
                                    Logger.Debug("Descarding old hosts entry for {0} since it's older than {1} days", tokens[1], Config.Instance.GetOldHostRetentionDays());
                                }
                                break;
                            }
                            prevToken = token;
                        }
                    }
                }
            }

            return foundOurContent;
        }

        internal void Rewrite()
        {
            var oldEntries = ScrubFile();
            if (_hostnameToAddress.Count > 0 || oldEntries.Count > 0)
            {
                Logger.Info("Rewriting {0}", _hostsFilePath);

                // We always want our own friendly name to be self pingable
                var ourName = Config.Instance.GetFriendlyHostname();
                _hostnameToAddress[ourName] = new HostEntry(ourName, "127.0.0.1");

                var addresses = new HashSet<string>();
                foreach(var value in _hostnameToAddress.Values)
                {
                    addresses.Add(value.Address);
                }

                foreach(var oldEntry in oldEntries)
                {
                    // Add back any old host entries which don't conflict with the latest information we got from broadcasts
                    if(!_hostnameToAddress.ContainsKey(oldEntry.HostName) && !addresses.Contains(oldEntry.Address))
                    {
                        _hostnameToAddress.Add(oldEntry.HostName, oldEntry);
                    }
                }

                try
                {
                    using (var writer = new StreamWriter(_hostsFilePath, true))
                    {
                        writer.WriteLine();
                        writer.WriteLine();

                        foreach (var originalLine in _hostnameToAddress.Values)
                        {
                            writer.WriteLine("{0,-15} {1,-20} {2,-25} {3}", originalLine.Address, originalLine.HostName, _commentFormat, originalLine.Timestamp);
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
                Logger.Debug("Reading {0}", _hostsFilePath);

                // Parse the file
                var originalLines = new List<string>();

                // If our content was found in the file, we'll need to rewrite it back to the original file
                if (Parse(oldEntries, originalLines))
                {
                    // Rewrite the file without our entries
                    using (var writer = new StreamWriter(_hostsFilePath, false))
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
