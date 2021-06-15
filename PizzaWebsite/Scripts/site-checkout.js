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
    fillDeliveryFeilds: (name, addressType, streetAddress, city, state, zipCode, phoneNumber) => {
        var $deliveryAddressName = $('#DeliveryAddressName');
        var $deliveryAddressType = $('#SelectedDeliveryAddressType');
        var $deliveryStreetAddress = $('#DeliveryStreetAddress');
        var $deliveryCity = $('#DeliveryCity');
        var $deliveryState = $('#SelectedDeliveryState');
        var $deliveryZipCode = $('#DeliveryZipCode');
        var $deliveryPhoneNumber = $('#DeliveryPhoneNumber');

        $deliveryAddressName.val(name);
        $deliveryAddressType.val(addressType);
        $deliveryStreetAddress.val(streetAddress);
        $deliveryCity.val(city);
        $deliveryState.val(state);
        $deliveryZipCode.val(zipCode);
        $deliveryPhoneNumber.val(phoneNumber);
    },
    resetDeliveryFields: () => {
        pizzaSiteCheckoutNs.fillDeliveryFeilds('', '', '', '', '', '', '');
    },
    initializeDeliveryAddressSelect: () => {
        var $deliveryAddressSelect = $('#SelectedDeliveryAddressId')
        var $deliveryFieldsGroup = $('#deliveryFieldsGroup');

        $deliveryAddressSelect.on("change", function () {
            var addressId = $deliveryAddressSelect.val();
            addressIsSelected = addressId != 0;
            if (addressIsSelected) {
                var params = { addressId: addressId };
                pizzaSiteNs.ajaxCall('/ManageDeliveryAddresses/GetDeliveryAddressAjax', JSON.stringify(params), 'POST').
                    fail(function (response) {
                        console.log(response);
                    }).
                    done(function (response) {
                        var name = response[0];
                        var addressType = response[1];
                        var streetAddress = response[2];
                        var city = response[3];
                        var state = response[4];
                        var zipCode = response[5];
                        var phoneNumber = response[6];

                        pizzaSiteCheckoutNs.fillDeliveryFeilds(name, addressType, streetAddress, city, state, zipCode, phoneNumber);
                        $deliveryFieldsGroup.hide();
                    });
            } else {
                pizzaSiteCheckoutNs.resetDeliveryFields();
                $deliveryFieldsGroup.show();
            }
        });
    },
    initializeCheckoutView: () => {
        pizzaSiteCheckoutNs.initializeOrderTypeSelect();
        pizzaSiteCheckoutNs.initializeDeliveryAddressSelect();
    }
};