# ConvertVideoToAudio

Use ffmpeg and Xabe.FFmpeg to extract audios from videos.

Such as convert video like mp4 or mkv to mp3 or aac.

## Usage

### Quick start for win64

1. Download the last release <https://github.com/qiuhaotc/ConvertVideoToAudio/releases/latest> of Release-X-with-win64-ffmpeg.zip
2. Drag the video files you want to convert to the "ConvertVideoToAudio.exe"

![Convert To Audio](https://raw.githubusercontent.com/qiuhaotc/ConvertVideoToAudio/master/doc/ConvertToAudio.gif)

### Advanced use

1. Download ffmpeg from <https://ffmpeg.org/>, extract the bin files to your local PC

2. Change App.config section "FFmpegExecutablePath" value to your ffmpeg folder that contains the "ffmpeg.exe" and "ffprobe.exe", like "C:\ffmpeg\bin"

3. You can also change the audio you want to convert to, like mp3, aac. Override existing files or not. Threads count. They all placed in App.config.

4. Compile the solution, the bin file is placed in "ConvertVideoToAudio\bin\netcoreapp3.1"

5. Drag the video files you want to convert to the "ConvertVideoToAudio.exe"

6. Wait converting finshed
