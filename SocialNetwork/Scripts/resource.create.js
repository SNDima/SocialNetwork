function addURL(number) {
    $('#urls').append('<div id="url' + number + '" class="input-group">'
        + '<span class="input-group-btn">'
        + '<button class="btn btn-default" type="button" '
        + 'onclick="removeURL(\'url' + number + '\')">'
        + '<i class="glyphicon glyphicon-minus"></i></button></span>'
        + '<input class="form-control" style="max-width: 240px" name="URLs" '
        + 'type="url"><span class="field-validation-valid text-danger" '
        + 'data-valmsg-for="URLs" data-valmsg-replace="true"></span></div>');

    $('#addButton').attr('onclick', 'addURL(' + ++number + ')');
}

function removeURL(id) {
    $('#' + id).remove();
}