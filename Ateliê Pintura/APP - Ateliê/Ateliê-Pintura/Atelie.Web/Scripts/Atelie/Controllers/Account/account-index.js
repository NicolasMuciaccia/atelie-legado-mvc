var Atelie = Atelie || {};
Atelie.Controller = Atelie.Controller || {};
Atelie.Controller.Account = {};

Atelie.Controller.Account.Logar = function () {
    var loginData = {
        username: $("#username").val(),
        password: $("#password").val()
    };

    $.post(Atelie.Controller.Account.UrlLogar, { model: loginData }, function (response) {
        if (response.Success) {
            window.location.href = Atelie.Controller.Account.UrlRedirecionarHome;
        } else {
            Atelie.Helpers.ModalMessage.GlobalShow("Erro!", response.MessageList, response.Success);
        }
    });
}