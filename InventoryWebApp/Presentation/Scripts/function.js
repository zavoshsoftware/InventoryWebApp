function clearForm() {
    $('#Detail_Code').val('');
    $('#Detail_Quantity').val('');
    $('#Detail_DestinationWeight').val('');
    $('#Detail_SourceWeight').val('');
    $('#ProductId').val('');
}

function backToList() {
    window.location.href = "/Inputs";
}