using ConcatVideos;

CancellationTokenSource cancellationTokenSource;
bool finishedRunning;

cancellationTokenSource = new CancellationTokenSource();

try
{
    var fileNames = Environment.GetCommandLineArgs().Skip(1);
    Console.WriteLine("Start concatenate videos for: " + string.Join(Environment.NewLine, fileNames));
    await ConcatenateUtils.Run(fileNames, cancellationTokenSource.Token);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

finishedRunning = true;
Console.WriteLine("Running finished, press any key to exit.");
Console.ReadLine();

// See https://aka.ms/new-console-template for more information
AppDomain.CurrentDomain.ProcessExit += new EventHandler((u, v) =>
{
    cancellationTokenSource.Cancel();

    while (!finishedRunning)
    {
        Console.WriteLine("Wait ffmpeg exiting...");
        Thread.Sleep(100);
    }

    cancellationTokenSource.Dispose();
});
