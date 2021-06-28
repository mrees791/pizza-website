// Namespace for manage menu image scripts
const pizzaSiteManageMenuImageNs = {
    updateImageElement: ($img) => {
        var imgSrc = $img.attr('src');
        $img.attr('src', imgSrc + '?dateTime=' + new Date().getTime());
    },
    initializeManagePizzaBuilderIconSection: (actionUrl) => {
        pizzaSiteManageMenuImageNs.initializeManageMenuImageSection(actionUrl, $('#pizzaBuilderIcon'), $('#pizzaBuilderIconError'), $('#pizzaBuilderIconDropArea'))
    },
    initializeManageMenuImageSection: (actionUrl, $iconImg, $iconErrorMessage, $iconDropArea) => {
        $iconDropArea.dmUploader({
            url: actionUrl,
            onInit: function () { },
            onNewFile: function (id, file) { },
            onBeforeUpload: function (id) { },
            onUploadProgress: function (id, percent) { },
            onUploadSuccess: function (id, data) {
                console.log(data);
                $iconErrorMessage.hide();
                pizzaSiteManageMenuImageNs.updateImageElement($iconImg);
            },
            onUploadError: function (id, xhr, status, errorThrown) {
                console.log(xhr);
                $iconErrorMessage.text(xhr.responseText.replaceAll('"', ''));
                $iconErrorMessage.show();
            }
        });
    }
};