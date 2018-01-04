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
        cache: false,
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
        cache: false,
        data: { id: id, isNext: isNext },
        success: function (result) {
            $('#photo').html(result);
            $('#photo-modal').show();
        }
    });
}

function WatermarkInit()
{
    $('.IsWatermarkBlack').attr("disabled", "true");
    $('.IsSignatureBlack').attr("disabled", "true");
    $('.IsWebSiteTitleBlack').attr("disabled", "true");
    $('input[type="checkbox"]').attr('checked', false);
}

function WatermarkChangeHandler() {
    $('#isWaterMarked').change(function () {
        if ($(this).is(':checked'))
            $('.IsWatermarkBlack').removeAttr('disabled');
        else $('.IsWatermarkBlack').attr('disabled', 'true');
    });

    $('#IsSignatureApplied').change(function () {
        if ($(this).is(':checked'))
            $('.IsSignatureBlack').removeAttr('disabled');
        else $('.IsSignatureBlack').attr('disabled', 'true');
    });
    $('#IsWebSiteTitleApplied').change(function () {
        if ($(this).is(':checked'))
            $('.IsWebSiteTitleBlack').removeAttr('disabled');
        else $('.IsWebSiteTitleBlack').attr('disabled', 'true');
    });
}