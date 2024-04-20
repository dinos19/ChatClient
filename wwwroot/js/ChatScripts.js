function scrollToBottom(element) {
    console.log("Attempting to scroll to bottom...");
    if (element) {
        console.log("Element found, scrolling now.");
        element.scrollTop = element.scrollHeight;
    } else {
        console.error("Element not available for scrolling");
    }
}