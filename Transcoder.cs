using System.Diagnostics;

public static class Transcoder
{  
    public static string Transcode(string highlight, string start, string end)
    {
        var output = GetOutputPath(highlight);
        DoTranscode($"-i \"{highlight}\" -hide_banner -loglevel error -c:v h264_nvenc -c:v copy -c:a copy -ss {start} -to {end} \"{output}\"");
        return output;
    }

    public static string Transcode(string highlight, int targetSize)
    {
        var output = GetOutputPath(highlight);
        var targetBitrate = CalculateTargetBitrate(GetDuration(highlight), targetSize);
        DoTranscode($"-i \"{highlight}\" -hide_banner -loglevel error -c:v h264_nvenc -b:v {targetBitrate}k -maxrate {targetBitrate}k -bufsize {targetBitrate}k -c:a copy \"{output}\"");
        return output;
    }

    public static string Transcode(string highlight, string start, string end, int targetSize)
    {
        var output = GetOutputPath(highlight);
        var targetBitrate = CalculateTargetBitrate(GetDuration(highlight), targetSize);
        DoTranscode($"-i \"{highlight}\" -hide_banner -loglevel error -c:v h264_nvenc -b:v {targetBitrate}k -maxrate {targetBitrate}k -bufsize {targetBitrate}k -c:a copy -ss {start} -to {end} \"{output}\"");
        return output;
    }

    private static double GetDuration(string highlight)
    {
        using (var process = new Process())
        {
            process.StartInfo.FileName = "ffprobe";
            process.StartInfo.Arguments = $"-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 \"{highlight}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            if (double.TryParse(output, out double durationSeconds))
            {
                return durationSeconds;
            }
            return 0;
        }
    }

    private static int CalculateTargetBitrate(double duration, int targetSize)
    {
         /* Factors applied:
            1000 => KB to MB
            0.8 => Margin to ensure target file size won't be exceeded (variable bitrate)
            8 => Bytes to bits
        */
        return (int)(targetSize * 1000 * 0.8 * 8 / duration);
    }

    private static string GetOutputPath(string highlight)
    {
        return Path.Combine(Path.GetDirectoryName(highlight), $"{Path.GetFileNameWithoutExtension(highlight)}_Transcoded{Path.GetExtension(highlight)}");
    }

    private static void DoTranscode(string arguments)
    {
        try
        {
            Logger.LogMessage($"Transcoding Highlight..");

            using(var process = new Process())
            {
                process.StartInfo.FileName = "ffmpeg.exe";
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardError = true;

                process.Start();
                process.WaitForExit();

                string errorOutput = process.StandardError.ReadToEnd();
                if (string.IsNullOrWhiteSpace(errorOutput))
                {
                    Logger.LogMessage($"Highlight successfully transcoded.");
                }
                else
                {
                    Logger.LogError($"An error occured while transcoding: {errorOutput}.");
                }
            }
        }
        catch(Exception e)
        {
            Logger.LogError($"An exception occured during transcoding: {e.Message}.");
        }
    }
}