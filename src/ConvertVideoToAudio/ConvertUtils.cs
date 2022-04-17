using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ConvertCommon;
using Xabe.FFmpeg;

namespace ConvertVideoToAudio
{
    public static class ConvertUtils
    {
        public static async Task Run(string[] fileNames, CancellationToken token)
        {
            var config = Utils.GetConvertConfiguration();
            var filesToConvert = Utils.GetFilesToConvert(fileNames);
            await Console.Out.WriteLineAsync($"Find {filesToConvert.Count} files to convert");

            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //Set FFmpeg directory
            FFmpeg.SetExecutablesPath(config.FFmpegExecutablePath);
            await Console.Out.WriteLineAsync($"Set ffmpeg executable path to: {config.FFmpegExecutablePath}");

            //Run conversion
            await RunConversion(filesToConvert, token, config);
        }

        static async Task RunConversion(IEnumerable<FileInfo> filesToConvert, CancellationToken token, ConvertConfiguration config)
        {
            var audioType = config.AudioType;
            var overrideExistFile = config.OverrideExistFile;
            var threadsCount = config.ThreadsCount;

            if (threadsCount <= 0)
            {
                threadsCount = Environment.ProcessorCount;
            }

            await Console.Out.WriteLineAsync($"Threads Count: {threadsCount}");

            foreach (var fileToConvert in filesToConvert)
            {
                var outputFileName = Path.ChangeExtension(fileToConvert.FullName, audioType);
                var conversion = await FFmpeg.Conversions.FromSnippet.ExtractAudio(fileToConvert.FullName, outputFileName);
                conversion.SetOverwriteOutput(overrideExistFile);
                conversion.UseMultiThread(threadsCount);

                conversion.OnProgress += async (sender, args) =>
                {
                    await Console.Out.WriteLineAsync($"[{args.Duration}/{args.TotalLength}][{args.Percent}%] {fileToConvert.Name}");
                };

                await conversion.Start(token);

                await Console.Out.WriteLineAsync($"Finished converion file [{fileToConvert.Name}], output path [{conversion.OutputFilePath}]");
            }
        }
    }
}
