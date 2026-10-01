

let currentEditIndex = 0;
let rowIndex = 0;
var selectedIds = [];
function fnShowBannerModel(bannerNo, sectionType, title, content) {    
    $("#bannerIndex").val(bannerNo);
    $("#bannerSection").val(sectionType);
    previewGrid.innerHTML = "";
   
    currentEditIndex = null;
    let banner = getBannerData(bannerNo, sectionType);

    if (banner.title != undefined) {
        currentEditIndex = bannerNo;
        $("#bannerTitle").val(banner.title);
        $("#bannerSubTitle").val(banner.subTitle);
        $("#bannerContentTitle").val(banner.contentTitle);
        $("#displayOrder").val(banner.displayOrder);
        $("#existingImageUrl").val(banner.imageUrl);

        tinymce.get('bannerContent').setContent(banner.content);

        // Show existing image preview
        $("#previewGrid").html(`
       
        <div class="preview-item">
                <button type="button" class="remove-btn" id="removeImage">×</button>
                <img src="${banner.imageUrl}" />
            </div>
    `);
    }
    //}

    $("#fuModal").modal('show');
}

function previewStoryCardImage(event, previewId, placeholderId) {
    const file = event.target.files[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = function (e) {
        const img = document.getElementById(previewId);
        const placeholder = document.getElementById(placeholderId);
        const removeBtn = img.parentElement.querySelector('.story-remove-btn');

        img.src = e.target.result;
        img.classList.remove('d-none');
        if (placeholder) placeholder.classList.add('d-none');
        if (removeBtn) removeBtn.classList.remove('d-none');
    };
    reader.readAsDataURL(file);
}

function removeUploadCardImage(inputId, previewId, placeholderId, hiddenId) {
    const fileInput = document.getElementById(inputId);
    const img = document.getElementById(previewId);
    const placeholder = document.getElementById(placeholderId);
    const removeBtn = img.parentElement.querySelector('.story-remove-btn');
    const hd = document.getElementById(hiddenId);

    fileInput.value = '';
    img.src = '';
    img.classList.add('d-none');
    if (placeholder) placeholder.classList.remove('d-none');
    if (removeBtn) removeBtn.classList.add('d-none');
    if (hd) hd.value = '';
}


$("#fuModal").on("hidden.bs.modal", function () {
    $("#bannerTitle").val('');
    $("#displayOrder").val('');
    tinymce.get('bannerContent').setContent('');
});

function getBannerData(index, section) {
    let container = $(`.${section}-item[data-index="${index}"]`);

    return {
        title: container.find(`input[name="${section}[${index}].BannerTitle"]`).val(),
        subTitle: container.find(`input[name="${section}[${index}].BannerSubTitle"]`).val(),
        imageUrl: container.find(`input[name="${section}[${index}].ImageUrl"]`).val(),
        displayOrder: container.find(`input[name="${section}[${index}].DisplayOrder"]`).val(),
        content: container.find(`input[name="${section}[${index}].PopupContent"]`).val(),
        contentTitle: container.find(`input[name="${section}[${index}].PopupContentTitle"]`).val()
    };
}

$(document).ready(function () {


    selectedIds = $("#selectedVehicleIds").val().split(',').map(x => parseInt(x, 10))
        .filter(x => !isNaN(x));

    function toggleSection() {
        var pageType = $("#PageType").val();

        if (pageType == "1") { // Mission
            $(".clsVehicle").hide();
            $(".clsMission").show();
        } else {
            $(".clsVehicle").show();
            $(".clsMission").hide();
        }
    }

    // run on load
    toggleSection();

    // run on change
    $("#PageType").change(function () {
        toggleSection();
    });
});

$("#pageForm").submit(function () {
    $("#selectedVehicleIds").val(selectedIds.join(","));
});

$("#saveBanner").click(function () {
    debugger
    let title = $("#bannerTitle").val();
    let subtitle = $("#bannerSubTitle").val();
    let bannerIndex = $("#bannerIndex").val();
    let sectionType = $("#bannerSection").val();
    let popupContentTitle = $("#bannerContentTitle").val();
    let popupContent = tinymce.get('bannerContent').getContent();
    let displayOrder = parseInt($("#displayOrder").val(), 10);

    if (isNaN(displayOrder)) {
        displayOrder = 0;
    }

    let existingImage = $("#existingImageUrl").val();

    if (!selectedFile && !existingImage) {
        alert("Please select an image");
        return;
    }

    var formData = new FormData();

    formData.append("file", selectedFile);


    $.ajax({
        url: urlUploadBannerImage,
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (res) {

            /*if (res.success) {*/
            debugger
            uploadedImagePath = !existingImage ? res.filePath : existingImage;
            let index = currentEditIndex !== null ? currentEditIndex : bannerIndex;
            let previewImg,containerName;
            let html = `
                    <div class="${sectionType}-item" data-index="${index}">
                        <input type="hidden" name="${sectionType}[${bannerIndex}].ImageUrl" value="${uploadedImagePath}" />
                        <input type="hidden" name="${sectionType}[${bannerIndex}].BannerTitle" value="${title}" />
                        <input type="hidden" name="${sectionType}[${bannerIndex}].SectionType" value="${sectionType}" />
                        <input type="hidden" name="${sectionType}[${bannerIndex}].DisplayOrder" value="${displayOrder}" />
                        <input type="hidden" name="${sectionType}[${bannerIndex}].PopupContent" value="${popupContent}" />
                        <input type="hidden" name="${sectionType}[${bannerIndex}].PopupContentTitle" value="${popupContentTitle}" />
                        <input type="hidden" name="${sectionType}[${bannerIndex}].BannerSubTitle" value="${subtitle}" />
                    </div>
                    `;
            if (sectionType == 'Banners') {
               
                previewImg = "previewImage" + bannerIndex;
                containerName = "bannerContainer";
            }
            else if (sectionType == 'Performances') {
                
                previewImg = "prefromanceImage" + bannerIndex;
                containerName = "performanceContainer";
            } else if (sectionType == 'DrivingExperience') {
                
                previewImg = "DEImage" + bannerIndex;
                containerName = "driverExperienceContainer";
            } else if (sectionType == 'TNT') {
                
                previewImg = "TNTImage" + bannerIndex;
                containerName = "TNTContainer";
            }

            $("#fuModal").modal('hide');

            if (currentEditIndex !== null) {
                $(`.${sectionType}-item[data-index='${index}']`).replaceWith(html);
            } else {
                $("#" + containerName).append(html);
                bannerIndex++;
            }
            currentEditIndex = null;
            // Show preview
            $("#" + previewImg)
                .attr("src", res.filePath)
                .removeClass("d-none");

            selectedFile = null;
            $("#previewGrid").empty();
            $("#bannerTitle").val('');
            /*}*/
        }
    });
});

function addRow(content = '', title = '') {
    let html = `
    <tr>
        <td>
            <input type="text" name="AccordionItems[${rowIndex}].Title"
                class="form-control" value="${title}" />
        </td>
        <td>
            <textarea name="AccordionItems[${rowIndex}].Content"
                class="form-control">${content}</textarea>
        </td>
        <td>
            <button type="button" class="btn btn-danger" onclick="removeRow(this)">X</button>
        </td>
    </tr>`;

    $("#accordionTable tbody").append(html);
    rowIndex++;
}

function removeRow(btn) {
    $(btn).closest("tr").remove();
}

$(document).ready(function () {

    loadPage($('#LanguageId').val(), $('#hdPageId').val());
    loadFormContent($('#LanguageId').val(), $('#hdFormContentId').val());

    $('#LanguageId').change(function () {

        var languageId = $(this).val();
        loadPage(languageId);
        loadVehicle(languageId);
    });
    loadVehicle($('#hdLanguageId').val());
});

function loadPage(languageId, selectedId = 0) {

    // Clear dropdown
    $('#PageId').empty();
    $('#PageId').append('<option value="0">-- Select Page --</option>');

    if (languageId !== "") {

        $.ajax({
            url: urlGetPageByLanguage,
            type: 'GET',
            data: { languageId: languageId },
            success: function (data) {

                $.each(data, function (i, item) {

                    var selected = (item.id == selectedId) ? 'selected' : '';

                    $('#PageId').append(
                        `<option value="${item.id}" ${selected}>${item.name}</option>`
                    );
                });

            },
            error: function () {
                alert('Error loading parent menu');
            }
        });

    }
}

function loadFormContent(languageId, selectedId = 0) {

    // Clear dropdown
    $('#PageId').empty();
    $('#PageId').append('<option value="0">-- Select Form Content --</option>');

    if (languageId !== "") {

        $.ajax({
            url: urlGetFormContentByLanguage,
            type: 'GET',
            data: { languageId: languageId },
            success: function (data) {

                $.each(data, function (i, item) {

                    var selected = (item.id == selectedId) ? 'selected' : '';

                    $('#FormContentId').append(
                        `<option value="${item.id}" ${selected}>${item.name}</option>`
                    );
                });

            },
            error: function () {
                alert('Error loading parent menu');
            }
        });

    }
}

function loadVehicle(languageId) {


    if (languageId !== "") {

        $.ajax({
            url: urlGetVehicleListByLanguage,
            type: 'GET',
            data: { languageId: languageId },
            success: function (data) {
                $("#vehicleBody").html(data);
            },
            error: function () {
                alert('Error loading parent menu');
            }
        });

    }
}

function addVehicle() { $("#vehicleModal").modal('show'); }
