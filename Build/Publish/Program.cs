using AutoHostfile;
using System;
using System.IO;

namespace Publish
{
    class Program
    {
        static void Main(string[] args)
        {
            var shortVersion = Common.ShortVersion;
            var longVersion = Common.LongVersion;

            string[] expectsShortVersion = { @"..\..\..\..\PATCH_NOTES.md", @"..\..\..\..\README.md" };
            string[] expectsLongVersion = { @"..\..\..\..\AutoHostfileInstaller\Product.wxs" };

            foreach (var file in expectsShortVersion)
            {
                var contents = File.ReadAllText(file);
                if (!contents.Contains(shortVersion))
                {
                    throw new InvalidDataException(file + " has not been updated for " + shortVersion);
                }

                Console.WriteLine("Checked {0}", file);
            }

            foreach (var file in expectsLongVersion)
            {
                var contents = File.ReadAllText(file);
                if (!contents.Contains(longVersion))
                {
                    throw new InvalidDataException(file + " has not been updated for " + longVersion);
                }

                Console.WriteLine("Checked {0}", file);
            }

            var publishedFile = @"..\..\..\..\Releases\AutoHostfile-" + shortVersion + ".msi";
            File.Copy(@"..\..\..\..\AutoHostfileInstaller\bin\Release\AutoHostfileInstaller.msi", publishedFile, true);
            Console.WriteLine("Published {0}, you can now Ping and tag the release", publishedFile);
        }
    }
}
