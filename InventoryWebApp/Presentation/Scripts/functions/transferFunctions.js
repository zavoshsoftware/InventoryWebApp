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
    $('#qtyTransfer').val('');
    $('#weightTransfer').val('');
    $('#transfer-errorTransfer').css('display', 'none');

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
                $('#productTitleTransfer').html(data.ProductTitle);
                $('#orderCodeTransfer').html(data.OrderCode);
                $('#customerNameTransfer').html(data.CustomerFullName);
                $('#remainWeightTransfer').html(data.RemainWight);
                $('#remainQtyTransfer').html(data.RemainQuantity);
                $('#orderIdTransfer').val(data.ParentOrderId);
                $('#productIdTransfer').val(data.ProductId);
                //console.log(data);
            },
            error: function (reponse) {
                alert("خطایی رخ داده است. لطفا مجدادا تلاش کنید");
            }
        });
}

function openLoadingModal(productId, orderId, inputDetailId) {
    $('#hdnInputDetailId').val(inputDetailId);
    jQuery.noConflict();
    $('#qty').val('');
    $('#weight').val('');
    $('#transfer-error').css('display', 'none');

    $('#loadingModal').modal('show');

    $.ajax(
        {
            url: "/Orders/ShowLoadingData",
            data: {
                inputDetailId: inputDetailId,
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

function openCutModal(productId, orderId, inputDetailId) {
    $('#hdnInputDetailId').val(inputDetailId);
    jQuery.noConflict();
    $('#qty').val('');
    $('#weight').val('');
    $('#transfer-error').css('display', 'none');

    $('#cutModal').modal('show');

    $.ajax(
        {
            url: "/Orders/ShowLoadingData",
            data: {
                inputDetailId: inputDetailId,
            },
            cache: false,
            type: "POST",
            success: function (data) {
                $('#productTitle').html(data.ProductTitle);
                $('#orderCode').html(data.OrderCode);
                $('#customerName').html(data.CustomerFullName);
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
    var qty = $('#qtyTransfer').val();
    var weight = $('#weightTransfer').val();
    var customerId = $('#CustomerId').val();
    var productId = $('#productIdTransfer').val();
    var orderId = $('#orderIdTransfer').val();


    if (qty !== '' && weight !== '') {
        showErrorMessage('یکی از دو مقادیر تعداد و وزن باید تکمیل شوند.');
    } else {
        if (customerId === '' || customerId === '0') {

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
                    $('#transfer-successTransfer').css('display', 'block');
                    $('#transfer-errorTransfer').css('display', 'none');
                    window.location.reload();
                }
            },
            error: function (reponse) {
                alert("خطایی رخ داده است. لطفا مجدادا تلاش کنید");
            }
        });
}

function showErrorMessage(message) {
    $('#transfer-errorTransfer').css('display', 'block');
    $('#transfer-errorTransfer').html(message);
}


function Loading() {
    var qty = $('#qty').val();
    var weight = $('#weight').val();
    var exitDriverId = $('#ExitDriverId').val();
    var productId = $('#productId').val();
    var orderId = $('#orderId').val();
    var inputDetailId = $('#hdnInputDetailId').val();
    var carNumber = $('#carNumber').val();
    var desc = $('#desc').val();
    var phone = $('#driverPhone').val();
    var exitId = $('#ExitId').val();

    if (qty !== '' && weight !== '') {
        showErrorMessage('یکی از دو مقادیر تعداد و وزن باید تکمیل شوند.');
    }
    else if (exitDriverId === '') {
        showErrorMessage('یکی از دو مقادیر تعداد و وزن باید تکمیل شوند.');

    }
    else {
        LoadingAction(productId, orderId, qty, weight, inputDetailId, exitDriverId, carNumber, phone, desc, exitId);
    }
}

function LoadingAction(productId, orderId, qty, weight, inputDetailId, driverId, carNumber, phone, desc, exitId) {
    $.ajax(
        {
            url: "/Orders/PostLoading",
            data: {
                productId: productId,
                orderId: orderId,
                weight: weight,
                qty: qty,
                driverId: driverId,
                carNumber: carNumber,
                phone: phone,
                desc: desc,
                inputDetailId: inputDetailId,
                exitId: exitId
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
                    window.location.reload();
                }
                console.log(data);
            },
            error: function (reponse) {
                alert("خطایی رخ داده است. لطفا مجدادا تلاش کنید");
            }
        });
}


function PostCutOrder() {

    var cutTypeId = $('#CutTypeId').val();
    var inputDetailId = $('#hdnInputDetailId').val();
    if (inputDetailId != '' && cutTypeId != '') {
        CutOrder(inputDetailId, cutTypeId);
    }
    else {
        showErrorMessage('نوع برش را مشخص نمایید');
    }

}

function CutOrder(inputDetailId, cutTypeId) {
    $.ajax(
        {
            url: "/Cut/PostCutOrder",
            data: {
                cutTypeId: cutTypeId,
                inputDetailId: inputDetailId
            },
            cache: false,
            type: "POST",
            success: function (data) {
                if (data.includes('message')) {
                    showErrorMessage(data.split('-')[1]);
                }
                else if (data === 'false') {
                    showErrorMessage("خطایی رخ داده است. لطفا مجدادا تلاش کنید");
                }
                else {
                    window.location.href = "/cut/Details/" + data;
                }
                console.log(data);
            },
            error: function (reponse) {
                alert("خطایی رخ داده است. لطفا مجدادا تلاش کنید");
            }
        });
}

function ShowDriverModal() {
    $("#driverModal").modal('show');
}
function AddDriver() {
    var driverName = $("#txtDriverName").val();
    $.ajax({
        url: '/orders/CreateDriver',
        data: { driverName: driverName },
        type: 'POST',
        success: function (data) {
            $("#ExitDriverId").append('<option value="' + data.Value + '">' + data.Text + '</option>');
            $('#success-insertDriver').css('display', 'block');
            $('#error-insertDriver').css('display', 'none');
        },
        error: function (reponse) {
            $('#success-insertDriver').css('display', 'none');
            $('#error-insertDriver').css('display', 'block');
            $('#error-insertDriver').innerHtml = "خطایی رخ داده است. لطفا مجدادا تلاش کنید";
        }

    });
}
 