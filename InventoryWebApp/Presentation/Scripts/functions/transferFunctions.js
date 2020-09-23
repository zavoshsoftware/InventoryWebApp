function openModal(productId, orderId, orderCode, productTitle) {
    jQuery.noConflict();

    $('#detailModal').modal('show');
    $('#detailModalLabel').html('جزییات ورود ' + productTitle + ' از حواله شماره ' + orderCode);

    $.ajax(
        {
            url: "/InputDetails/Details",
            data: {
                productId: productId,
                orderId: orderId
            },
            cache: false,
            type: "POST",
            success: function (data) {
                var item = "";
                for (var i = 0; i < data.length; i++) {
                    item = item +
                        loadInputInfo(
                            data[i].InputCode,
                            data[i].InputDate,
                            data[i].Quantity,
                            data[i].SourceWeight,
                            data[i].DestinationWeight);
                }

                $('#factor tbody').html(item);

            },
            error: function (reponse) {
                alert("خطایی رخ داده است. لطفا مجدادا تلاش کنید");

            }
        });

}

function openTransferModal(productId, orderId) {
    jQuery.noConflict();

    $('#transferModal').modal('show');

    $.ajax(
        {
            url: "/Orders/ShowTransferData",
            data: {
                productId: productId,
                orderId: orderId
            },
            cache: false,
            type: "POST",
            success: function (data) {
                $('#productTitle').html(data.ProductTitle);
                $('#orderCode').html(data.OrderCode);
                $('#customerName').html(data.CustomerFullName);
                $('#remainWeight').html(data.RemainWight);
                $('#remainQty').html(data.RemainQuantity);
                console.log(data);
            },
            error: function (reponse) {
                alert("خطایی رخ داده است. لطفا مجدادا تلاش کنید");
            }
        });
}


function loadInputInfo(inputCode, inputDate, quantity, sourceWeight, destinationWeight) {
    return "<tr><td>" +
        inputCode +
        "</td><td>" +
        inputDate +
        "</td><td>" +
        quantity +
        "</td><td>" +
        sourceWeight +
        "</td><td>" +
        destinationWeight +
        "</td></tr>";
}
