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

function IsInViewport(el) {
    var rect = el.getBoundingClientRect();
    var html = document.documentElement;
    return (
        rect.top >= 0 &&
        rect.left >= 0 &&
        rect.bottom <= (window.outerHeight + 270 || html.clientHeight) &&
        rect.right <= (window.outerWidth + 270|| html.clientWidth)
    );
}

    var counter = 0;
    var loadCounter = 0;

    function LazyLoad(grid) {
        var lazy = $('.loader');
        var photos = $('.lazy');

        for (var i = counter; i < photos.length; i++) {
            if (IsInViewport(lazy[i])) {
                photos[i].src = photos[i].getAttribute('data-src');
                loadCounter++;
                photos[i].onload = function () {
                    $(this).fadeTo("slow", 1);
                    $(this).parent().children('.loader').hide();
                    loadCounter--;

                    if (grid && loadCounter == 0) {
                        setTimeout(BeautifyLayout, 100);
                    }
                };
                counter++;
            }
        }
    }

    function BeautifyLayout() {
        $('.grid').masonry('layout');
    }


