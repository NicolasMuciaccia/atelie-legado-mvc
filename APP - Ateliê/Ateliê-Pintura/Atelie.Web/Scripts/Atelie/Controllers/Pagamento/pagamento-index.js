var Atelie = Atelie || {};
Atelie.Controller = Atelie.Controller || {};
Atelie.Controller.Pagamento = Atelie.Controller.Pagamento || {};

$(function () {
    Atelie.Helpers.TableFormat.aplicarLarguraHeader('#table-header-pagamento', ["16%", "12%", "12%", "12%", "12%", "11%", "16%", "9%"]);

    if (Atelie.Controller.Pagamento.AlunoIdFiltroInicial && Atelie.Controller.Pagamento.ListaDeAlunos) {
        var alunoEncontrado = Atelie.Controller.Pagamento.ListaDeAlunos.find(aluno => aluno.Value == Atelie.Controller.Pagamento.AlunoIdFiltroInicial);

        if (alunoEncontrado) {
            $('#formAlunoNomeFiltro').val(alunoEncontrado.Text);
        }
    }

    $('#filtroFormulario').on('submit', function (event) {
        event.preventDefault();
        Atelie.Controller.Pagamento.Filtrar();
    });

    $('#formAlunoId').on('change', function () {
        var alunoId = $(this).val();
        Atelie.Controller.Pagamento.CarregarTurmas(alunoId);
    });

    $("#formPrincipal").on("submit", function (e) {
        e.preventDefault();
        Atelie.Controller.Pagamento.Salvar();
    });

    Atelie.Controller.Pagamento.Filtrar();
});

Atelie.Controller.Pagamento.Filtrar = function () {
    var filtro = Atelie.Controller.Pagamento.Filtro();
    Atelie.Controller.Pagamento.CarregarDados(filtro);
}

Atelie.Controller.Pagamento.Filtro = function () {
    var pendenteVal = $("#formPendenteFiltro").val();
    var tipoPagamentoVal = $("#formTipoPagamentoFiltro").val();
    var mesVal = $("#formMesFiltro").val();
    var anoVal = $("#formAnoFiltro").val();

    return {
        AlunoNomeFiltro: $("#formAlunoNomeFiltro").val(),
        MesFiltro: mesVal == "0" ? null : parseInt(mesVal),
        AnoFiltro: anoVal ? parseInt(anoVal) : null,
        PendenteFiltro: pendenteVal === "" ? null : pendenteVal === "true",
        TipoPagamentoFiltro: tipoPagamentoVal == "" ? null : parseInt(tipoPagamentoVal)
    };
};

Atelie.Controller.Pagamento.CarregarDados = function (filtro) {
    $.get(Atelie.Controller.Pagamento.UrlListar, filtro, function (data) {
        var tabela = $("#tabelaDados");
        tabela.empty();

        if (data.Success) {
            if (data.Object.length === 0) {
                tabela.append('<tr><td colspan="8" class="text-center">Nenhum pagamento encontrado.</td></tr>');
                return;
            }

            data.Object.forEach(function (pagamento) {
                var dataRef = Atelie.Controller.Pagamento.formatarDataJson(pagamento.DataReferencia);
                var dataEfet = pagamento.DataEfetivacao ? Atelie.Controller.Pagamento.formatarDataJson(pagamento.DataEfetivacao) : '-';

                var valor = pagamento.ValorPago.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
                var tipo = pagamento.TipoPagamentoDisplay;
                var status = pagamento.Pendente ? '<span class="text-warning">Sim</span>' : '<span class="text-success">Não</span>';
                var turmaNome = pagamento.TurmaNome ? `${pagamento.TurmaNome}` : 'Não informado!'

                var linha = `
                    <tr>
                        <td>${pagamento.AlunoNome}</td>
                        <td>${dataRef}</td>
                        <td>${dataEfet}</td>
                        <td>${valor}</td>
                        <td>${tipo}</td>
                        <td>${status}</td>
                        <td>${turmaNome}</td>
                        <td class="">
                            <button class="btn btn-sm btn-info" onclick="Atelie.Controller.Pagamento.AbrirModalEditar(${pagamento.Id}, ${pagamento.AlunoId}, ${pagamento.TurmaId})" title="Editar"><i class="fas fa-edit mr-2"></i></button>
                            <button class="btn btn-sm btn-danger" onclick="Atelie.Controller.Pagamento.Excluir(${pagamento.Id})" title="Excluir Pagamento"><i class="fas fa-trash mr-2"></i></button>
                        </td>
                    </tr>`;

                tabela.append(linha);
            });
        } else {
            tabela.append('<tr><td colspan="8" class="text-center">Erro ao carregar os pagamentos.</td></tr>');
        }
    });
};

Atelie.Controller.Pagamento.AbrirModalNovo = function () {
    $("#modalTitle").text("Cadastrar Novo Pagamento");
    $("#formPrincipal")[0].reset();

    $("#formId").val(0);
    $("#formPendente").prop("checked", true);
    $("#formTipoPagamento").val("0").trigger("change");
    $("#formValorPago").val("");
    $("#formDataReferencia").val("");
    $("#formDataEfetivacao").val("");

    $('#formAlunoId').val('').trigger("change").prop('disabled', false);
    $('#formTurmaId').prop('disabled', true);
    $('#turmaInfo').hide();

    $('#modalFormulario').modal('show');
};

Atelie.Controller.Pagamento.AbrirModalEditar = function (pagamentoId) {
    $.get(Atelie.Controller.Pagamento.UrlEditar, { id: pagamentoId }, function (response) {
        if (response.Success) {
            var pagamento = response.Object;
            $("#modalTitle").text("Editar Pagamento");

            $("#formId").val(pagamento.Id);
            $("#formTipoPagamento").val(pagamento.TipoPagamento).trigger('change');
            $("#formValorPago").val(pagamento.ValorPago);
            $("#formDataReferencia").val(Atelie.Controller.Pagamento.formatarDataParaInput(pagamento.DataReferencia));
            $("#formDataEfetivacao").val(Atelie.Controller.Pagamento.formatarDataParaInput(pagamento.DataEfetivacao));
            $("#formPendente").prop("checked", pagamento.Pendente);

            $('#formAlunoId').val(pagamento.AlunoId).prop('disabled', true);

            if (pagamento.TurmaId) {
                Atelie.Controller.Pagamento.CarregarTurmas(pagamento.AlunoId);

                setTimeout(function () {
                    $('#formTurmaId').val(pagamento.TurmaId);
                }, 500);

                $('#formTurmaId').show();
                $('#turmaInfo').hide();
            } else {
                $('#formTurmaId').hide();
                $('#turmaInfo').text("Pagamento referente a uma turma que foi excluída.").show();
            }

            $('#modalFormulario').modal('show');
        }
    });
};

Atelie.Controller.Pagamento.Salvar = function () {
    var pagamentoData = {
        Id: $("#formId").val(),
        TipoPagamento: $("#formTipoPagamento").val(),
        ValorPago: $("#formValorPago").val(),
        DataReferencia: $("#formDataReferencia").val(),
        DataEfetivacao: $("#formDataEfetivacao").val(),
        Pendente: $("#formPendente").is(":checked"),
        Turma: {
            Id: $("#formTurmaId").val()
        },
        Aluno: {
            Id: $("#formAlunoId").val()
        }
    };

    $.post(Atelie.Controller.Pagamento.UrlSalvarPagamento, { pagamento: pagamentoData }, function (data) {
        if (data.Success) {
            $('#modalFormulario').modal('hide');
            Atelie.Helpers.ModalMessage.GlobalShow("Sucesso!", data.MessageList, true);
            Atelie.Controller.Pagamento.CarregarDados();
        } else {
            Atelie.Helpers.ModalMessage.GlobalShow("Erro de Validação", data.MessageList, false);
        }
    }).fail(function (jqXHR) {
        Atelie.Helpers.ModalMessage.GlobalShow("Erro!", jqXHR.responseJSON?.MessageList || ["Erro inesperado."], false);
    });
};

Atelie.Controller.Pagamento.Excluir = function (id) {
    var message = `Tem certeza que deseja EXCLUIR PERMANENTEMENTE o pagamento? Esta ação não pode ser desfeita.`;
    var title = "Exclusão PERMANENTE!";

    Atelie.Helpers.ModalMessage.ConfirmacaoShow(title, message, () => {
        $.post(Atelie.Controller.Pagamento.UrlExcluirPagamento, { id: id }, function (response) {
            if (response.Success) {
                Atelie.Helpers.ModalMessage.GlobalShow("Sucesso!", response.MessageList, true);
                Atelie.Controller.Pagamento.CarregarDados();
            } else {
                Atelie.Helpers.ModalMessage.GlobalShow("Erro", response.MessageList, false);
            }
        }).fail(function (jqXHR) {
            Atelie.Helpers.ModalMessage.GlobalShow("Erro!", jqXHR.responseJSON.MessageList, false);
        });
    });
};

Atelie.Controller.Pagamento.CarregarTurmas = function (id) {
    var selectTurmas = $('#formTurmaId');
    selectTurmas.empty().prop('disabled', false).append($('<option>', { value: '', text: 'A carregar...' }));

    if (!id) {
        selectTurmas.empty().prop('disabled', true).append($('<option>', { value: '', text: 'Selecione um aluno' }));
        return;
    }

    $.get(Atelie.Controller.Pagamento.UrlObterTurmas, { alunoId: id }, function (turmas) {
        selectTurmas.empty().append($('<option>', { value: '', text: 'Selecione uma turma...' }));
        turmas.forEach(function (turma) {
            selectTurmas.append($('<option>', {
                value: turma.Id,
                text: turma.Nome
            }));
        });
    });
};

Atelie.Controller.Pagamento.formatarDataJson = function (data) {
    if (!data)
        return '-';

    var timestamp = parseInt(data.replace(/\/Date\((\d+)\)\//, "$1"), 10);
    var dataObj = new Date(timestamp);

    var dia = String(dataObj.getDate()).padStart(2, '0');
    var mes = String(dataObj.getMonth() + 1).padStart(2, '0');
    var ano = dataObj.getFullYear();

    return `${dia}/${mes}/${ano}`;
};

Atelie.Controller.Pagamento.formatarDataParaInput = function (data) {
    if (!data)
        return '';

    var timestamp = parseInt(data.replace(/\/Date\((\d+)\)\//, "$1"), 10);
    var dataObj = new Date(timestamp);

    var ano = dataObj.getFullYear();
    var mes = String(dataObj.getMonth() + 1).padStart(2, '0');
    var dia = String(dataObj.getDate()).padStart(2, '0');

    return `${ano}-${mes}-${dia}`;
};