function changeConversationName(conversationId) {
    var name = prompt('Введите новое название диалога', '');
    $.ajax({
        type: "POST",
        url: "User/ChangeConversationName",
        data: { conversationId : conversationId, name : name}
    });
    $('#' + conversationId).text(name);
}