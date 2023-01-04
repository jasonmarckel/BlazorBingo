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

export async function host(id) {

    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    dotnetExports = await getAssemblyExports("BlazorBingo.dll");

    await insertGlobalScript('../lib/peerjs/peerjs.min.js');

    var peer = new peerjs.Peer(hostDomain + id);

    peer.on('open', function (id) {
        console.log('My Peer Id is: ' + id);
    });
    
    peer.on('connection', function (conn) {
        conns.push(conn);
        console.log('received connection from ' + conn.peer + ' ' + conn.metadata.playerName);

        dotnetExports.BlazorBingo.PeerInterop.OnConnected(conn.metadata.playerName);

        conn.on('data', function (data) {
            console.log(data);
            dotnetExports.BlazorBingo.PeerInterop.OnDataReceived(data);
        });
    });

    peer.on('error', function (err) {
        console.log('error ' + err.type);
    });
}

export async function connect(remoteId, playerName) {

    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    dotnetExports = await getAssemblyExports("BlazorBingo.dll");

    await insertGlobalScript('../lib/peerjs/peerjs.min.js');

    var peer = new peerjs.Peer();

    peer.on('open', function (id) {
        console.log('My Peer Id is: ' + id);
        console.log('Remote Id:' + remoteId);
        console.log('Player Name:' + playerName);
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
            console.log(data);
            dotnetExports.BlazorBingo.PeerInterop.OnDataReceived(data);
        });
    });

    peer.on('error', function (err) {
        console.log('error ' + err.type);
    });
}

export async function notifyHost(message) {
    console.log('notifyHost: ' + message);
    hostConnection.send(message);
}

export async function broadcast(message) {
    console.log('broadcast: ' + message);
    conns.forEach(function (conn) {
        conn.send(message);
    });
}

/*
const spinner = document.querySelector('.loading');
spinner.ontransitionend = () => {
    spinner.style.display = 'none';
};
*/
