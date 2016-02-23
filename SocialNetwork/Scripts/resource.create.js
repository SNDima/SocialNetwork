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

$('.switch-input').change(function () {
    if ($(this).is(':checked')) {
        document.getElementById('upload').style.display = "block";
    } else {
        document.getElementById('upload').style.display = "none";
    }
});

$(function () {
    'use strict';

    // Name of a web application (usually in full IIS mode). Can be found in Properties/Web/Server/Project-Url. Example: http://localhost/Demo (Name of web application is "Demo")
    var web_app = 'http://localhost/SocialNetwork/';

    // We use the upload handler integrated into Backload:
    // In this example we set an objectContect (id) in the url query (or as form parameter). You can use a user id as 
    // objectContext give users only access to their own uploads. ObjectContext can also be set server side (See Custom Data Provider Demo, 2.2+).
    var url = web_app + 'Backload/FileHandler?objectContext=C5F260DD3787';


    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({
        url: url,
        maxChunkSize: 50000000                                           // Optional: file chunking with 50MB chunks
        //acceptFileTypes: /(jpg)|(jpeg)|(png)|(gif)|(pdf)$/i                 // Allowed file types
    });

    // Optional: Load existing files:
    $('#fileupload').addClass('fileupload-processing');
    $.ajax({
        // Uncomment the following to send cross-domain cookies:
        // xhrFields: {withCredentials: true},
        url: url,
        dataType: 'json',
        context: $('#fileupload')[0]
    }).always(function () {
        $(this).removeClass('fileupload-processing');
    }).done(function (result) {
        $(this).fileupload('option', 'done')
            .call(this, $.Event('done'), { result: result });
    });
});
