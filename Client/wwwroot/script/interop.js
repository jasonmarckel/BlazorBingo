export async function getLanguage() {
    return window.navigator.language;
}

export async function getUserAgent() {
    return window.navigator.userAgent;
}

// Detects if device is on iOS 
const isIosAgent = /iphone|ipad|ipod/.test(window.navigator.userAgent.toLowerCase());

export async function isIOS() {
    return isIosAgent;
}

export async function isInStandaloneMode() {
    return ('standalone' in window.navigator) && (window.navigator.standalone);
}

export async function isInstalled() {
    return deferredPrompt == null;
}

export async function installPWA() {
    try {
        // Show the install prompt
        deferredPrompt.prompt();
        // Wait for the user to respond to the prompt
        const { outcome } = await deferredPrompt.userChoice;
        // Optionally, send analytics event with outcome of user choice
        console.log(`User response to the install prompt: ${outcome}`);
        // We've used the prompt, and can't use it again, throw it away
        deferredPrompt = null;
    } catch (e) {
        console.log(`${e.name}, ${e.message}`);
    }
}

export async function showModal(id) {
    // includes workaround for older versions of iOS which do not
    // have support for the <dialog> element by manipulating the
    // display attribute of the <form> element nested within the 
    // <dialog> element.
    var hasDialogSupport = ((typeof HTMLDialogElement) === 'function');
    var dialogElement = document.getElementById(id);
    var formElement = dialogElement.firstElementChild;
    formElement.style.display = 'block';
    if (hasDialogSupport) {
        dialogElement.showModal();
    }
}

// https://developer.mozilla.org/en-US/docs/Web/API/SpeechSynthesisUtterance
// https://github.com/mdn/dom-examples/tree/main/web-speech-api/speak-easy-synthesis
// https://mdn.github.io/dom-examples/web-speech-api/speak-easy-synthesis/ // live demo
// https://talkrapp.com/speechSynthesis.html // lessons learned
// https://weboutloud.io/bulletin/speech_synthesis_in_safari/ // iOS-specific lessons learned
// https://codersblock.com/blog/javascript-text-to-speech-and-its-many-quirks/
// https://stackoverflow.com/questions/52769453/trying-to-use-speechsynthesis-api-in-ios-webview-app
// https://azure.microsoft.com/en-us/products/cognitive-services/text-to-speech/

const synth = window.speechSynthesis;
const utterance = new SpeechSynthesisUtterance();
const SilentUtteranceText = "ha";
let voices = [];

function populateVoiceList() {
    voices = synth.getVoices().sort(function (a, b) {      
        const aname = a.name.toUpperCase();
        const bname = b.name.toUpperCase();
        // sort by language and then by speaker name
        if (a.lang < b.lang) {
            return -1;
        } else if (a.lang > b.lang) {
            return +1;
        } else if (aname < bname) {
            return -1;
        } else if (aname == bname) {
            return 0;
        } else {
            return +1;
        }
    });
}

export async function getVoices() {
    var voiceList = [];
    for (let i = 0; i < voices.length; i++) {
        voiceList.push({
            "lang": voices[i].lang,
            "name": voices[i].name,
            "isDefault": voices[i].default,
            "localService": voices[i].localService
        });
    }
    return JSON.stringify(voiceList);
}

export async function initVoices() {
    populateVoiceList();
    if (speechSynthesis.onvoiceschanged !== undefined) {
        speechSynthesis.onvoiceschanged = populateVoiceList;
    }
}

export async function speak(inputText, voiceName, language) {
    // An inputText value of SilentUtteranceText is used to trigger a
    // silent utterance to prime text-to-speech synthesis on iOS devices
    // which require an action to be taken by the user (i.e. a button click) to engage 
    // the voice synthesis.  Other devices can skip this voice synthesis 'priming'.
    if (!isIOS() && inputText === SilentUtteranceText) { return; }

    if (synth.speaking) {
        console.error("speechSynthesis.speaking");
        return;
    } 

    if (inputText !== "") {
 
        //utterance.onerror = function (event) {
        //    console.error("SpeechSynthesisUtterance.onerror");
        //};

        for (let i = 0; i < voices.length; i++) {
            if (voices[i].name === voiceName && voices[i].lang == language) {
                utterance.voice = voices[i];
                console.log("Found match for requested voice.")
                break;
            }
        }
        utterance.text = inputText;
        utterance.pitch = 1.0;
        utterance.rate = inputText === SilentUtteranceText ? 2 : 1;
        utterance.volume = inputText === SilentUtteranceText ? 0 : 1;
        synth.speak(utterance);
    }
}

export async function primeSpeechSynthesis() {
    //console.log("primeSpeechSynthesis");
    speak(SilentUtteranceText, "", "");
}

// https://developer.mozilla.org/en-US/docs/Web/API/Screen_Wake_Lock_API

// Create a reference for the Wake Lock.
let wakeLock = null;

export async function requestWakeLock() {
    if ('wakeLock' in navigator) {
        // create an async function to request a wake lock
        try {
            wakeLock = await navigator.wakeLock.request('screen');
            wakeLock.addEventListener('release', () => {
                // the wake lock has been released
                console.log('Wake Lock has been released');
            });
        } catch (err) {
            // The Wake Lock request has failed - usually system related, such as battery.
            console.log(`${err.name}, ${err.message}`);
        }
    }
}

export async function releaseWakeLock() {
    if (wakeLock !== null) {
        wakeLock.release()
            .then(() => {
                wakeLock = null;
            });
    }
}


// references:
// https://github.com/pavelsavara/blazor-wasm-hands-pose/blob/main/DetectHandsJsComponent/DetectHands.razor.js
// https://peerjs.com/

function insertGlobalScript(url) {
    var element = document.createElement('script');
    element.setAttribute('src', url);
    element.setAttribute('crossorigin', 'anonymous');
    document.head.appendChild(element);

    return new Promise((resolve) => {
        element.onload = () => {
            resolve();
        };
    });
}

let dotnetExports;
let hostConnection;
let clientConnections = [];
const hostDomain = "net-marckel-blazorbingo-";
const peerJSscript = 'https://unpkg.com/peerjs@1.5.5/dist/peerjs.min.js';

export async function host(component, id) {

    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    dotnetExports = await getAssemblyExports("BlazorBingo.Client.dll");

    await insertGlobalScript(peerJSscript);

    var peer = new Peer(hostDomain + id);

    peer.on('open', function (id) {
        console.log('My Peer Id is: ' + id);
    });
    
    peer.on('connection', function (conn) {
        clientConnections.push(conn);
        var playerName = conn.metadata.playerName;
        console.log('received connection from ' + conn.peer + ' ' + playerName);
        dotnetExports.BlazorBingo.Client.Interop.OnDataReceived(component, "connected", playerName);

        conn.on('data', function (data) {
            //console.log(data);
            dotnetExports.BlazorBingo.Client.Interop.OnDataReceived(component, data.messageType, data.message);
        });
        conn.on('close', function () {
            //console.log('connection to ' + playerName + ' has been closed.');
            dotnetExports.BlazorBingo.Client.Interop.OnDataReceived(component, "disconnected", playerName);
        });
    });

    peer.on('error', function (err) {
        console.log('error ' + err.type);
    });
}

export async function connect(component, remoteId, playerName) {

    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    dotnetExports = await getAssemblyExports("BlazorBingo.Client.dll");

    await insertGlobalScript(peerJSscript);

    var peer = new Peer();

    peer.on('open', function (id) {
        console.log('My Peer Id is: ' + id);
        console.log('Remote Id: ' + remoteId);
        console.log('Player Name: ' + playerName);
        hostConnection = peer.connect(hostDomain + remoteId, {
            reliable: true,
            metadata: {
                playerName: playerName
            }
        });
        hostConnection.on("open", () => {
            console.log('connected');
        });
        hostConnection.on("data", (data) => {
            //console.log(data);
            dotnetExports.BlazorBingo.Client.Interop.OnDataReceived(component, data.messageType, data.message);
        });
        hostConnection.on('close', function () {
            console.log('connection to host has been closed.');
        });
    });

    peer.on('error', function (err) {
        console.log('error ' + err.type);
    });
}

export async function notifyHost(messageType, message) {
    //console.log(`notifyHost (${messageType}): ${message}`);
    let data = {
        messageType: messageType,
        message: message,
    };
    hostConnection.send(data);
}

export async function broadcast(messageType, message) {
    //console.log(`broadcast (${messageType}): ${message}`);
    let data = {
        messageType: messageType,
        message: message,
    };
    clientConnections.forEach(function (conn) {
        conn.send(data);
    });
}

export async function shareUrl(title, text, url) {
    if (navigator.share) {
        navigator.share({
            title: title,
            text: text,
            url: url,
        })
        .then(() => console.log('Successful share'))
        .catch((error) => console.log('Error sharing', error));
    } else {
        console.log('Share not supported on this browser.');
        window.navigator.clipboard.writeText(url);
    }
}
