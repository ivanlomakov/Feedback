function QuestionTemplateFactory() {

    this.Create = function (data) {
        
        var content = $('<div>');

        var type = $('<input>');
        type.attr('type', 'hidden');
        type.attr('name', 'type');
        type.attr('id', 'type');
        type.val(data.Type);

        content.append(type);

        var error = $('<p>');
        error.attr('class', 'error');

        switch(data.Type)
        {
            case 'SingleOptionQuestion':

                var optionType = $('<input>');
                optionType.attr('type', 'hidden');
                optionType.attr('name', 'optionType');
                optionType.attr('id', 'optionType');
                content.append(optionType);
                
                var title = $('<p>');
                title.text(data.Text);

                content.append(title);

                var factory = new QuestionOptionTemplateFactory();
                $.each(data.Options, function (index, value) {

                    var template = factory.Create(value);
                    content.append(template);
                });

                break;

            case 'TextQuestion':

                var title = $('<p>');
                title.text(data.Text);

                var answer = $('<textarea>');
                answer.attr('type', 'text');
                answer.attr('class', 'textQuestionInput');
                answer.attr('name', 'text');
                answer.attr('id', 'text');
                answer.bind('input propertychange', function () {
                    $('.error').removeAttr('style');
                });

                content.append(title);
                content.append(answer);

                break;

            case 'RatingQuestion':

                var title = $('<p>');
                title.text(data.Text);

                content.append(title);

                $.each(data.Options, function (index, value) {
                    generateRatingQuestionOption(index, value, content);
                })

                break;

            case 'AcquaintanceQuestion':

                var logo = $('<div>');
                logo.attr('id', 'logo');

                var link = $('<a>');
                link.attr('href', conferenceUrl);
                logo.append(link);

                var title = $('<p>');
                title.text(data.Text);

                var name = generateTextField('nameField', '&nbsp;&nbsp;Имя&nbsp;&nbsp;', 'name', 'Как мы можем к тебе обращаться?');
                var uid = generateTextField('number', 'Номер', 'uid', 'Номер твоего бейджа');

                content.append(logo);
                content.append(title);
                content.append(name);
                content.append(uid);

                break;
        }

        content.append(error);

        return content;
    };
};

function generateRatingQuestionOption(index, data, content)
{
    var container = $('<div>');
    container.attr('class', 'ratingOptionContainer');

    var id = $('<input>');
    id.attr('type', 'hidden');
    id.attr('name', '[' + index + '].Id');
    id.val(data.Id);
    container.append(id);

    var optionsContainer = $('<div>');
    optionsContainer.attr('class', 'ratingContainer');

    var description = $('<p>');
    description.text(data.Description);
    description.attr('class', 'description');

    for (var i = data.From; i <= data.To; i += data.Step) {

        var ratingOption = $('<input>');
        ratingOption.attr('type', 'radio');
        ratingOption.attr('name', '[' + index + '].Rating');
        ratingOption.attr('class', 'rating');
        ratingOption.val(i);
        ratingOption.change(function () {
            $('.error').removeAttr('style');
        });

        optionsContainer.append(ratingOption);
    }

    container.append(description);
    container.append(optionsContainer);

    content.append(container);
}

function generateTextField(id, title, inputId, inputText)
{
    var container = $('<div>');
    container.attr('class', 'form-group');
    container.attr('id', id);

    var group = $('<div>');
    group.attr('class', 'input-group');
    container.append(group);

    var text = $('<span>');
    text.attr('class', 'input-group-addon');
    text.html(title);
    group.append(text);

    var input = $('<input>');
    input.attr('required', 'required');
    input.attr('id', inputId);
    input.attr('name', inputId);
    input.attr('type', 'text');
    input.attr('class', 'form-control');
    input.attr('placeholder', inputText);
    group.append(input);

    return container;
}

function QuestionOptionTemplateFactory() {

    this.Create = function (data) {

        var content = $('<div>');
        content.attr('class', 'radioButtonContainer');

        switch (data.Type) {
            case 'QuestionOption':
            case 'RedirectionQuestionOption':

                var radioButton = createRadioButton(data);
                content.append(radioButton);

                break;

            case 'InputQuestionOption':

                var radioButton = createRadioButton(data);
                radioButton.attr('class', 'inputQuestionOption');

                var answer = $('<input>');
                answer.attr('type', 'text');
                answer.attr('id', 'text');
                answer.attr('name', 'text');
                answer.attr('disabled', 'disabled');
                answer.attr('class', 'inputQuestionOptionInput');
                answer.data('optionId', radioButton.find('input[type=radio]').attr('id'));
                answer.keydown(function (event) {
                    $('.error').removeAttr('style');
                    if (event.keyCode == 13) {
                        $('#next').focus()
                        return false;
                    }
                });

                content.append(radioButton);
                content.append(answer);

                $(document).on('change', 'input[name=optionId]', function () {
                    var optionId = answer.data('optionId');
                    var currentOptionId = $(this).attr('id');
                    if (optionId == currentOptionId) {
                        answer.removeAttr('disabled');
                    }
                    else {
                        answer.attr('disabled', 'disabled');
                    }
                });

                break;
        }

        return content;
    };

    function createRadioButton(data) {

        var container = $('<div>');
        container.attr('class', 'radioButton');
        container.click(function () {
            $('.innerCircle').removeAttr('style');
            $(this).find('.innerCircle').css('display', 'block');

            var input = $(this).find('input[type=radio]');
            var type = input.data('type');
            input.prop("checked", true).trigger('change');

            $('#optionType').val(type);

            $('.error').removeAttr('style');
        });

        var innerCircle = $('<div>');
        innerCircle.attr('class', 'innerCircle');

        var circle = $('<div>');
        circle.attr('class', 'circle');
        circle.append(innerCircle);

        var radioButton = $('<input>');
        radioButton.attr('type', 'radio');
        radioButton.attr('name', 'optionId');
        radioButton.attr('id', 'id' + data.Id);
        radioButton.attr('value', data.Id);
        radioButton.data('type', data.Type);

        var label = $('<label>');
        label.text(data.Text);
        label.attr('for', 'id' + data.Id);

        container.append(circle);
        container.append(radioButton);
        container.append(label);

        return container;
    }
};