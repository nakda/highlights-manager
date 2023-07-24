# What are NVIDIA Highlights?
NVIDIA Highlights is a feature provided by NVIDIA's GeForce Experience software which offers players the ability to automatically capture and save memorable in-game moments without the need for manual recording.

# How does it work?
Game developers can trigger Highlights recording automatically by signaling to GeForce Experience that an important event occured.

# How can I enable it?
Within the GeForce Experience settings, you can find an entry for Highlights wich can be toggled ON/OFF. When launching your game for the first time, you may be prompted to authorize or not Highlights. At any time, while the game is launched, you can access the GeForce Experience's Highlights settings to enable/disable Highlights for specific events (eg. for Hunt: Showdown you can have Highlights everytime you Kill someone, or everytime you get killed).

# Can I tweak the recording settings?
To some extent yes. The duration of the highlights is controlled by the game developers, unless they expose extra settings for you (I'm not sure if it's possible). For example in Hunt: Showdown a "kill" Highlight's duration is 25 seconds: 20 seconds before the kill, and the 5 following seconds. On your end, you can control the resolution, bitrate and framerate via the settings offered by GeForce Experience.

# Why am I not seeing a NVIDIA notification about Highlights being saved?
Developers can decide to hide this banner for multiple reasons. It is the case in Hunt: Showdown, and my guess is that in addition to creating distraction for the user and potentially hiding important information on the screen, knowing or not that you killed someone is an information they are not willing to share (even when it's obvious you confirmed the kill).

# Why does the audio of my microphone isn't recorded?
In addition to checking all your settings, I have noticed that the user's microphone input is not recorded if GeForce Experience is set to record separated audio tracks. Go into the Audio settings of GeForce Experience, and make sure it's set to record a single track.

# Why is this application needed if everything is automated?
The way Highlights works is the automated recordings are temporarily saved to a folder until you had time to review them. You can then decide to share them online and/or save them permanently on your computer. Developers have the responsibility to select what's the best moment to offer you the time to review it (for Hunt: Showdown it's after a game, when you are seeing the cards).
The thing is, if you forget to save them or if your game crash you will lose those precious Highlights. This application monitors the temporary folder where NVIDIA Highlights are stored and move them automatically to the permanent place of your choice. 

In addition, this application also offers the possibility to automatically share your clip on a Discord channel via webhooks (note that you can't share it on any channel, because you need to have the permissions to create/retrieve the webhook id of the channel). Discord restricting file uploads to 25 MB, this application also offers to automatically transcode the Highlights so it can fit for a Discord upload (FFMPEG has to be installed and added to the PATH environment variable).

# Any requirements for this application?
FFMPEG should be installed and added to PATH environment variable if transcoding is expected, and both a NVIDIA GeForce graphics card and NVIDIA GeForce Experience should be ready to use.

# How can I configure this application?
The following documentation provides details on the various options available in the configuration file. The configuration file uses the TOML format to specify settings related to the temporary and permanent storage of Highlights, transcoding options, Discord sharing, and other preferences.

## Configuration Options

### `temporary_folder` (string)

Specifies the folder path where NVIDIA GeForce Experience saves temporary highlights. The value you need to set this with can be retrieved in the GeForce Experience Highlights settings. If the path contains backslashes, they need to be escaped (e.g., "C:\Temp" becomes "C:\\Temp"). This field cannot be empty.

### `permanent_folder` (string)

Defines the folder path where you want your Highlights to be permanently stored. Similar to `temporary_folder`, you need to escape backslashes if present (e.g., "C:\Temp" becomes "C:\\Temp"). This field cannot be empty.

### `move_highlights` (boolean)

Determines whether the Highlights should be moved from the temporary folder to the permanent folder. If set to `true`, the files will be moved, saving disk space and avoiding unnecessary duplication. However, if set to `false`, the Highlights will be duplicated and thus available in both locations. If you want to preserve native handling of the Highlights through the GeForce Experience UI (e.g., sharing to YouTube), you should set this to `false`.

### `transcode_highlights` (boolean)

Indicates whether the Highlights should be transcoded. Transcoding allows you to trim the videos or reduce their file size. To enable transcoding, you must have FFMPEG installed and added to the PATH environment variable. If set to `true`, the transcoding process will be applied.

### `transcode_trim` (boolean)
### `transcode_start_time` (string)
### `transcode_end_time` (string)

If `transcode_highlights` is set to `true`, you can use these options to trim the Highlights during the transcoding process. `transcode_trim` enables or disables trimming, while `transcode_start_time` and `transcode_end_time` specify the start and end times for the trimming. By default, in Hunt: Showdown, the Highlights last 25 seconds with the kill happening at the 20 seconds mark.

### `transcode_size` (integer)

Defines the approximate target file size (in MB) for the transcoded Highlights. Lowering this value will result in reduced video quality. Set `transcode_size` to 0 if you don't want to enforce any file size limitation during transcoding.

### `preserve_original` (boolean)
### `preserve_transcoded` (boolean)

After transcoding, these options allow you to decide whether to preserve the original Highlights, the transcoded version, both, or neither. If `preserve_original` is set to `true`, the original Highlights will be kept. Similarly, if `preserve_transcoded` is set to `true`, the transcoded versions will be retained.

### `discord_share` (boolean)
### `discord_webhook` (string)
### `discord_username` (string)
### `discord_avatar` (string)

These options are related to sharing Highlights to Discord. If `discord_share` is set to `true`, the application will automatically share Highlights to Discord. The Discord webhook's URL should be provided in `discord_webhook`. You can set `discord_username` and `discord_avatar` to specify a default username and avatar, respectively, when uploading to the Discord channel. The avatar must be a publicly accessible URL to an image. Note that if you are not a Nitro-users, you will have to set `transcode_size` to `25` to comply with Discord upload restrictions.

## Example Configuration

This configuration will automatically move captured Highlights to the `permanent_folder`, transcode the Highlight to make sure it fits Discord upload restrictions for non-Nitro users (25 MB), and trim the Highlight to only preserve a specific segment. Afterwards, it will delete the transcoded version and only preserve the original (untrimmed).

```toml
temporary_folder = "C:\\Temp\\GeForceHighlights"
permanent_folder = "C:\\Highlights\\Saved"
move_highlights = true
transcode_highlights = true
transcode_trim = true
transcode_start_time = "00:00:15"
transcode_end_time = "00:00:23"
transcode_size = 25
preserve_original = true
preserve_transcoded = false
discord_share = false
discord_webhook = "https://discord.com/api/webhooks/your-webhook-url"
discord_username = "GeForceBot"
discord_avatar = "https://example.com/geforcebot-avatar.png"
