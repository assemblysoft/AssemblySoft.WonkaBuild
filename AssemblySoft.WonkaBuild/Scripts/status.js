$('.build-latest').on('click', function (evt) {
    evt.preventDefault();
    evt.stopPropagation();

    $('.build-latest').addClass('disabled');

    var $progressDiv = $('#progressDiv'),
        url = $(this).data('url');

    $.get(url, function (data) {
        $progressDiv.replaceWith(data);
    });

    setTimeout(CheckStatus, 2000);
});


function notifyUserOfConnectionProblem() {
    //alert('experiencing some problems with connections');
}

function notifyUserOfTryingToReconnect() {
    //alert('reconnect issues');
}

$(function () {

    $.connection.hub.connectionSlow(function () {
        notifyUserOfConnectionProblem(); 
    });

    $.connection.hub.reconnecting(function () {
        notifyUserOfTryingToReconnect(); 
    });

    // Reference the auto-generated proxy for the hub.
    var messageStatusHub = $.connection.messageStatusHub;

    //Callback
    messageStatusHub.client.addModel = function (model) {

        // Add the message to the page.
        $('#messages').append('<li class="line"><strong>' + htmlEncode(model.message) + '</strong>: ' + '</li>');
        $('.console').animate({ scrollTop: $('.console').prop("scrollHeight") }, 10);
    };   

    
    // Start the connection.
    $.connection.hub.start().done(function () {      

    });

    //ToDo: disconnect when complete

});

// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}