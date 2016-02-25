Details = function (filesNames) {
    'use strict';

    var names = filesNames.split('|');

    var web_app = 'http://localhost/SocialNetwork/';
    var url = web_app + 'Backload/FileHandler';

    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({
        url: url
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