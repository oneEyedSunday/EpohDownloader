using System;
using System.Web;
using System.Threading.Tasks;
using EpohScraper.Helpers;
using EpohScraper.Models;


namespace EpohScraper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // "scripts/produce.sh".BashRunScriptFileSync();
            // "scripts/produce.sh".BashRunScriptFileAsync();
            //Console.WriteLine($"Did we create directory: {DownloadHelpers.TryCreateDirectory("/Users/ispoa/Downloads/EpohScraper/NonExistent")}");

            // Download File from internet
            //var _downloadTasks = DownloadHelpers.DownloadFiles(new string[] {
            //    //"http://localhost:3000/?artist=Future&album=Honest&song=02+T-Shirt.mp3",
            //    //"http://localhost:3000/?artist=Jeremih" + "&album=" + HttpUtility.UrlEncode("Late Nights") + "&song=" + HttpUtility.UrlEncode("05 Drank.mp3"),
            //    //"http://localhost:3000/?artist=Jeremih" + "&album=" + HttpUtility.UrlEncode("Late Nights") + "&song=" + HttpUtility.UrlEncode("10 Actin' Up.mp3"),
            //    //"http://localhost:3000/?artist=Jeremih" + "&album=" + HttpUtility.UrlEncode("Late Nights") + "&song=" + HttpUtility.UrlEncode("15 Paradise.mp3"),
            //    //"http://localhost:3000/?artist=Guards&album=" + HttpUtility.UrlEncode("In Guards We Trust") + "&song=" + HttpUtility.UrlEncode("07 I Know It's You.mp3")
            //});

            string rootDownloadDir = args.Length > 1 ? args[1] : "~/Downloads/EpohScraper";

            DownloadHelpers.SetRootDownloadPath(rootDownloadDir);

            const string songs = @"""01 Look Ahead.mp3, 02 T - Shirt.mp3, 03 Move That Dope.mp3, 04 My Momma(Ft.Wiz Khalifa).mp3, 05 Honest.mp3, 06 I Won.mp3, 07 Never Satisfied.mp3, 08 I Be U.mp3, 09 Covered N Money.mp3, 
12 Blood Sweat Tears.mp3, 13 Big Rube Speaks.mp3, 14 Side Effects.mp3, 15 Ill Be Yours.mp3, 16 How Can I Not(Ft.Young Scooter).mp3, 17 Sh!t.mp3, 
18 Karate Chop Ft.Lil Wayne(Remix).mp3""";

            var media = new MediaEntity("Future", "Honest");

            foreach (string song in songs.Trim().Split(new char[] { ',' }))
            {
                media.AddDownloadUrl("http://localhost:3000/?artist=Future&album=Honest&song=" + HttpUtility.UrlEncode(song.Trim()));
            }


            var _downloadTasks = DownloadHelpers.DownloadAlbum(media);

            _downloadTasks.GetAwaiter().OnCompleted(() =>
            {
                Console.WriteLine("[+] Finished downloading");
            });

            await _downloadTasks;
        }
    }
}
