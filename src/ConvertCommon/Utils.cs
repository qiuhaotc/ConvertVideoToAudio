using Microsoft.Extensions.Configuration;

namespace ConvertCommon
{
    public static class Utils
    {
        public static List<FileInfo> GetFilesToConvert(IEnumerable<string> fileNames)
        {
            return fileNames.Select(u => new FileInfo(u)).Where(file => file.Exists).ToList();
        }

        public static ConvertConfiguration GetConvertConfiguration()
        {
            var convertConfig = new ConvertConfiguration();
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.Build().Bind(convertConfig);

            if (string.IsNullOrEmpty(convertConfig.FFmpegExecutablePath))
            {
                convertConfig.FFmpegExecutablePath = AppDomain.CurrentDomain.BaseDirectory;
            }

            return convertConfig;
        }
    }
}