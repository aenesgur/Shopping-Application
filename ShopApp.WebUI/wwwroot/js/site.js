
function validateFileType() {
        var fileName = document.getElementById("fileName").value;
    var idxDot = fileName.lastIndexOf(".") + 1;
    var extFile = fileName.substr(idxDot, fileName.length).toLowerCase();
        if (extFile == "jpg" || extFile == "jpeg" || extFile == "png") {
        //TO DO
    } else {
        alert("Only .jpg .jpeg and .png files are allowed!");
        document.getElementById("fileName").value = "";
}
}


