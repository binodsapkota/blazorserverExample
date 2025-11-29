window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchor = document.createElement("a");
    anchor.href = url;
    anchor.download = fileName ?? "download";
    anchor.click();
    URL.revokeObjectURL(url);
};

window.triggerInputFileClick = (elementId) => {
    const el = document.getElementById(elementId);
    if (el) el.click();
};