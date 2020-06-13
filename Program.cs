using System;
using EpohScraper.Helpers;

namespace EpohScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            "scripts/produce.sh".BashRunScriptFileSync();
            "scripts/produce.sh".BashRunScriptFileAsync();
        }
    }
}
