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

        var $progressDiv = $('#progressDiv');

        url = $(this).data('url');
        $.get(url, function (data) {            
            $progressDiv.replaceWith(data);
        });

        setTimeout(CheckStatus, 1000);
    });
    
});