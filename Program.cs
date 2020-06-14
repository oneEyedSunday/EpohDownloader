using System;
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
            await DownloadHelpers.DownloadFiles(new string[] {
                "", ""
            });
        }
    }
}
