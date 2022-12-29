

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

export async function host(component, id) {

    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    peerjsExports = await getAssemblyExports("BlazorBingo.dll");

    await insertGlobalScript('../lib/peerjs/peerjs.min.js');
    const peer = new peerjs.Peer(id);

    peer.on('connection', function (conn) {


//        conn.on('data', function (data) {
//            peerjsExports.PeerInterop.OnReceive(component, data);
//            console.log(data);
//        });
    });

    //hands.onResults(results => onResults(component, results));
    //console.log("peer started");
}


export async function connect(component, id) {

    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    peerjsExports = await getAssemblyExports("BlazorBingo.dll");

    await insertGlobalScript('../lib/peerjs/peerjs.min.js');
    const peer = new peerjs.Peer();

    var conn = peer.connect(id, {
        serialization: "json",
        reliable: true
    });

    peerConn = conn;

    // on open will be launch when you successfully connect to PeerServer
    conn.on('open', function () {
        conn.on('data', function (data) {
            peerjsExports.PeerInterop.OnReceive(component, data);
            console.log(data);
        });      
    });
}

export async function send(component, message) {
    console.log(message);
    peerConn.send(message);
}

export async function broadcast(component, message) {
    console.log(message);
    peerConn.send(message);
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
