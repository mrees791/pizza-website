// Namespace for manage menu image scripts
const pizzaSiteManageMenuImageNs = {
    updateImageElement: ($img) => {
        var imgSrc = $img.attr('src');
        $img.attr('src', imgSrc + '?dateTime=' + new Date().getTime());
    },
    initializeUploadSection: (actionUrl, imageId, errorMessageId, dropAreaId) => {
        var $imageElement = $(imageId);
        var $errorMessage = $(errorMessageId);
        var $dropArea = $(dropAreaId);
        $dropArea.dmUploader({
            url: actionUrl,
            maxFileSize: 1000000,
            extFilter: ['webp'],
            allowedTypes: 'image/webp',
            onInit: function () { },
            onNewFile: function (id, file) { },
            onBeforeUpload: function (id) { },
            onUploadProgress: function (id, percent) { },
            onUploadSuccess: function (id, data) {
                console.log(data);
                $errorMessage.hide();
                pizzaSiteManageMenuImageNs.updateImageElement($imageElement);
            },
            onUploadError: function (id, xhr, status, errorThrown) {
                console.log(xhr);
                $errorMessage.text(xhr.responseText.replaceAll('"', ''));
                $errorMessage.show();
            },
            onFileTypeError: function (file) {
                var errorMessage = 'Mime type must be image/webp.';
                console.log(errorMessage);
                $errorMessage.text(errorMessage);
                $errorMessage.show();
            },
            onFileSizeError: function (file) {
                var errorMessage = 'File size cannot exceed 1 megabyte.';
                console.log(errorMessage);
                $errorMessage.text(errorMessage);
                $errorMessage.show();
            },
            onFileExtError: function (file) {
                var errorMessage = 'File extension must be webp.';
                console.log(errorMessage);
                $errorMessage.text(errorMessage);
                $errorMessage.show();
            }
        });
    }
};