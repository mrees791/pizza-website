// Namespace for Order Status view scripts
const pizzaSiteOrderStatusNs = {
    updateOrderStatus: (orderId) => {
        $orderStatus = $("#orderStatus");

        var params = { orderId: orderId };
        pizzaSiteNs.ajaxCall("/Shop/GetOrderStatusAjax", JSON.stringify(params), "POST").fail(function(response) {
                console.log(response);
            })
            .done(function(response) {
                $orderStatus.html(response);
            });
    }
};