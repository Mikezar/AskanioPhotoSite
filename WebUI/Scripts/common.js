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

function OpenPhotoInModal(context, url, includeTag)
{
    var id = $(context).data("id");

    $.ajax({
        type: 'GET',
        url: url,
        data: { id: id, includeTag: includeTag },
        cache: false,
        success: function (result) {
            $('#photo').html(result);
            $('#photo-modal').show();
        }
    });
}

function GetPhoto(id, url, isNext, includeTag) {
    $.ajax({
        type: 'POST',
        url: url,
        cache: false,
        data: { id: id, isNext: isNext, includeTag: includeTag },
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

function SortHandler(url)
{
    $('tr').on('click', function () {
        $('tr').removeClass('active-row');
        $(this).addClass('active-row');
    });

    $('.down').on('click', function () {
        var id = $(this).data('photo');
        var sid = $(this).closest('tr').next().find('.down').data('photo');
        var row = $(this).closest('tr');
        row.insertAfter(row.next());

        var data = {
            CurrentId: id,
            SwappedId: sid
        };

        $.ajax({
            type: 'POST',
            url: url,
            data: data
        });
    });

    $('.up').on('click', function () {
        var id = $(this).data('photo');
        var row = $(this).closest('tr');
        var sid = row.prev().find('.up').data('photo');
        row.prev().insertAfter(row);

        var data = {
            CurrentId: id,
            SwappedId: sid
        };

        $.ajax({
            type: 'POST',
            url: url,
            data: data
        });
    });
}

function ChangeOrder(data) {
    $.ajax({
        type: 'POST',
        url: '/Management/ChangeOrder',
        data: data
    });
}

function RegisterListener(event, func) {
    if (window.addEventListener) {
        window.addEventListener(event, func);
    } else {
        window.attachEvent('on' + event, func);
    }
}

function IsInViewport(element) {
    var rect = element.getBoundingClientRect();

    return (
        rect.bottom >= 0 &&
        rect.right >= 0 &&

        rect.top <= (
        window.innerHeight ||
        document.documentElement.clientHeight) &&

        rect.left <= (
        window.innerWidth ||
        document.documentElement.clientWidth)
     );
}

function LazyLoad(grid) {
    var lazy = $('.lazy');
    for (var i = 0; i < lazy.length; i++) {
        if (IsInViewport(lazy[i])) {
            lazy[i].src = lazy[i].getAttribute('data-src');
            lazy[i].onload = function (el) {
                $(this).fadeTo("slow", 1);
                $(this).parent().children('.loader').hide();
                //if (grid) {
                //    $('.grid').masonry('layout');
                //}
            };
        }
    }
    if (grid) {
           $('.grid').masonry('layout');
    }
}

