using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using static System.Console;

namespace EpohScraper.Helpers
{
    public static class DownloadHelpers
    {
        public static bool TryCreateDirectory(string directoryPath)
        {
            try
            {
                // Directory.GetDirectoryRoot(directoryPath);
                // Console.WriteLine($"DIr Root: {Directory.GetDirectoryRoot(directoryPath)}");
                // Console.WriteLine($"Another attempt at full dir path: {new FileInfo(directoryPath).Directory.FullName}");
                // var toExpand = new string[] {
                //     "~/Downloads", "$HOME/Downloads", "/users/$USER/Downloads",
                //     "~/Downloads", "HOME/Downloads", "/users/USER/Downloads",
                //     "~/Downloads", "%HOME%/Downloads", "/users/%USER%/Downloads"
                // };

                // foreach (var path in toExpand)
                // {
                //     // Console.WriteLine($"Expands {path} to {QualifyPath(path)}");
                //     Console.WriteLine($"Preprocessing {path} to windows env path {PreprocessUseWindowsEnvironment(path)}");
                // };
                if (Directory.Exists(directoryPath))
                {
                    WriteLine($"[+] Directory {directoryPath} already exists. Skipping Creation...");
                    return true;
                }
                var _info = Directory.CreateDirectory(directoryPath);
                WriteLine($"[+] {_info.FullName}");
                return true;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"[-] A(n)  {ex.GetType().Name} Error Occured: {ex.Message}");
                return false;
            }
        }

        private static string QualifyPath(string relativePath)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                relativePath = relativePath.StartsWith("~") ? "%HOME%" + relativePath.Substring(1) : relativePath;
            Console.WriteLine($"Path after preprocessing {relativePath}");
            return Environment.ExpandEnvironmentVariables(relativePath);
        }

        private static string MatchAgainstEnvironment(string pattern)
        {
            return System.Environment.GetEnvironmentVariable(pattern);
        }

        private static string PreprocessUseWindowsEnvironment(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return path;

            return path;
        }

        private static bool TryDownloadFile(string url)
        {
            WriteLine("Attempting to Download...");
            try
            {
                File.Copy("/Users/ispoa/Music/iTunes/iTunesMedia/Music/Iyanya/Unknown Album/_Head Swell.mp3", "/Users/ispoa/Downloads/EpohScraper/_Head Swell.mp3");
                WriteLine("[+] Successfully downloaded file");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[-] A(n)  {ex.GetType().Name} Error Occured: {ex.Message}");
                return false;
            }
        }

        public static Task DownloadFiles(string[] urls)
        {
            List<Task> downloads = new List<Task>();

            foreach(var url in urls)
                downloads.Add(Task.FromResult<bool>(TryDownloadFile(url)));

            return Task.WhenAll(downloads);
        }
    }
}

