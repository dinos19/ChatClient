function scrollToBottom(element) {
    console.log("Attempting to scroll to bottom...");
    if (element) {
        console.log("Element found, scrolling now.");
        element.scrollTop = element.scrollHeight;
    } else {
        console.error("Element not available for scrolling");
    }
}

function createVideoUrlFromByteArray(byteArray, mimeType) {
    const blob = new Blob([byteArray], { type: mimeType });
    return URL.createObjectURL(blob);
}

window.setVideoSource = (videoElement, byteArray, mimeType) => {
    const blob = new Blob([byteArray], { type: mimeType });
    const url = URL.createObjectURL(blob);
    console.log("Generated Blob URL:", url); // Log URL for debugging

    //const videoElement = document.getElementById(elementId);
    if (videoElement) {
        videoElement.src = url;
        videoElement.load(); // Ensure the video loads the new source
        videoElement.onloadedmetadata = () => console.log("Video metadata loaded.");
        videoElement.onerror = () => console.error("Error loading video.");
    } else {
        console.error("Video element not found:", elementId);
    }
};