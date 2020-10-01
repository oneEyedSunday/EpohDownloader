using System.Collections.Generic;
using library.Structures;

namespace EpohScraper.Models
{
    public class MediaEntity
    {
        public MediaEntity(string artistName, string albumName): this(artistName, albumName, new string[] { })
        { }

        public MediaEntity(string artistName, IEnumerable<string> urls): this(artistName, "Unknown Album", urls)
        { }

        public MediaEntity(string artistName, string albumName, string url): this(artistName, albumName, new string[] { url })
        { }

        public MediaEntity(string artistName, string albumName, IEnumerable<string> urls)
        {
            Artist = artistName;
            Album = string.IsNullOrEmpty(albumName) ? "Unknown Album" : albumName;
            AddDownloadUrls(urls);
        }

        private Set<string> _downloadUrls { get; } = new Set<string>();

        public string Artist { get; private set; }

        public string Album { get; private set; }

        public void AddDownloadUrl(string url)
        {
            _downloadUrls.Add(url);
        }

        public void AddDownloadUrls(IEnumerable<string> urls)
        {
            foreach (string url in urls)
                _downloadUrls.Add(url);
        }

        public IEnumerable<string> GetUrls()
        {
            return _downloadUrls.Values;
        }
    }
}
