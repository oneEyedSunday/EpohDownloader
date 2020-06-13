using System;
using System.Diagnostics;
using System.IO;
using static System.Console;


namespace EpohScraper.Helpers
{
    // TODO (oneeyedsunday) handle Errors
   public static class BashHelper
   {
       private static Process BootstrapProcess(string filePath) => new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    ArgumentList = { filePath }
                }
            };
        public static void BashRunScriptFileSync(this string filePath, Action<string> lineHandler = null)
        {
            var _process = BootstrapProcess(filePath);

            if (lineHandler == null)
            {
                lineHandler = new Action<string>(DefaultLineHandler);
            }

            _process.Start();
             using (StreamReader stdOutput = _process.StandardOutput)
            {
                while (!_process.StandardOutput.EndOfStream)
                {
                    string line = stdOutput.ReadLine();
                    if (line == null) break;
                    lineHandler(line);
                }
            }
            _process.WaitForExit();
        }

        public static void BashRunScriptFileAsync(this string filePath, DataReceivedEventHandler _handler = null)
        {
            var _process = BootstrapProcess(filePath);

            if (_handler == null)
            {
                _handler = DefaultAsyncLineHandler;
            }

            _process.Start();
            _process.OutputDataReceived += _handler;
            _process.BeginOutputReadLine();
            _process.WaitForExit();
        }

        private static void DefaultLineHandler(string line)
        {
            WriteLine($"Output (sync): {line}");
        }

        private static void DefaultAsyncLineHandler(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null) Console.WriteLine($"Received via handler: {e.Data}");
        }
   }
}
