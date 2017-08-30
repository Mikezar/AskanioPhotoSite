function ConfirmDelete(message, callback) {
    if (!callback) {
        callback = function () { };
    }
    bootbox.setDefaults("locale", "ru");
    bootbox.dialog({
        message: message,
        title: "Подтвердите удаление",
        buttons: {
            yes: {
                label: "Удалить",
                className: "btn-danger",
                callback: function () {
                    callback();
                }
            },
            no: {
                label: "Отмена",
                className: "btn-default"
            }
        }
    }
    );
}

function DeleteEntity(id, url, element)
{
    $.ajax({
        type: 'POST',
        url: url,
        data: { id: id },
        success: function (result) {
            if (result.success) {
                $(element).closest('tr').fadeIn(500,
                    function () {
                        $(this).remove();
                    });
            } else {
                bootbox.alert(result.errorMessage);
            }
        }
    });
}