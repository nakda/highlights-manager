public class Configuration
{
    public string TemporaryFolder { get; set; }
    public string PermanentFolder { get; set; }

    public bool MoveHighlights { get; set; }

    public bool TranscodeHighlights { get; set; }
    public bool TranscodeTrim { get; set; }
    public string TranscodeStartTime { get; set; }
    public string TranscodeEndTime { get; set; }
    public int TranscodeSize { get; set; }
    public bool PreserveOriginal { get; set; }
    public bool PreserveTranscoded { get; set; }

    public bool DiscordShare { get; set; }
    public string DiscordWebhook { get; set; }
    public string DiscordUsername { get; set; }
    public string DiscordAvatar { get; set; }

    public static Configuration Load(string configurationFile)
    {
        if(File.Exists(configurationFile))
        {
            var toml = File.ReadAllText(configurationFile);
            return Tomlyn.Toml.ToModel<Configuration>(toml);
        }

        return null;
    }

    public bool IsValid()
    {
        var valid = true;

        if(string.IsNullOrEmpty(TemporaryFolder) || !Directory.Exists(TemporaryFolder))
        {
            valid = false;
            Logger.LogError("Temporary folder is invalid or doesn't exist.");
        }

        if(string.IsNullOrEmpty(PermanentFolder) || !Directory.Exists(PermanentFolder))
        {
            valid = false;
            Logger.LogError("Permanent folder is invalid or doesn't exist.");
        }

        if(TranscodeHighlights)
        {
            if(TranscodeTrim)
            {
                var startTime = TimeSpan.Zero;
                if(!TimeSpan.TryParse(TranscodeStartTime, out startTime))
                {
                    valid = false;
                    Logger.LogError("Transcode start time is invalid: expected format is \"00:00:00\" (hh:mm:ss).");
                }

                var endTime = TimeSpan.Zero;
                if(!TimeSpan.TryParse(TranscodeEndTime, out endTime))
                {
                    valid = false;
                    Logger.LogError("Transcode end time is invalid: expected format is \"00:00:00\" (hh:mm:ss).");
                }

                if(endTime <= startTime)
                {
                    valid = false;
                    Logger.LogError("Transcode end time has to be higher than start time.");
                }
            }

            if(TranscodeSize <= 0)
            {
                valid = false;
                Logger.LogError("Transcode size can't be negative.");
            }
        }

        if(DiscordShare)
        {
            if(string.IsNullOrEmpty(DiscordWebhook))
            {
                valid = false;
                Logger.LogError("A webhook needs to be provided for Discord sharing.");
            }
        }

        return valid;
    }
}