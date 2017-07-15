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

        $tr = $(this).closest('tr');
        $tr.addClass('active');
        $tr.find('.status').addClass('fa-refresh fa-spin active-task');
        

        $rows = $('table.table tr');               
        $rows.not('.active').hide(3000);        
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

    //task information
    $("#tasksDiv").on("click",".task-info",function (evt) {               

        var header = $(this).closest('td').find('.info-header').html();
        $('.modal-title').html(header);


        $('#taskDefinitionModalBody').empty();
        var markup = $(this).closest('td').find('.info-body').html();
        $('#taskDefinitionModalBody').html(markup);       
        

    });
    
    
});
