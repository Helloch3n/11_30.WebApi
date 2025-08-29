window.downloadFile = (fileName, bytesBase64) => {
    const blob = new Blob([new Uint8Array(bytesBase64)], { type: "application/octet-stream" });
    const url = URL.createObjectURL(blob);

    const anchorElement = document.createElement("a");
    anchorElement.href = url;
    anchorElement.download = fileName ?? '导出.xlsx';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
};
