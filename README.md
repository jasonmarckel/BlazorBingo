# BlazorBingo

* Blazor Web Assembly (WASM) / C# / .NET 7.0 / Progressive Web App (PWA)
* [PeerJS](https://peerjs.com/) is used via JSInterop to establish peer-to-peer communication between the game host and the players.  No server required!
* The Web Speech API and SpeechSynthesisUtterance (text-to-speech) is used to announce the numbers called on the player instances.
* Platform-native emoji characters are used for stamping the bingo cards.
* Screen Wake Lock API is used to keep the screen from timing out / turning off on devices that have support for it.
* GitHub Actions is used to build and publish the Blazor WASM app to GitHub pages.

Play a game at https://jasonmarckel.github.io/BlazorBingo.

Select 'Host' to start a new *Bingo Hall* and generate the game code for the session.  Enter the game code in the other instances and select 'Join'.

BlazorBingo can be installed/pinned to the homescreen and launched as a full screen PWA web app without the browser's chome around it.

**TODO**:
* Add special effects for winner (confetti, fireworks, etc.)
* User Interface (UI) improvements
* Implement best practice for PWA app to notify that a new version is available and self-update.
  * Currently for an installed/pinned PWA, it must be launched, killed, and re-opened several times for it to detect, install, and use the latest version.
* Add statistics (games played, games won, minutes/hours/days played).
* Create an app icon!
