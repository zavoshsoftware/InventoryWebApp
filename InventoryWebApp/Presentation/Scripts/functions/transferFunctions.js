﻿function openModal(productId, orderId, orderCode, productTitle) {
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
    $('#qty').val('');
    $('#weight').val('');
    $('#transfer-error').css('display', 'none');

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
                $('#orderId').val(data.ParentOrderId);
                $('#productId').val(data.ProductId);
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


function postTransfer() {
    var qty = $('#qty').val();
    var weight = $('#weight').val();
    var customerId = $('#CustomerId').val();
    var productId = $('#productId').val();
    var orderId = $('#orderId').val();


    if (qty !== '' && weight !== '') {
        showErrorMessage('یکی از دو مقادیر تعداد و وزن باید تکمیل شوند.');
    } else {
        if (customerId === '') {

            showErrorMessage('خریدار را انتخاب نمایید.');
        } else {
            postTransferAction(productId, orderId, qty, weight, customerId);
        }
    }
}

function postTransferAction(productId, orderId, qty, weight, customerId) {
    $.ajax(
        {
            url: "/Orders/PostTransfer",
            data: {
                productId: productId,
                orderId: orderId,
                weight: weight,
                qty: qty,
                customerId: customerId
            },
            cache: false,
            type: "POST",
            success: function (data) {
                if (data.includes('message')) {
                    showErrorMessage(data.split('-')[1]);
                }
                else if (data === 'true') {
                    $('#transfer-success').css('display', 'block');
                    $('#transfer-error').css('display', 'none');
                }
                console.log(data);
            },
            error: function (reponse) {
                alert("خطایی رخ داده است. لطفا مجدادا تلاش کنید");
            }
        });
}

function showErrorMessage(message) {
    $('#transfer-error').css('display', 'block');
    $('#transfer-error').html(message);
}