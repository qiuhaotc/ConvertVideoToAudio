using ConcatVideos;

CancellationTokenSource cancellationTokenSource;
var finishedRunning = false;

cancellationTokenSource = new CancellationTokenSource();


// See https://aka.ms/new-console-template for more information
AppDomain.CurrentDomain.ProcessExit += new EventHandler((u, v) =>
{
    cancellationTokenSource.Cancel();

    if (!finishedRunning)
    {
        Console.WriteLine("Wait ffmpeg exiting...");
        Thread.Sleep(100);
    }

    cancellationTokenSource.Dispose();
});

try
{
    var fileNames = Environment.GetCommandLineArgs().Skip(1);
    Console.WriteLine("Start concatenate videos for: " + string.Join(", ", fileNames));
    await ConcatenateUtils.Run(fileNames, cancellationTokenSource.Token);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

finishedRunning = true;
Console.WriteLine("Running finished, press any key to exit.");
Console.ReadLine();
