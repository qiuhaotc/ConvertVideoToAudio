# ConvertVideoToAudio

Use ffmpeg and Xabe.FFmpeg to extract audios from videos

## Usage

1. Download ffmpeg from <https://ffmpeg.org/>, extract the bin files to your local PC

2. Change App.config section "FFmpegExecutablePath" value to your ffmpeg folder that contains the "ffmpeg.exe" and "ffprobe.exe", like "C:\ffmpeg\bin"

3. You can also change the audio you want to convert to, like mp3, aac. Override existing files or not. Threads count. They all placed in App.config.

4. Compile the solution, the bin file is placed in "ConvertVideoToAudio\bin\netcoreapp3.1"

5. Drag the video files you want to convert to the "ConvertVideoToAudio.exe"

6. Wait converting finshed
