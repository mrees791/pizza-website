// Namespace for Cart view scripts
const pizzaSiteCartNs = {
    updateCartSubtotal: (cartId) => {
        var $subtotalCell = $("#subtotalCell");

        var params = { cartId: cartId };
        pizzaSiteNs.ajaxCall("/Shop/GetCartSubtotalAjax", JSON.stringify(params), "POST").fail(function(response) {
                console.log(response);
            })
            .done(function(response) {
                $subtotalCell.html(response);
            });
    },
    updateCartElementVisibility: () => {
        var $cartTableBody = $("#cartTableBody");
        var $cartBlock = $("#cartBlock");
        var $emptyCartBlock = $("#emptyCartBlock");

        // Get number of rows in table
        var numberOfCartItems = $cartTableBody.children().length;
        // If it only has the subtotal row, then the cart is empty;
        var cartIsEmpty = numberOfCartItems === 1;
        if (cartIsEmpty) {
            $cartBlock.hide();
            $emptyCartBlock.show();
        } else {
            $emptyCartBlock.hide();
            $cartBlock.show();
        }
    }
};