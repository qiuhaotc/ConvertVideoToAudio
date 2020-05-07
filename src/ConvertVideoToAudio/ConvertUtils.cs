using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace ConvertVideoToAudio
{
    public static class ConvertUtils
    {
        public static async Task Run(string[] fileNames, CancellationToken token)
        {
            var ffmpegLocation = ConfigurationManager.AppSettings["FFmpegExecutablePath"];

            if (string.IsNullOrEmpty(ffmpegLocation))
            {
                throw new Exception("Please set the ffmpeg executable path in the configuration!!!");
            }

            var filesToConvert = new List<FileInfo>(GetFilesToConvert(fileNames ?? Array.Empty<string>()));
            await Console.Out.WriteLineAsync($"Find {filesToConvert.Count} files to convert.");

            //Set FFmpeg directory
            FFmpeg.SetExecutablesPath(ffmpegLocation);

            //Run conversion
            await RunConversion(filesToConvert, token);
        }

        static IEnumerable<FileInfo> GetFilesToConvert(string[] fileNames)
        {
            return fileNames.Select(u => new FileInfo(u)).Where(file => file.Exists);
        }

        static async Task RunConversion(IEnumerable<FileInfo> filesToConvert, CancellationToken token)
        {
            var audioType = ConfigurationManager.AppSettings["AudioType"];
            var overrideExistFile = bool.Parse(ConfigurationManager.AppSettings["OverrideExistFile"]);
            var threadsCount = int.Parse(ConfigurationManager.AppSettings["ThreadsCount"]);

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
