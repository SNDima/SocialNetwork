Util = function (conversationId) {
    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.chatHub;

    // Recizing messagelist area
    $(window).load(function () { // On load
        var messagelistHeight = $(document).height() - ($('body').height() - $('#messagelist').height()) - 150;
        $('#messagelist').css({ 'height': messagelistHeight + 'px' });
        // Displays the last messages in the list
        $("#messagelist").animate({ scrollTop: screen.height * 1000 }, "slow");
    });
    $(window).resize(function () { // On resize
        var messagelistHeight = $(document).height() - ($('body').height() - $('#messagelist').height()) - 100;
        $('#messagelist').css({ 'height': messagelistHeight + 'px' });
        $('#messagelist').scrollTop(screen.height * 1000);
    });

    // Create a function that the hub can call back to display messages.
    chat.client.addMessage = function (convId, isRead, isShouldBeRead, senderName, content, time) {
        if (convId === conversationId) {
            // Add a message to the page.
            if (isRead) {
                $('#chatroom').append('<br/>' + '<li><strong>' + htmlEncode(senderName)
                + '</strong>: ' + htmlEncode(content) + '<br/>' + htmlEncode(time) + '</li>');
            } else {
                $('#chatroom').append('<br/>' + '<li style="color:DarkBlue"><strong>' + htmlEncode(senderName)
                + '</strong>: ' + htmlEncode(content) + '<br/>' + htmlEncode(time) + '</li>');
                if (isShouldBeRead) {
                    readMessages();
                }
            }
            $('#messagelist').scrollTop(screen.height * 1000);
        }
    };

    // Create a function that the hub can call back to display users to connect/disconnect them.
    chat.client.addUser = function(id, name, isConnected, isCurrentUser) {
        if (isCurrentUser) {
            addUser(id, name, 'danger', 'Выйти');
        } else if (isConnected) {
            addUser(id, name, 'danger', 'Удалить');
        } else {
            addUser(id, name, 'success', 'Добавить');
        }
    };

    chat.client.updateUser = function (convId, isConnected, id, conversationName) {
        if (convId === conversationId) {
            if (isConnected) {
                $('#' + id + ' .btn').removeClass('btn-success');
                $('#' + id + ' .btn').val('Удалить').addClass('btn-danger');
            } else {
                $('#' + id + ' .btn').removeClass('btn-danger');
                $('#' + id + ' .btn').val('Добавить').addClass('btn-success');
            }
            $('#conversationName').text(conversationName);
        }
    };

    chat.client.disconnect = function() {
        window.location.assign("https://localhost:44300/User");
    }

    // Connection opening
    $.connection.hub.start().done(function () {
        $('#sendmessage').click(function () {
            // Call the Send method on the hub.
            chat.server.send($('#message').val(), conversationId);
            $('#message').val('');
        });
        chat.server.connect(conversationId);
    });

    var addUser = function(id, name, buttonStyle, buttonValue) {
        $('#users').append('<p id="' + id + '"><b>' + name + '&nbsp;&nbsp;'
            + '</b>' + '<input type="button" class="btn btn-' + buttonStyle
            + '" style="width: 90px" value="' + buttonValue + '" />' + '</p>');
        $('#' + id + ' .btn').click(function () {
            chat.server.changeConversationParticipant(conversationId, id);
        });
    };

    // Reading messages
    var readMessages = function () {
        setTimeout(function () {
            chat.server.readMessages(conversationId);
            $("li").each(function() {
                $(this).css("color", "black");
            });
        }, 2000);
    };
    readMessages();
};

// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}