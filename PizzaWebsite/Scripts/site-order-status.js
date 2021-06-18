// Namespace for Order Status view scripts
const pizzaSiteOrderStatusNs = {
    updateOrderStatusText: (orderId) => {
        var $orderStatusText = $('#orderStatusText');
        var params = { orderId: orderId };
        pizzaSiteNs.ajaxCall("/Shop/GetOrderStatusAjax", JSON.stringify(params), "POST").fail(function (response) {
                console.log(response);
                $orderStatusText.html('Unable to get order status.');
            })
            .done(function (response) {
                $orderStatusText.html(response);
            });
    }
};