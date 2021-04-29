// Namespace for Manage Addresses view scripts
const pizzaSiteManageAddressesNs = {
    updateAddressListElementVisibility: () => {
        // Get number of rows in table
        var numberOfAddressItems = $('#addressTableBody').children().length;
        var cartIsEmpty = numberOfAddressItems === 0;
        if (cartIsEmpty) {
            $('#addressListBlock').hide();
            $('#emptyAddressListBlock').show();
        } else {
            $('#emptyAddressListBlock').hide();
            $('#addressListBlock').show();
        }
    }
};