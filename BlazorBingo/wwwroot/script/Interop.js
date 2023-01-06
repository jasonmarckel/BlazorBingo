const peerjs = window;

// references:
// https://github.com/pavelsavara/blazor-wasm-hands-pose/blob/main/DetectHandsJsComponent/DetectHands.razor.js

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
let conns = [];
const hostDomain = "net-marckel-blazorbingo-";

export async function host(component, id) {

    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    dotnetExports = await getAssemblyExports("BlazorBingo.dll");

    await insertGlobalScript('../lib/peerjs/peerjs.min.js');

    var peer = new peerjs.Peer(hostDomain + id);

    peer.on('open', function (id) {
        console.log('My Peer Id is: ' + id);
    });
    
    peer.on('connection', function (conn) {
        conns.push(conn);
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

    await insertGlobalScript('../lib/peerjs/peerjs.min.js');

    var peer = new peerjs.Peer();

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
    conns.forEach(function (conn) {
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
