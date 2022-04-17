namespace ConvertCommon
{
    public class ConvertConfiguration
    {
        public string AudioType { get; set; } = string.Empty;
        public bool OverrideExistFile { get; set; }
        public int ThreadsCount { get; set; }
        public string FFmpegExecutablePath { get; set; } = string.Empty;
        public string PrefixForConcatenate { get; set; } = string.Empty;
    }
}
