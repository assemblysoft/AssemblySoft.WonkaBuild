$(function () {

    //load tasks
    var $taskList = $('#taskList');

    $.get('/build/loadTasks', function (data) {
        $taskList.replaceWith(data);
    });

    //task selection - run task
    $("#tasksDiv").on("click", ".run-task", function (evt) {
        evt.preventDefault();
        evt.stopPropagation();

        $('.run-task').addClass('disabled');
        //$(this).closest('.run-task').addClass('disabled');
        $(this).closest('tr').find('.status').addClass('fa-refresh fa-spin active-task');
        
        var $progressDiv = $('#progressDiv');

        url = $(this).data('url');                
        
        
        $.get(url, function (data) {            
            $progressDiv.replaceWith(data);
        });
        
        setTimeout(CheckStatus, 1000);
    });


    $(".clearConsole").click(function (evt) {
        evt.preventDefault();
        evt.stopPropagation();

        $('#messages').empty();
    });
    
    
});