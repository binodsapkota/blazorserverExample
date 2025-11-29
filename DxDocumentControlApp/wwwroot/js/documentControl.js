// Trigger a hidden InputFile click
window.triggerInputFileClick = (elementId) => {
    const input = document.getElementById(elementId);
    if (input) input.click();
};

// Download a file from .NET stream
window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);

    const a = document.createElement("a");
    a.href = url;
    a.download = fileName ?? "download";
    a.click();

    URL.revokeObjectURL(url);
};
