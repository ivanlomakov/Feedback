
$(function () {

    composeLoader();

    var winner = $("#winner");

    var loader = $('#loader');
    loader.hide();

    var timer;
    jQuery.ajaxSetup({
        beforeSend: function () {
            timer = setTimeout(showLoader, 1200);            
        },
        complete: function () {
            clearTimeout(timer);
            hideLoader();            
        }
    });

    winner.validate({
        submitHandler: function (form) {

            $.ajax({
                type: "GET",
                url: '/api/winner?password=' + $('#password').val()
            })
            .done(function (data) {
                $('#uid').val(data.Uid);
                $('#name').val(data.Name);
            });
        },
        errorPlacement: function (error, element) { }
    });

    $('#login').click(function () {
        $.ajax({
            type: "POST",
            url: "api/checkpassword",
            data: JSON.stringify($('#password').val()),
            contentType: "application/json",
            success: function (data) {
                $('#selection').removeAttr('style');
                $('#credentials').css('display', 'none');

                setKey($('#password').val());
            },
            error: function () {
                alert('Неверный пароль');
            }
        });
    });

});

function composeLoader() {
    var logo = $('.logo');
    logo.find('a').attr('href', conferenceUrl);
    logo.find('img').attr('src', 'styles/images/logos/' + conferenceName + '-logo.png');
}

function showLoader() {
    var winner = $("#winner");
    var loader = $('#loader');

    winner.css('opacity', '0.3');
    loader.css('height', $('.container-fluid').height());
    loader.show();
}

function hideLoader() {
    var winner = $("#winner");
    var loader = $('#loader');

    winner.css('opacity', '1');
    loader.hide();
}

function setKey(key) {
    if ($.cookie("password") === undefined || $.cookie("password") !== key) {
        $.cookie("password", key);
    }
}