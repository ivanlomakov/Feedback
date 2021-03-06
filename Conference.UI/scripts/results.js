$(function () {

    $('#login').click(function () {

        $.ajax({
            type: "POST",
            url: "api/results",
            data: JSON.stringify($('#password').val()),
            contentType: "application/json",
            success: function (data) {
                displayGrid(data);
            },
            error: function () {
                alert('Неверный пароль');
            }
        });
    });    
});

function displayGrid(data) {
    $('#container').remove();

    var source =
    {
        localdata: data,
        datatype: "array"
    };

    var dataAdapter = new $.jqx.dataAdapter(source, {
        loadComplete: function (data) { },
        loadError: function (xhr, status, error) { }
    });

    $("#results").jqxGrid(
    {
        width: 1840,
        autorowheight: true,
        autoheight: true,
        pageable: true,
        source: dataAdapter,
        columns: [
            { text: 'Uid', datafield: 'Uid', width: 40 },
            { text: 'Имя', datafield: 'Q23', width: 100 },
            { text: 'Давай познакомимся', datafield: 'Q1', width: 150 },
            { text: 'Виделись мы раньше?', datafield: 'Q2', width: 160 },
            { text: 'Что привело тебя?', datafield: 'Q3', width: 150 },
            { text: 'Встреча', datafield: 'Q4', width: 60 },
            { text: 'Регистрация', datafield: 'Q6', width: 90 },
            { text: 'Кофе-брейк?', datafield: 'Q8', width: 100 },
            { text: 'JavaScript Services', datafield: 'Q10', width: 160 },
            { text: 'SQL\'фобия', datafield: 'Q18', width: 160 },
            { text: 'Xamarin', datafield: 'Q14', width: 160 },
            { text: 'Мне понравилось', datafield: 'Q24', width: 180 },
            { text: 'Я хочу предложить', datafield: 'Q25', width: 180 },
            { text: 'Мы ещё встретимся?', datafield: 'Q22', width: 150 },
        ]
    });
}