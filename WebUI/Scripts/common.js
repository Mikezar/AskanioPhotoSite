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

function OpenPhotoInModal(context, url)
{
    var id = $(context).data("id");
    $.ajax({
        type: 'GET',
        url: url,
        data: { id: id },
        success: function (result) {
            $('#photo').html(result);
            $('#photo-modal').show();
            $('body').css("overflow", "hidden");
        }
    });
}

function GetPhoto(id, url, isNext) {
    $.ajax({
        type: 'POST',
        url: url,
        data: { id: id, isNext: isNext },
        success: function (result) {
            $('#photo').html(result);
            $('#photo-modal').show();
        }
    });
}