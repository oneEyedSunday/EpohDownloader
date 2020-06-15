using System;
using System.Web;
using EpohScraper.Helpers;
using System.Threading.Tasks;

namespace EpohScraper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // "scripts/produce.sh".BashRunScriptFileSync();
            // "scripts/produce.sh".BashRunScriptFileAsync();
            Console.WriteLine($"Did we create directory: {DownloadHelpers.TryCreateDirectory("/Users/ispoa/Downloads/EpohScraper/NonExistent")}");

            // Download File from internet
            var _downloadTasks = DownloadHelpers.DownloadFiles(new string[] {
                "http://localhost:3000/?artist=Future&album=Honest&song=02+T-Shirt.mp3",
                "http://localhost:3000/?artist=Jeremih" + "&album=" + HttpUtility.UrlEncode("Late Nights") + "&song=" + HttpUtility.UrlEncode("05 Drank.mp3"),
                "http://localhost:3000/?artist=Jeremih" + "&album=" + HttpUtility.UrlEncode("Late Nights") + "&song=" + HttpUtility.UrlEncode("10 Actin' Up.mp3"),
                "http://localhost:3000/?artist=Jeremih" + "&album=" + HttpUtility.UrlEncode("Late Nights") + "&song=" + HttpUtility.UrlEncode("15 Paradise.mp3"),
                "http://localhost:3000/?artist=Guards&album=" + HttpUtility.UrlEncode("In Guards We Trust") + "&song=" + HttpUtility.UrlEncode("07 I Know It's You.mp3")
            });

            Console.WriteLine("Im off continuing with work");

            _downloadTasks.GetAwaiter().OnCompleted(() =>
            {
                Console.WriteLine("[+] Finished downloading");
            });

            await _downloadTasks;
        }
    }
}
