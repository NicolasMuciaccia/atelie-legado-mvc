var Atelie = Atelie || {};
Atelie.Controller = Atelie.Controller || {};
Atelie.Controller.Aluno = Atelie.Controller.Aluno || {};

$(function () {
    Atelie.Helpers.TableFormat.aplicarLarguraHeader('#table-header-aluno', ["16%", "12%", "18%", "12%", "10%", "12%", "16%"]);

    $('#formTipoContato').change(function () {
        Atelie.Controller.Aluno.AplicarMascaraDescricaoContato(); 
    });

    $('#filtroFormulario').on('submit', function (event) {
        event.preventDefault();
        Atelie.Controller.Aluno.Filtrar();
    });

    $("#formPrincipal").on("submit", function (e) {
        e.preventDefault();
        Atelie.Controller.Aluno.Salvar();
    });

    Atelie.Controller.Aluno.Filtrar();
});

Atelie.Controller.Aluno.Filtrar = function () {
    var filtro = Atelie.Controller.Aluno.Filtro();
    Atelie.Controller.Aluno.CarregarDados(filtro);
}

Atelie.Controller.Aluno.Filtro = function () {
    var ativoFiltroVal = $("#formAtivoFiltro").val();
    var mesFiltroVal = $("#formMesFiltro").val();

    var ativoFiltro = null;
    if (ativoFiltroVal === "true") ativoFiltro = true;
    else if (ativoFiltroVal === "false") ativoFiltro = false;

    var mesFiltro = (mesFiltroVal == 0) ? new Date().getMonth() + 1 : mesFiltroVal;

    var filtro = {
        NomeFiltro: $("#formNomeFiltro").val(),
        DiaPagamentoPreferencialFiltro: $("#formDiaPagamentoPreferencialFiltro").val(),
        AtivoFiltro: ativoFiltro,
        MesDasPresencasFiltro: mesFiltro,
    }

    return filtro;
}

Atelie.Controller.Aluno.CarregarDados = function (filtro) {
    $.get(Atelie.Controller.Aluno.UrlListar, filtro, function (data) {
        var tabela = $("#tabelaDados");
        if (data.Success) {
            tabela.empty();

            if (data.Object.length === 0) {
                tabela.append('<tr><td colspan="4" class="text-center">Nenhum aluno encontrado.</td></tr>');
                return;
            }
            
            data.Object.forEach(function (aluno) {
                var status = aluno.Ativo ? '<span class="text-success">Ativo</span>' : '<span class="text-warning">Inativo</span>';
                var linha = `
                    <tr>
                        <td>${aluno.Nome}</td>
                        <td>${aluno.TipoContatoDisplay}</td>
                        <td>${aluno.DescricaoContato}</td>
                        <td>${aluno.DiaPagamentoPreferencial}</td>
                        <td>${status}</td>
                        <td>${aluno.PresencaMensal}</td>
                        <td>
                            <button class="btn btn-sm btn-info" onclick="Atelie.Controller.Aluno.AbrirModalEditar(${aluno.Id})" title="Editar"><i class="fas fa-edit mr-2"></i></button>
                            <button class="btn btn-sm btn-secondary" onclick="Atelie.Controller.Aluno.VisualizarPagamentos(${aluno.Id})" title="Visualizar Pagamentos"><i class="fas fa-eye mr-2"></i></button>
                            <button class="btn btn-sm btn-warning" onclick="Atelie.Controller.Aluno.Switch(${aluno.Id}, '${aluno.Nome}')" title="Alterar Status"><i class="fas fa-toggle-on mr-2"></i></button>
                            <button class="btn btn-sm btn-danger" onclick="Atelie.Controller.Aluno.Excluir(${aluno.Id}, '${aluno.Nome}')" title="Excluir Aluno"><i class="fas fa-trash mr-2"></i></button>
                        </td>
                    </tr>`;
                tabela.append(linha);
            });
        }
        else {
            tabela.append('<tr><td colspan="4" class="text-center">A requisição não foi bem sucedida</td></tr>');
        }
    });
};

Atelie.Controller.Aluno.AbrirModalNovo = function () {
    $("#modalTitle").text("Cadastrar Novo Aluno");
    $("#formPrincipal")[0].reset();
    $("#formId").val(0);
    $("#formAtivo").prop('checked', true);
    $("#formTipoContato").val(0).trigger('change');

    $('#modalFormulario').modal('show');
};

Atelie.Controller.Aluno.AbrirModalEditar = function (id) {
    $.get(Atelie.Controller.Aluno.UrlEditar, { id: id }, function (response) {
        if (response.Success) {
            var aluno = response.Object;
            $("#modalTitle").text('Editar Aluno: "' + aluno.Nome + '"');

            $("#formId").val(aluno.Id);
            $("#formNome").val(aluno.Nome);
            $("#formTipoContato").val(aluno.TipoContato).trigger('change');
            $("#formDiaPagamentoPreferencial").val(aluno.DiaPagamentoPreferencial);
            $("#formDescricaoContato").val(aluno.DescricaoContato);
            $("#formAtivo").prop('checked', aluno.Ativo);

            $('#modalFormulario').modal('show');
        }
    });
};

Atelie.Controller.Aluno.Salvar = function () {
    var alunoData = {
        Id: $("#formId").val(),
        Nome: $("#formNome").val(),
        TipoContato: $("#formTipoContato").val(),
        DescricaoContato: $("#formDescricaoContato").val(),
        DiaPagamentoPreferencial: $("#formDiaPagamentoPreferencial").val(),
        Ativo: $("#formAtivo").is(':checked')
    };

    $.post(Atelie.Controller.Aluno.UrlSalvarAluno, { aluno: alunoData }, function (data) {
        if (data.Success) {
            $('#modalFormulario').modal('hide');
            Atelie.Helpers.ModalMessage.GlobalShow("Sucesso!", data.MessageList, true);
            Atelie.Controller.Aluno.Filtrar();
        } else {
            Atelie.Helpers.ModalMessage.GlobalShow("Erro de Validação", data.MessageList, false);
        }
    }).fail(function (jqXHR) {
        Atelie.Helpers.ModalMessage.GlobalShow("Erro!", jqXHR.dataJSON.MessageList, false);
    });
};

Atelie.Controller.Aluno.Excluir = function (id, nome) {
    var message = `Tem certeza que deseja EXCLUIR PERMANENTEMENTE o aluno ${nome}? Esta ação não pode ser desfeita.`;
    var title = "Exclusão PERMANENTE!";

    Atelie.Helpers.ModalMessage.ConfirmacaoShow(title, message, () => {
        $.post(Atelie.Controller.Aluno.UrlExcluirAluno, { id: id }, function (response) {
            if (response.Success) {
                Atelie.Helpers.ModalMessage.GlobalShow("Sucesso!", response.MessageList, true);
                Atelie.Controller.Aluno.Filtrar();
            } else {
                Atelie.Helpers.ModalMessage.GlobalShow("Erro", response.MessageList, false);
            }
        }).fail(function (jqXHR) {
            Atelie.Helpers.ModalMessage.GlobalShow("Erro!", jqXHR.responseJSON.MessageList, false);
        });
    });
};

Atelie.Controller.Aluno.Switch = function (id, nome) {
    var message = `Tem certeza que deseja alterar o status do aluno ${nome}? Caso torne o cliente INATIVO, ele se DESMATRICULARÁ de TODAS as TURMAS!`;
    var title = "Alteração de Status";

    Atelie.Helpers.ModalMessage.ConfirmacaoShow(title, message, () => {
        $.post(Atelie.Controller.Aluno.UrlSwitchAlunos, { id: id }, function (response) {
            if (response.Success) {
                Atelie.Controller.Aluno.Filtrar();
            } else {
                Atelie.Helpers.ModalMessage.GlobalShow("Erro de Validação", response.MessageList, false);
            }
        }).fail(function (jqXHR) {
            Atelie.Helpers.ModalMessage.GlobalShow("Erro!", jqXHR.responseJSON.MessageList, false);
        });
    });
};

Atelie.Controller.Aluno.VisualizarPagamentos = function (alunoId) {
    var urlFinal = Atelie.Controller.Aluno.UrlPagamentos + '?id=' + alunoId;
    window.location.href = urlFinal;
};

Atelie.Controller.Aluno.AplicarMascaraDescricaoContato = function () {
    var tipo = parseInt($('#formTipoContato').val());
    var descricao = $('#formDescricaoContato');

    descricao.val("");
    descricao.unmask();

    if (tipo === 0 || isNaN(tipo)) {
        descricao.prop('disabled', true);
        descricao.prop('placeholder', 'Escolha um tipo válido');
        return;
    }

    descricao.prop('disabled', false);

    switch (tipo) {
        case 1:
            descricao.mask('(00) 0000-0000');
            descricao.prop('placeholder', '(00) 0000-0000');
            break;

        case 2:
            descricao.mask('(00) 00000-0000');
            descricao.prop('placeholder', '(00) 00000-0000');
            break;

        case 3:
            descricao.prop('placeholder', 'ex: osana@gmail.com');
            break;

        default:
            descricao.prop('disabled', true);
            descricao.prop('placeholder', '');
            break;
    }
};