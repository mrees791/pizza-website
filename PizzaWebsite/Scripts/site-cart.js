// Namespace for Cart view scripts
const pizzaSiteCartNs = {
    updateCartElementVisibility: () => {
        // Get number of rows in table
        var numberOfCartItems = $('#cartTableBody').children().length;
        var cartIsEmpty = numberOfCartItems === 0;
        if (cartIsEmpty) {
            $('#cartBlock').hide();
            $('#emptyCartBlock').show();
        } else {
            $('#emptyCartBlock').hide();
            $('#cartBlock').show();
        }
    }
};