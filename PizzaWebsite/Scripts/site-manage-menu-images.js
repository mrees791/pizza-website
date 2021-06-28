// Namespace for manage menu image scripts
const pizzaSiteManageMenuImageNs = {
    updateImageElement: ($img) => {
        var imgSrc = $img.attr('src');
        $img.attr('src', imgSrc + '?dateTime=' + new Date().getTime());
    },
    initializeManageMenuIconSection: (actionUrl) => {
        var $menuIconImg = $('#menuIcon');
        var $menuIconError = $('#menuIconError');
        var $menuIconDropArea = $('#menuIconDropArea');
        $menuIconDropArea.dmUploader({
            url: actionUrl,
            onInit: function () { },
            onNewFile: function (id, file) { },
            onBeforeUpload: function (id) { },
            onUploadProgress: function (id, percent) { },
            onUploadSuccess: function (id, data) {
                console.log(data);
                $menuIconError.hide();
                pizzaSiteManageMenuImageNs.updateImageElement($menuIconImg);
            },
            onUploadError: function (id, xhr, status, errorThrown) {
                console.log(xhr);
                $menuIconError.text(xhr.responseText.replaceAll('"', ''));
                $menuIconError.show();
            }
        });
    }
};