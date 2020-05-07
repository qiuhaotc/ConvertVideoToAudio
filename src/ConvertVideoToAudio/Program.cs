using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConvertVideoToAudio
{
    class Program
    {
        static CancellationTokenSource cancellationTokenSource;
        static bool finishedRunning;

        static async Task Main(string[] fileNames)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await ConvertUtils.Run(fileNames, cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            finishedRunning = true;
            Console.WriteLine("Running finished, press any key to exit.");
            Console.ReadLine();
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();

            if (!finishedRunning)
            {
                Console.WriteLine("Wait ffmpeg exiting...");
                Thread.Sleep(100);
            }

            cancellationTokenSource.Dispose();
        }
    }
}
