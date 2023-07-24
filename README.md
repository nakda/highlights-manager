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
You just have to look at the "config.toml" file, where everything is explained.