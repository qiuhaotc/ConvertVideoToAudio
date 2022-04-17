using ConvertCommon;
using Xabe.FFmpeg;

namespace ConcatVideos
{
    public static class ConcatenateUtils
    {
        public static async Task Run(IEnumerable<string> fileNames, CancellationToken token)
        {
            var config = Utils.GetConvertConfiguration();
            var filesToConvert = Utils.GetFilesToConvert(fileNames);
            await Console.Out.WriteLineAsync($"Find {filesToConvert.Count} files to convert");

            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //Set FFmpeg directory
            FFmpeg.SetExecutablesPath(config.FFmpegExecutablePath);
            await Console.Out.WriteLineAsync($"Set ffmpeg executable path to: {config.FFmpegExecutablePath}");

            //Run conversion
            await RunConcatenate(filesToConvert, token, config);
        }

        static async Task RunConcatenate(List<FileInfo> filesToConvert, CancellationToken token, ConvertConfiguration config)
        {
            if (filesToConvert.Count <= 1)
            {
                await Console.Out.WriteLineAsync("Run concatenate should select at least two files");
                return;
            }

            var overrideExistFile = config.OverrideExistFile;
            var threadsCount = config.ThreadsCount;

            if (threadsCount <= 0)
            {
                threadsCount = Environment.ProcessorCount;
            }

            await Console.Out.WriteLineAsync($"Threads Count: {threadsCount}");

            var output = AppendPrefixToFilename(filesToConvert[0], config.PrefixForConcatenate);
            var conversion = await FFmpeg.Conversions.FromSnippet.Concatenate(output, filesToConvert.Select(u => u.FullName).ToArray());
            conversion.SetOverwriteOutput(overrideExistFile);
            conversion.UseMultiThread(threadsCount);

            conversion.OnProgress += async (sender, args) =>
            {
                await Console.Out.WriteLineAsync($"[{args.Duration}/{args.TotalLength}][{args.Percent}%]");
            };

            await conversion.Start(token);
            await Console.Out.WriteLineAsync($"Finished concatenate files [{string.Join(",", filesToConvert.Select(u => u.Name))}], output path [{conversion.OutputFilePath}]");
        }

        static string AppendPrefixToFilename(FileInfo fileInfo, string prefix)
        {
            return Path.Combine(fileInfo.DirectoryName ?? throw new ArgumentException($"Null Directory for file {fileInfo.FullName}"), prefix + fileInfo.Name);
        }
    }
}
