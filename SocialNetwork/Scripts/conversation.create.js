var recipient =
    '{ "Id": "' + $('#chatusers p:nth-child(2)').attr('id')
    + '", "Name": "' + $('#recipientName').text() + '" }';
var conversationParticipants = new Array(JSON.parse(recipient));
var conversationParticipantsIds = [$('#chatusers p:nth-child(2)').attr('id')];

function removeUser(userId, userName) {
    var index = conversationParticipantsIds.indexOf(userId);
    if (index != -1) {
        conversationParticipants.splice(index, 1);
        conversationParticipantsIds.splice(index, 1);
    }
    $('#' + userId).remove();
    $('#' + userId + 'img').attr('src', $('#' + userId + 'img').attr('src').replace('minus', 'plus'));
    $('#' + userId + 'img').attr('onclick', $('#' + userId + 'img').attr('onclick').replace('remove', 'add'));
}

function addUser(userId, userName) {
    conversationParticipants.push({Id: userId, Name: userName});
    conversationParticipantsIds.push(userId);
    $('#chatusers').append('<p id="' + userId + '"><b>' + userName
        + '</b><img src="/SocialNetwork/Content/Images/minus.png" onclick="removeUser('
        + '\'' + userId + '\'' + ', \'' + userName + '\')"/></p>');
    $('#' + userId + 'img').attr('src', $('#' + userId + 'img').attr('src').replace('plus', 'minus'));
    $('#' + userId + 'img').attr('onclick', $('#' + userId + 'img').attr('onclick').replace('add', 'remove'));
}

function addUsers() {
    jQuery.ajaxSettings.traditional = true;
    $.getJSON('GetNotIncludedFriends', { participantsIds: conversationParticipantsIds }, displayUsers);
}

var wereDisplayed = false;

function displayUsers(data) {
    if (!wereDisplayed) {
        for (var i = 0; i < conversationParticipants.length; i++) {
            $('#friendsTable').append('<tr><td><b>' + conversationParticipants[i].Name
                + '</b></td><td><img id="' + conversationParticipants[i].Id
                + 'img" src="/SocialNetwork/Content/Images/minus.png"  onclick="removeUser(\''
                + conversationParticipants[i].Id + '\', \''
                + conversationParticipants[i].Name + '\')"/></td></tr>');
        }
        if (data != null) {
            for (var j = 0; j < data.length; j++) {
                $('#friendsTable').append('<tr><td><b>' + data[j].Name
                + '</b></td><td><img id="' + data[j].Id
                + 'img" src="/SocialNetwork/Content/Images/plus.png"  onclick="addUser(\''
                + data[j].Id + '\', \''
                + data[j].Name + '\')"/></td></tr>');
            }
        }
        wereDisplayed = true;
    }
    document.getElementById('hiddenSpace').style.display = "block";
}

function send() {
    if (conversationParticipantsIds.length === 0) {
        alert("Выберите хотя бы одного получателя!");
        return false;
    }
    $('input[type="hidden"]').val(conversationParticipantsIds);
    return true;
}

function hide() {
    document.getElementById('hiddenSpace').style.display = "none";
}