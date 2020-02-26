var helloWord = function () {
    return $("body").append("Olá, Mundo!")

}

var get = function () {
    $.ajax({
        type: "GET",
        url: '/Home/Consulta',
        contentType: "application/json; charset=utf-8",
        data: {},
        dataType: "json",
        success: function (response) {
            console.log(response[0]['Titulo']);
            $("body").html(JSON.stringify(response));
        },
        error: function (ex) {
            var r = jQuery.parseJSON(response.responseText);
            alert("Message: " + r.Message);
            alert("StackTrace: " + r.StackTrace);
            alert("ExceptionType: " + r.ExceptionType);
        }
    });

}

var getDisponibilidade = function () {
    $.ajax({
        type: "GET",
        url: '/Home/ConsultaDisponibilidade',
        contentType: "application/json; charset=utf-8",
        data: {},
        dataType: "json",
        success: function (json) {
            console.log(json);
            $("body").html(JSON.stringify(json));
        },
        error: function (ex) {
            var r = jQuery.parseJSON(json.responseText);
            alert("Message: " + r.Message);
            alert("StackTrace: " + r.StackTrace);
            alert("ExceptionType: " + r.ExceptionType);
        }
    });

}
