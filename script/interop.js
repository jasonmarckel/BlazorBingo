// https://developer.mozilla.org/en-US/docs/Web/API/SpeechSynthesisUtterance
// https://github.com/mdn/dom-examples/tree/main/web-speech-api/speak-easy-synthesis
// https://mdn.github.io/dom-examples/web-speech-api/speak-easy-synthesis/

const synth = window.speechSynthesis;
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
    populateVoiceList();
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

//export async function initVoices() {
//    populateVoiceList();
//    if (speechSynthesis.onvoiceschanged !== undefined) {
//        speechSynthesis.onvoiceschanged = populateVoiceList;
//    }
//}

export async function speak(inputText, voiceName) {
    if (synth.speaking) {
        console.error("speechSynthesis.speaking");
        return;
    } 

    if (inputText !== "") {
        const utterThis = new SpeechSynthesisUtterance(inputText);

        utterThis.onend = function (event) {
            //console.log("SpeechSynthesisUtterance.onend");
        };

        utterThis.onerror = function (event) {
            //console.error("SpeechSynthesisUtterance.onerror");
        };

        if (voices.length === 0) { voices = synth.getVoices(); }

        for (let i = 0; i < voices.length; i++) {
            if (voices[i].name === voiceName) {
                utterThis.voice = voices[i];
                break;
            }
        }
        //utterThis.pitch = pitch.value;
        //utterThis.rate = rate.value;
        synth.speak(utterThis);
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

export async function host(component, id) {

    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    dotnetExports = await getAssemblyExports("BlazorBingo.dll");

    await insertGlobalScript('https://unpkg.com/peerjs@1.4.7/dist/peerjs.min.js');

    var peer = new Peer(hostDomain + id);

    peer.on('open', function (id) {
        console.log('My Peer Id is: ' + id);
    });
    
    peer.on('connection', function (conn) {
        clientConnections.push(conn);
        var playerName = conn.metadata.playerName;
        //console.log('received connection from ' + conn.peer + ' ' + playerName);
        dotnetExports.BlazorBingo.Interop.OnDataReceived(component, "connected", playerName);

        conn.on('data', function (data) {
            //console.log(data);
            dotnetExports.BlazorBingo.Interop.OnDataReceived(component, data.messageType, data.message);
        });
        conn.on('close', function () {
            //console.log('connection to ' + playerName + ' has been closed.');
            dotnetExports.BlazorBingo.Interop.OnDataReceived(component, "disconnected", playerName);
        });
    });

    peer.on('error', function (err) {
        console.log('error ' + err.type);
    });
}

export async function connect(component, remoteId, playerName) {

    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    dotnetExports = await getAssemblyExports("BlazorBingo.dll");

    await insertGlobalScript('https://unpkg.com/peerjs@1.4.7/dist/peerjs.min.js');

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
            dotnetExports.BlazorBingo.Interop.OnDataReceived(component, data.messageType, data.message);
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
        console.log('Share not supported on this browser, do it the old way.');
    }
}
