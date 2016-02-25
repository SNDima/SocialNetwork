Edit = function(filesNames) {
    'use strict';

    var names = filesNames.replace('&#39;', '\'').split('|');

    var web_app = 'http://localhost/SocialNetwork/';
    var url = web_app + 'Backload/FileHandler';


    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({
        url: url,
        maxChunkSize: 50000000
    });

    $('#fileupload').bind('fileuploaddone', function (e, data) {
        $.ajax(
            {
                type: "POST",
                url: web_app + "/Resources/AddFile",
                data: {
                    fileName: data.result.files[0].name
                }
            });
    });

    $('#fileupload').bind('fileuploaddestroy', function (e, data) {
        $.ajax(
            {
                type: "POST",
                url: web_app + "/Resources/DeleteFile",
                data: {
                    fileName: data.context.find('a[download]').attr('download')
                }
            });
    });

    // Load existing files:
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
        var excludedFiles = new Array();
        $.each(result.files, function (index, value) {
            var isNecessary = false;
            $.each(names, function (i, v) {
                if (v === value.name) {
                    isNecessary = true;
                }
            });
            if (!isNecessary) {
                excludedFiles.push(index);
            }
        });
        var removedFilesNumber = 0;
        $.each(excludedFiles, function (index, value) {
            result.files.splice(value - removedFilesNumber, 1);
            removedFilesNumber++;
        });
        $(this).fileupload('option', 'done')
            .call(this, $.Event('done'), { result: result });
    });
};

function editResource() {
    if ($('.switch-input').prop('checked')) {
        $("#fileupload td p.name a").each(function () {
            $('#editresource').append('<input type="hidden" name="FilesNames" '
                + ' value="' + $(this).attr("download") + '" />');
        });
    }
    return true;
}

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