using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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

        private static bool TryDownloadRemoteFile(string url, string path)
        {
            using (var _webClient = new WebClient())
            {
                try
                {
                    _webClient.DownloadProgressChanged += (sender, e) => WriteLine($"[+] {e.ProgressPercentage}% of {e.TotalBytesToReceive / 1024 / 1024} MB");
                    _webClient.DownloadFileCompleted += (sender, e) => Console.WriteLine($"[+] Downloaded Successfully: {(!e.Cancelled ? "Yes" : "No")}");
                    _webClient.DownloadFileAsync(new Uri(url), path);
                    return true;
                }
                catch (Exception ex)
                {
                    WriteLine($"[-] A(n) {ex.GetType().Name} Error Occured while downloading async remote file from {url}: {ex.Message}");
                    return false;
                }
            }
        }

        private static async Task DownloadRemoteFileAsync(string uri, string path)
        {
            using(var _webClient = new WebClient())
            {
                try
                {
                    _webClient.DownloadProgressChanged += (sender, e) =>
                    {
                        if (e.ProgressPercentage % 10 == 0 && e.ProgressPercentage < 100)
                            WriteLine($"[+] { e.ProgressPercentage}% of { e.TotalBytesToReceive / 1024 / 1024} MB");
                    };
                    _webClient.DownloadFileCompleted += (sender, e) => Console.WriteLine($"[+] Downloaded Successfully: {(!e.Cancelled ? "Yes" : "No")}");
                    await _webClient.DownloadFileTaskAsync(new Uri(uri), path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[-] A(n) {ex.GetType().Name} Error Occured while downloading remote file from {uri}: {ex.Message}");
                }
            }
        }

        private static string GetFileName(string url)
        {
            string[] _splitByParams = url.Split("=");
            if (_splitByParams.Length > 0 && _splitByParams[_splitByParams.Length - 1].EndsWith(".mp3"))
                return _splitByParams[_splitByParams.Length - 1];
            return url;
        }

        public static Task DownloadFiles(string[] urls)
        {
            List<Task> downloads = new List<Task>();

            foreach (var url in urls)
            {
                WriteLine($"[+] Will save {url} to {GetFileName(url)}");
                //downloads.Add(TryDownloadRemoteFile(url, "/Users/ispoa/Downloads/EpohScraper/" + GetFileName(url));
                downloads.Add(DownloadRemoteFileAsync(url, "/Users/ispoa/Downloads/EpohScraper/" + GetFileName(url)));
            }


            return Task.WhenAll(downloads);

        }
    }
}

