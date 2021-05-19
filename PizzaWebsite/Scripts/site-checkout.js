// Namespace for Checkout view scripts
const pizzaSiteCheckoutNs = {
    initializeOrderTypeSelect: () => {
        var $orderTypeSelect = $('#SelectedOrderType');
        var $deliveryInfoGroup = $('#deliveryInfoGroup');

        // Update order type visibility
        $orderTypeSelect.on("change", function () {
            var selectedOrderType = $orderTypeSelect.val();

            if (selectedOrderType === 'Delivery') {
                $deliveryInfoGroup.show();
            } else {
                $deliveryInfoGroup.hide();
            }
        });
    },
    initializeDeliveryAddressSelect: () => {
        // Get address ID from select
        // Use AJAX to return string array with delivery info
        // Fill delivery fields with returned data

        var $deliveryAddressSelect = $('#SelectedDeliveryAddressId')
        var $deliveryStreetAddress = $('#DeliveryStreetAddress');
    }
};