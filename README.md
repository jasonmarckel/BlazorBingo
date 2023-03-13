# BlazorBingo

* Blazor Web Assembly (WASM) / .NET 7.0 / Progressive Web App (PWA)
* PeerJS is used via JSInterop to establish peer-to-peer communication between the game host and the players.  No server required!
* The Web Speech API and SpeechSynthesisUtterance (text-to-speech) is used to announce the numbers called on the player instances.
* Platform-native emoji characters are used for stamping the bingo cards.
* Screen Wake Lock API is used to keep the screen from timing out / turning off on devices that have support for it.
* GitHub Actions is used to build and publish the Blazor WASM app to GitHub pages.

Play a game at https://jasonmarckel.github.io/BlazorBingo.

Use multiple tabs in a single browser instance, multiple browser instances, or multiple devices.  Select 'Host' to start a new *Bingo Hall* and generate the game code for the session.  Enter the game code in the other instances and select 'Join'.

BlazorBingo can be installed/pinned to the homescreen and launched as a full screen PWA web app without the browser's chome around it.

Note: there are some peculiarities to updating PWA apps (not specific to Blazor).  Launch the app, and it detects a new version in the background.  Close all instances and launch again to get the updated version.

**TODO**:
* Add special effects for winner (confetti, fireworks, etc.)
* User Interface (UI) improvements
  * Adjust layout for landscape display.
* Figure out a better way of the PWA app to self-update.
  * Maybe notify that a new version is available and instruct the user to close and re-launch?
* Add statistics (games played, games won, minutes/hours/days played).
