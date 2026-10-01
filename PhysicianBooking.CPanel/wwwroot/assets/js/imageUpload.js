
//let filesArr = [];

//const dropArea = document.getElementById("dropArea");
//const fileInput = document.getElementById("fup");
//const previewGrid = document.getElementById("previewGrid");

//// Click
//dropArea.onclick = () => fileInput.click();

//// Select
//fileInput.onchange = (e) => handleFiles(e.target.files);

//// Drag
//dropArea.ondragover = e => e.preventDefault();
//dropArea.ondrop = e => {
//    e.preventDefault();
//    handleFiles(e.dataTransfer.files);
//};

//// Handle files
//function handleFiles(files) {
//    [...files].forEach(file => {
//        file.id = Date.now() + Math.random();
//        filesArr.push(file);
//        renderPreview(file);
//    });
//}

//// Preview
//function renderPreview(file) {
//    const reader = new FileReader();

//    reader.onload = e => {
//        const div = document.createElement("div");
//        div.className = "preview-item";
//        div.dataset.id = file.id;

//        div.innerHTML = `
//    <button class="remove-btn">×</button>
//    <img src="${e.target.result}" />
//    <div class="progress"><div class="progress-bar"></div></div>
//    `;

//        // Remove
//        div.querySelector(".remove-btn").onclick = () => {
//            filesArr = filesArr.filter(f => f.id !== file.id);
//            div.remove();
//        };

//        previewGrid.appendChild(div);
//    };

//    reader.readAsDataURL(file);
//}

//// Reorder
//new Sortable(previewGrid, {
//    animation: 150,
//    onEnd: () => {
//        let newOrder = [];
//        document.querySelectorAll(".preview-item").forEach(item => {
//            let id = item.dataset.id;
//            let file = filesArr.find(f => f.id == id);
//            if (file) newOrder.push(file);
//        });
//        filesArr = newOrder;
//    }
//});

//function closeModel() {
//    $('.modal.show').modal('hide');
//}

let selectedFile = null;

const dropArea = document.getElementById("dropArea");
const fileInput = document.getElementById("fup");
const previewGrid = document.getElementById("previewGrid");

// Click
dropArea.onclick = () => fileInput.click();

// Select
fileInput.onchange = (e) => handleFile(e.target.files[0]);

// Drag
dropArea.ondragover = e => e.preventDefault();

dropArea.ondrop = e => {
    e.preventDefault();

    if (e.dataTransfer.files.length > 0) {
        handleFile(e.dataTransfer.files[0]);
    }
};

// Handle single file
function handleFile(file) {

    if (!file) return;

    selectedFile = file;

    // Remove existing preview
    previewGrid.innerHTML = "";

    renderPreview(file);
}

// Preview
function renderPreview(file) {

    const reader = new FileReader();

    reader.onload = function (e) {

        previewGrid.innerHTML = `
            <div class="preview-item">
                <button type="button" class="remove-btn" id="removeImage">×</button>
                <img src="${e.target.result}" />
            </div>
        `;

        document.getElementById("removeImage").onclick = function () {
            selectedFile = null;
            fileInput.value = "";
            previewGrid.innerHTML = "";
            $("#existingImageUrl").val('');
        };
    };

    reader.readAsDataURL(file);
}

function closeModel() {
    $('.modal.show').modal('hide');
}
