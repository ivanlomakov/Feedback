$(function () {

    var content = $("#content");

    content.submit(function (e) { e.preventDefault(); });

    content.validate({
        submitHandler: function (form) {

            var name = $('#name');
            var nameField = $('#nameField');
            var number = $('#number');

            number.removeClass('has-error');
            name.removeClass('has-error');

            name.val($.trim(name.val()));

            if (name.val().trim().length == 0) {
                name.val('');
                nameField.addClass('has-error');
                return;
            }

            if (!/^[a-zA-Zа-яА-Я ]+$/.test(name.val())) {
                nameField.addClass('has-error');
                return;
            }

            if (!validateUid($('#uid').val())) {
                number.addClass('has-error');
                $('#uid').focus();
                return;
            }
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) { }
    });
});

function isValid() {
    var isValid = true;

    var error = $('.error');

    var type = $('#type').val();
    switch (type) {
        case 'SingleOptionQuestion':

            error.text('Выберите один из пунктов');

            var selectedItem = $('#questionContent').find('input[type=radio]:checked');
            isValid = selectedItem.length != 0;
            if (isValid) {
                isValid = isQuestionOptionValid(selectedItem, error);
            }

            break;

        case 'RatingQuestion':

            error.text('Сделайте Ваш выбор');

            var selectedItem = $('#questionContent').find('input[type=radio]:checked');
            isValid = selectedItem.length == $('.ratingContainer').length;

            break;

        case 'TextQuestion':

            error.text('Добавьте текст');

            var text = $('#text').val();
            var pattern = new RegExp('^ +$');
            
            isValid = text != '' && !pattern.test(text);

            break;

        case 'AcquaintanceQuestion':

            var form = $('#content');
            form.submit();

            isValid = $('#content').find('.has-error').length == 0;

            break
    }

    return isValid;
}

function isQuestionOptionValid(selectedItem, error) {

    var isValid = true;

    var optionType = $('#optionType').val();
    switch (optionType) {
        case 'InputQuestionOption':

            error.text('Введите Ваш вариант');

            var text = selectedItem.parents('.radioButtonContainer').find('input[type=text]').val();
            if (text == '') {
                isValid = false;
            }
            break;
    }

    return isValid;
}

function validateUid(uid) {

    var result = false;

    var regexp = /^[0-6]\d{2}$/;
    var check = regexp.exec(uid);
    if (check == null) {
        return false;
    }

    var number = parseInt(uid);
    if (uid.length == 3 && number != NaN && number > 0 && number <= 600) {
        result = true;
    }

    return result;
}