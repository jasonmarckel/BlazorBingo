

const peerjs = window;

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

let peerjsExports;

let peer;
let conn;

export async function host(id) {

    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    peerjsExports = await getAssemblyExports("BlazorBingo.dll");

    await insertGlobalScript('../lib/peerjs/peerjs.min.js');
    peer = new peerjs.Peer(id);

    peer.on('open', function (id) {
        console.log('My Peer Id is: ' + id);
    });
    
    peer.on('connection', function (conn2) {
        conn = conn2;
        console.log('received connection from ' + conn.peer + ' ' + conn.metadata.playerName);
        conn.on('data', function (data) {
//            peerjsExports.PeerInterop.OnReceive(component, data);
            console.log(data);
        });
    });
}

export async function connect(remoteId, playerName) {

    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    peerjsExports = await getAssemblyExports("BlazorBingo.dll");

    await insertGlobalScript('../lib/peerjs/peerjs.min.js');
    peer = new peerjs.Peer();

    peer.on('open', function (id) {
        console.log('My Peer Id is: ' + id);
        console.log('Remote Id:' + remoteId);
        console.log('Player Name:' + playerName);
        conn = peer.connect(remoteId, {
            reliable: true,
            metadata: {
                playerName: playerName
                }
        });
        conn.on("open", () => {
            console.log('connected');
            //conn.send("hi from " + id);
        });
        conn.on("data", (data) => {
            console.log(data);
        });
    });

    peer.on('error', function (err) {
        console.log('error ' + err.type);
    });
}

export async function send(id, message) {
    console.log('send: ' + message + ' to ' + id);
    conn.send(message);
}

export async function broadcast(message) {
    console.log('broadcast: ' + message);
    conn.send(message);
}

/*
const spinner = document.querySelector('.loading');
spinner.ontransitionend = () => {
    spinner.style.display = 'none';
};
*/


/*
function onResults(component, results) {
    document.body.classList.add('loaded');
    canvasCtx.save();
    canvasCtx.clearRect(0, 0, canvasElement.width, canvasElement.height);
    canvasCtx.drawImage(results.image, 0, 0, canvasElement.width, canvasElement.height);
    if (results.multiHandLandmarks && results.multiHandedness) {
        for (let index = 0; index < results.multiHandLandmarks.length; index++) {
            const classification = results.multiHandedness[index];
            const isRightHand = classification.label === 'Right';
            const landmarks = results.multiHandLandmarks[index];
            drawingUtils.drawConnectors(canvasCtx, landmarks, mpHands.HAND_CONNECTIONS, { color: isRightHand ? '#00FF00' : '#FF0000' });
            drawingUtils.drawLandmarks(canvasCtx, landmarks, {
                color: isRightHand ? '#00FF00' : '#FF0000',
                fillColor: isRightHand ? '#FF0000' : '#00FF00',
                radius: (data) => {
                    return drawingUtils.lerp(data.from.z, -0.15, .1, 10, 1);
                }
            });
        }
    }
    canvasCtx.restore();
    if (results.multiHandedness && results.multiHandLandmarks) {
        const hands = [];
        for (let index = 0; index < results.multiHandLandmarks.length; index++) {
            const classification = results.multiHandedness[index];
            const landmarks = results.multiHandLandmarks[index];
            hands.push({
                ...classification,
                landmarks
            })
        }

        const json = JSON.stringify({ hands });
        detectHandsExports.DetectHandsJsComponent.DetectHands.Interop.OnResults(component, json);
    }
}

*/
