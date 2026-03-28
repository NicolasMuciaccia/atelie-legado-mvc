Atelie.Helpers.ModalMessage = {};

/**
 * Exibe um modal de aviso global para o usuário.
 * @param {string} title
 * @param {string | string[]} message
 * @param {boolean} isSuccess
 */
Atelie.Helpers.ModalMessage.GlobalShow = function (title, message, isSuccess) {

    var modalHeader = $('#globalModalHeader');
    var modalTitle = $('#globalModalTitle');
    var modalBody = $('#globalModalBody');

    modalHeader.removeClass('modal-header-success modal-header-error');

    if (isSuccess) {
        modalHeader.addClass('modal-header-success');
    } else {
        modalHeader.addClass('modal-header-error');
    }

    modalTitle.text(title);

    modalBody.empty();

    if (Array.isArray(message)) {
        var list = $('<ul>');
        message.forEach(function (item) {
            list.append($('<li>').text(item));
        });
        modalBody.append(list);
    } else {
        modalBody.append($('<p>').text(message));
    }

    $('#globalModal').modal('show');
};

/**
* Exibe um modal de confirmação de ação global
* @param {string} title
* @param {string} message
* @param {boolean} isSuccess
*/
Atelie.Helpers.ModalMessage.ConfirmacaoShow = function (title, message, onConfirmCallback) {

    var modalTitle = $('#confirmacaoModalTitle');
    var modalBody = $('#confirmacaoModalBody');
    var btnConfirmar = $('#btnConfirmarAcao');

    modalTitle.text(title);
    modalBody.empty().append($('<p>').text(message));

    btnConfirmar.off('click').on('click', function () {
        if (typeof onConfirmCallback === 'function') {
            onConfirmCallback();
        }

        $('#confirmacaoModal').modal('hide');
    });

    $('#confirmacaoModal').modal('show');
};