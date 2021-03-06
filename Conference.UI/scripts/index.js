$(function () {

    var content = $("#content");
    
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

    $.ajax({
        url: 'api/start/',
        type: 'GET',
        headers: {
            Authorization: 'Bearer ' + $.cookie("header")
        }
    })
    .done(function (data) {

        next.removeAttr('style');

        progressStart(data);

        setKey(data.Key);

        displayQuestion(data.Question);
    });

    var next = $('#next');
    next.click(function () {

        if (!isValid()) {
            $('.error').css('display', 'block');
            return;
        }

        var type = $('#type').val();
        var parameters = $('#content').serialize();
        $.ajax({
            url: 'api/answer/' + type,
            type: 'POST',
            data: parameters,
            headers: {
                Authorization: 'Bearer ' + $.cookie("header")
            }
        })
        .done(function (data) {

            progressNextStep(data);

            displayQuestion(data.Question);
        });
    });
});

function showLoader() {
    var loader = $('#loader');
    var content = $("#content");

    centerItem(loader.find('img'));
    content.css('opacity', '0.3');
    loader.css('height', $('.container-fluid').height());
    loader.show();
}

function hideLoader() {
    var loader = $('#loader');
    var content = $("#content");

    content.css('opacity', '1');
    loader.hide();

}

function displayQuestion(data) {
    if (data != null && !$.isEmptyObject(data)) {
        var factory = new QuestionTemplateFactory();
        var template = factory.Create(data);

        var content = $('#questionContent');
        var isFirstTemplate = content.html() == '';
        content.append(template);

        animate(content, template, isFirstTemplate);

        initStars(template);
    }
    else {

        displayThanks();       
    }

    $('#content').removeAttr('style');
}

function animate(content, template, isFirstTemplate) {

    if (isFirstTemplate) {
        template.attr('class', 'oldTemplate');
    }
    else {
        var width = template.outerWidth();

        template.attr('class', 'newTemplate');
        template.css('width', width + 'px');
        template.css('top', '10px');
        template.css('right', '-' + parseInt(width + 20) + 'px');

        content.velocity(
                        {
                            right: parseInt(width + 20) + "px"
                        },
                        {
                            duration: 1000,
                            complete: function () {
                                template.removeAttr('style');
                                template.removeClass('newTemplate');

                                content.removeAttr('style');
                                $('.oldTemplate').remove();

                                template.addClass('oldTemplate');

                                var container = $('.progressWrapper');
                                container.removeAttr('style');
                            }
                        });
    }
}

function progressStart(data) {

    var container = $('.progressWrapper');
    container.removeAttr('style');

    var progress = $('#progress');

    createProgress(data, progress);

    if (data.Answered >= 0) {
        fillProgress(data, progress);
    }
    else {
        container.css('visibility', 'hidden');
    }
}

function progressNextStep(data) {

    var progress = $('#progress');

    if (data.Quantity > progress.find('.step').length) {
        progress.empty();
        createProgress(data, progress);
    }

    fillProgress(data, progress);

    $('.progressWrapper').removeAttr('style');    
}

function createProgress(data, progress) {

    var width = 100 / data.Quantity;
    for (var i = 0; i < data.Quantity; i++) {

        var step = $('<div>');
        step.attr('class', 'step');
        step.css('width', width + '%');

        progress.append(step);
    }
}

function fillProgress(data, progress) {

    var steps = progress.find('.step');

    steps.find('.current').removeClass('current');

    $.each(steps, function (index, value) {
        var step = $(value);

        if (index < data.Answered) {
            step.addClass('answered');
        }
        if (index == data.Answered) {
            step.addClass('current');
        }
    });
}

function initStars(template) {
    var rating = template.find('.ratingContainer');
    rating.rating();
    $.each(rating.find('.stars'), function (index, value) {
        var rating = $(this);
        var width = rating.find('.star').length * 50;;
        rating.css('width', width + 'px');
    });
}

function displayThanks() {

    var logo = $('<div>');
    logo.attr('id', 'logo');
    logo.css('height', '200px');

    var link = $('<a>');
    link.attr('href', conferenceUrl);
    link.css('width', '300px');
    logo.append(link);

    var message = $('<p>');
    message.attr('class', 'thanksText');
    message.text('Спасибо, что уделили нам время!');

    $('#questionContent').remove();
    $('#next').remove();
    $('.progressWrapper').remove();
    $('.push').remove();

    var wrapper = $('.wrapper');
    wrapper.append(logo);
    wrapper.append(message);

    setKey('');
}

function centerItem(item) {
    var offset = ($(window).height() / 2) - 40;
    item.css('margin-top', window.pageYOffset + offset);
}

function setKey(key) {
    if ($.cookie("header") == undefined || $.cookie("header") != key) {
        $.cookie("header", key);
    }
}