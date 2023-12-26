
    $(document).ready(function () {
        $('.table').DataTable();
        $("#multiSelectDD").chosen();
        });


$(function () {
    $("#loaderbody").addClass('hide');
    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass("hide");
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});

showInPopUp = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (response) {
            $("#form-modal .modal-title").html(title);
            $("#form-modal .modal-body").html(response);
            $("#form-modal").modal('show');
        }
    });
}
JQueryAjaxPost = form => {
    try
    {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FromData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $("#view-all").html(res.html);
                    $("#form-modal .modal-title").html('');
                    $("#form-modal .modal-body").html('');
                    $("#form-modal").modal('hide');
                    $.notify("Submited Successfully", "success");
                    $.notify('Submited Successfully', { globalPosition: 'top center', className: 'success' });

                } else {
                    $("#form-modal .modal-body").html(res.html);
                    $.notify('Something is Wrong', { globalPosition: 'top center', className: 'warn' });
                }
            },
            error: function (err) {
                console.log(err);
            }

        });
    } catch (e) {
        console.log(e);
    }
}


function searchByAppName() {

    var appName = $("#appName").val();

    $.ajax({
        url: "../User/AppDbChanges/", // the url of the controller action
        type: "POST", // the http method
        data: { appName: appName }, // the data to send
        success: function (data) {
            // update the table element with the partial view content
            $("#data-table").html(data);
        }
    });
    // for signof 
}

function showHideInfo(newid) {
    if (document.getElementById('appCheckbox').checked)
    {
        var id = newid

        $.ajax({
            url: "../User/SignIndicator/", // the url of the controller action
            type: "POST", // the http method
            data: { id: id }, // the data to send

        });
    } else {
  
    }
}






