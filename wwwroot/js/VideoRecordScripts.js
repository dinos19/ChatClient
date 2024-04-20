let videoElement = document.getElementById('videoElement');
let mediaRecorder;
let recordedBlobs;
let stream;
let isRecording = false;
function getSupportedMimeTypes() {
    const types = [
        'video/webm; codecs=vp9', // Preferably high-quality codec
        'video/webm; codecs=vp8', // Commonly supported in many browsers
        'video/webm',             // Fallback to default WebM
        'video/mp4'               // Not typically supported but included for completeness
    ];

    return types.filter(type => MediaRecorder.isTypeSupported(type));
}
async function startCamera() {
    stream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
    videoElement.srcObject = stream;
}

export async function startRecording() {
    await startCamera();
    recordedBlobs = [];
    const options = { mimeType: getSupportedMimeTypes()[0] };

    try {
        mediaRecorder = new MediaRecorder(stream, options);
        mediaRecorder.ondataavailable = (event) => {
            console.log(`Data size: ${event.data.size}`); // Check the size of each data chunk
            if (event.data && event.data.size > 0) {
                recordedBlobs.push(event.data);
            }
        };
        mediaRecorder.start(1000);
    } catch (e) {
        console.error('Exception while creating MediaRecorder:', e);
        return;
    }
}

export function toggleRecording() {
    if (!mediaRecorder) return;
    if (isRecording) {
        mediaRecorder.pause();
    } else {
        mediaRecorder.resume();
    }
    isRecording = !isRecording;
}

export async function stopRecording() {
    if (!mediaRecorder) return null;
    mediaRecorder.stop();
    mediaRecorder.stream.getTracks().forEach(track => track.stop());

    const blob = new Blob(recordedBlobs, { type: getSupportedMimeTypes()[0] });

    return new Promise((resolve, reject) => {
        let reader = new FileReader();
        reader.onload = () => {
            const arrayBuffer = reader.result;
            const byteArray = new Uint8Array(arrayBuffer);
            resolve(byteArray);
        };
        reader.onerror = (err) => reject(err);
        reader.readAsArrayBuffer(blob);
    });
}

function fallbackDownload(blob) {
    let url = window.URL.createObjectURL(blob);
    let a = document.createElement('a');
    a.style.display = 'none';
    a.href = url;
    a.download = 'recorded_video.mp4';
    document.body.appendChild(a);
    a.click();
    setTimeout(() => {
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
    }, 100);
}

window.onunload = () => {
    if (stream) {
        stream.getTracks().forEach(track => track.stop());
    }
}