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
            maxFileSize: 1000000,
            extFilter: ['jpg'],
            allowedTypes: 'image/jpeg',
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
            },
            onFileTypeError: function (file) {
                var errorMessage = 'Mime type must be image/jpeg.';
                console.log(errorMessage);
                $iconErrorMessage.text(errorMessage);
                $iconErrorMessage.show();
            },
            onFileSizeError: function (file) {
                var errorMessage = 'File size cannot exceed 1 megabyte.';
                console.log(errorMessage);
                $iconErrorMessage.text(errorMessage);
                $iconErrorMessage.show();
            },
            onFileExtError: function (file) {
                var errorMessage = 'File extension must be jpg.';
                console.log(errorMessage);
                $iconErrorMessage.text(errorMessage);
                $iconErrorMessage.show();
            }
        });
    }
};