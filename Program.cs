internal class Program
{
    private const string ConfigurationFile = "config.toml";
    private static Configuration configuration;

    private static void Main()
    {   
        //Load & check configuration
        configuration = Configuration.Load(ConfigurationFile);
        if(configuration == null)
        {
            Logger.LogError($"Configuration file {ConfigurationFile} is missing: please download it from GitHub.");
        }
        else if(configuration.IsValid())
        {
            // Start monitoring temporary Highlights folder
            var fileWatcher = new FileSystemWatcher(configuration.TemporaryFolder, "*.mp4");
            fileWatcher.Created += OnHighlightCaptured;
            fileWatcher.Error += OnWatcherError;
            fileWatcher.EnableRaisingEvents = true;
            
            Logger.LogMessage("Configuration loaded successfully: waiting for Highlights to be captured.");
        }
        else
        {
            Logger.LogError($"Configuration file {ConfigurationFile} is invalid: please fix the configuration and restart.");
        }

        // Prevent application from being terminated prematurely
        Logger.LogMessage("At any time, press Enter to quit the application.");
        Console.Read();
    }

    private static void OnHighlightCaptured(object eventSource, FileSystemEventArgs eventData)
    {
        Logger.LogMessage($"Newly captured Highlight detected: {(eventData.Name)}");

        // Preserve the Highlight before it's purged by GeForce Experience
        var savedHighlightPath = SaveHighlight(eventData.FullPath);

        if(configuration.TranscodeHighlights)
        {
            var transcodedHighlightPath = string.Empty;

            if(configuration.TranscodeTrim && configuration.TranscodeSize > 0)
            {
                transcodedHighlightPath = Transcoder.Transcode(savedHighlightPath, configuration.TranscodeStartTime, configuration.TranscodeEndTime, configuration.TranscodeSize);
            }
            else if(configuration.TranscodeTrim)
            {
                transcodedHighlightPath = Transcoder.Transcode(savedHighlightPath, configuration.TranscodeStartTime, configuration.TranscodeEndTime);
            }
            else if(configuration.TranscodeSize > 0)
            {
                transcodedHighlightPath = Transcoder.Transcode(savedHighlightPath, configuration.TranscodeSize);
            }

            if(string.IsNullOrEmpty(transcodedHighlightPath))
            {
                Logger.LogError($"Highlight could not be transcoded: aborting next steps.");
                return;
            }

            if(configuration.DiscordShare)
            {
                Task.Run(() => Discord.WebhookAsync(configuration.DiscordUsername, configuration.DiscordAvatar, configuration.DiscordWebhook, transcodedHighlightPath)).Wait();
            }

            if(!configuration.PreserveTranscoded && File.Exists(transcodedHighlightPath))
            {
                Logger.LogMessage($"Deleting transcoded Highlight: {transcodedHighlightPath}");
                File.Delete(transcodedHighlightPath);
            }
        }
        else if(configuration.DiscordShare)
        {
            Task.Run(() => Discord.WebhookAsync(configuration.DiscordUsername, configuration.DiscordAvatar, configuration.DiscordWebhook, savedHighlightPath)).Wait();
        }

        if(!configuration.PreserveOriginal && File.Exists(savedHighlightPath))
        {
            Logger.LogMessage($"Deleting original Highlight: {savedHighlightPath}");
            File.Delete(savedHighlightPath);
        }
    }

    private static string SaveHighlight(string highlight)
    {       
        var destinationFile = Path.Combine(configuration.PermanentFolder, Path.GetFileName(highlight));

        do
        {
            try
            {
                // Delaying save to ensure the Highlight is properly written to disk first, and not accessed by GeForce Experience
                Thread.Sleep(1000);
                
                if(configuration.MoveHighlights)
                {
                    Logger.LogMessage("Moving Highlight..");
                    File.Move(highlight, destinationFile);
                }
                else
                {
                    Logger.LogMessage($"Duplicating Highlight..");
                    File.Copy(highlight, destinationFile);
                }
            }
            catch(System.IO.IOException e)
            {
                Logger.LogError($"Error while saving Highlight: {e.Message}. Attempting again..");
            }
            catch(Exception e)
            {
                Logger.LogError($"Unexpected error while saving Highlight: {e.Message}.");
            }
        } while(!File.Exists(destinationFile));

        Logger.LogMessage($"Highlight successfully saved to Permanent folder.");

        return destinationFile;
    }

    private static void OnWatcherError(object eventSource, ErrorEventArgs eventData)
    {
        var exception = eventData.GetException();
        Logger.LogError($"Error while watching for newly recorded Highlights ({exception.GetType()}: {exception.Message})");
    } 
}