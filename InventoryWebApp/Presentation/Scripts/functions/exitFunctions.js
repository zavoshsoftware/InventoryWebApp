function updateWeight(exitDetailId) {
    var fullWeight = $('#txtfull_' + exitDetailId).val();
    var emptyWeight = $('#txtempty_' + exitDetailId).val();

    $.ajax(
        {
            url: "/ExitDetails/Edit",
            data: {
                exitDetailId: exitDetailId,
                fullWeight: fullWeight,
                emptyWeight: emptyWeight
            },
            cache: false,
            type: "POST",
            success: function (data) {
                if (data === 'false') {
                    $('#exitDetailDanger').css('display', 'block');
                    $('#exitDetailDanger').html("خطایی رخ داده است. لطفا مجدادا تلاش کنید");
                }
                else {
                    $('#exitDetailDanger').css('display', 'none');
                    $('#exitDetailSuccess').css('display', 'block');

                }
            },
            error: function (reponse) {
                $('#exitDetailDanger').css('display', 'block');
                $('#exitDetailDanger').html("خطایی رخ داده است. لطفا مجدادا تلاش کنید");
            }
        });
}



function openAccountModal(exitId) {

    jQuery.noConflict();

    $('#accountModal').modal('show');

    $.ajax(
        {
            url: "/ExitDetails/CalculateAccounts",
            data: {
                exitId: exitId,
            },
            cache: false,
            type: "POST",
            success: function (data) {
                $('#txtInventory').val(data.InventoryAmount);
                $('#txtLoad').val(data.LoadAmount);
                $('#txtWeightBridge').val(data.WeighbridgeAmount);
                $('#txtOther').val(data.OtherAmount);
                $('#txtCut').val(data.CutAmount);
                $('#txtVat').val(data.Vat);
                $('#txtTotal').val(data.TotalAmount);

                $('#txtInventoryIn').val(data.InventoryAmount);
                $('#txtLoadIn').val(data.LoadAmount);
                $('#txtWeightBridgeIn').val(data.WeighbridgeAmount);
                $('#txtOtherIn').val(data.OtherAmount);
                $('#txtCutIn').val(data.CutAmount);
                $('#txtVatIn').val(data.Vat);
                $('#txtTotalIn').val(data.TotalAmount);


            },
            error: function (reponse) {
                alert("خطایی رخ داده است. لطفا مجدادا تلاش کنید");
            }
        });

}


function finalizeExit(exitId) {
    var inventoryAmount = $('#txtInventory').val();
    var loadAmount = $('#txtLoad').val();
    var weighbridgeAmount = $('#txtWeightBridge').val();
    var otherAmount = $('#txtOther').val();
    var cutAmount = $('#txtCut').val();
    var vatAmount = $('#txtVat').val();

    var receivedInventoryAmount = $('#txtInventoryIn').val();
    var receivedCutAmount = $('#txtCutIn').val();
    var receivedLoadAmount = $('#txtLoadIn').val();
    var receivedVatAmount = $('#txtVatIn').val();

    var inventoryCustomer = $('#InventoryCustomer').val();
    var cutCustomer = $('#CutCustomer').val();
    var loadCustomer = $('#LoadCustomer').val();
    var vatCustomer = $('#VatCustomer').val();

    var customer = $('#Exit_CustomerId').val();




    $.ajax(
        {
            url: "/ExitDetails/FinalizeExit",
            data: {
                exitId: exitId,
                cutAmount: cutAmount,
                otherAmount: otherAmount,
                weighbridgeAmount: weighbridgeAmount,
                loadAmount: loadAmount,
                inventoryAmount: inventoryAmount,
                vatAmount: vatAmount,

                receivedInventoryAmount: receivedInventoryAmount,
                receivedCutAmount: receivedCutAmount,
                receivedLoadAmount: receivedLoadAmount,
                receivedVatAmount: receivedVatAmount,

                customer: customer,
                inventoryCustomer: inventoryCustomer,
                cutCustomer: cutCustomer,
                loadCustomer: loadCustomer,
                vatCustomer: vatCustomer

            },
            cache: false,
            type: "POST",
            success: function (data) {
                if (data === 'false') {
                    $('#dangerFinalizeExit').css('display', 'block');
                    $('#dangerFinalizeExit').html("خطایی رخ داده است. لطفا مجدادا تلاش کنید");
                }
                else {
                    $('#dangerFinalizeExit').css('display', 'none');
                    $('#successFinalizeExit').css('display', 'block');


                    $("#btnSave").prop("disabled", true);
                    $("#btnAccount").prop("disabled", false);
                    $("#btnAccountPaper").prop("disabled", false);

                }
            },
            error: function (reponse) {
                alert("خطایی رخ داده است. لطفا مجدادا تلاش کنید");
            }
        });

}

function ReceivedAmountChange(id)
{
    $("#" + id + "Customer").attr('disabled', false);
}

function redirectTo(url) {
    window.location.href = url;
}