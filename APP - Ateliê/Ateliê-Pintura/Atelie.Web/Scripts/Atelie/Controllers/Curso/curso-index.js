var Atelie = Atelie || {};
Atelie.Controller = Atelie.Controller || {};
Atelie.Controller.Curso = Atelie.Controller.Curso || {};

$(function () {
    Atelie.Helpers.TableFormat.aplicarLarguraHeader('#table-header-curso', ["29%", "29%", "29%", "13%"]);

    $("#formPrincipal").on("submit", function (e) {
        e.preventDefault();
        Atelie.Controller.Curso.Salvar();
    });

    Atelie.Controller.Curso.CarregarDados();
});

Atelie.Controller.Curso.CarregarDados = function () {
    $.get(Atelie.Controller.Curso.UrlListar, function (data) {
        var tabela = $("#tabelaDados");
        if (data.Success) {
            tabela.empty();

            if (data.Object.length === 0) {
                tabela.append('<tr><td colspan="4" class="text-center">Nenhum curso encontrado.</td></tr>');
                return;
            }

            data.Object.forEach(function (curso) {
                var status = curso.Ativo ? '<span class="text-success">Ativo</span>' : '<span class="text-warning">Inativo</span>';
                var valor = parseFloat(curso.ValorMensal).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
                var linha = `
                    <tr>
                        <td>${curso.Nome}</td>
                        <td>${valor}</td>
                        <td>${status}</td>
                        <td>
                            <button class="btn btn-sm btn-info" onclick="Atelie.Controller.Curso.AbrirModalEditar(${curso.Id})" title="Editar"><i class="fas fa-edit mr-2"></i></button> 
                            <button class="btn btn-sm btn-warning" onclick="Atelie.Controller.Curso.Switch(${curso.Id}, '${curso.Nome}')" title="Alterar Status"><i class="fas fa-toggle-on mr-2"></i></button>
                            <button class="btn btn-sm btn-danger" onclick="Atelie.Controller.Curso.Excluir(${curso.Id}, '${curso.Nome}')" title="Excluir Curso"><i class="fas fa-trash mr-2"></i></button>
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

Atelie.Controller.Curso.AbrirModalNovo = function () {
    $("#modalTitle").text("Cadastrar Novo Curso");
    $("#formPrincipal")[0].reset();
    $("#formId").val(0);
    $("#formAtivo").prop('checked', true);
    $('#modalFormulario').modal('show');
};

Atelie.Controller.Curso.AbrirModalEditar = function (id) {
    $.get(Atelie.Controller.Curso.UrlEditar, { id: id }, function (response) {
        if (response.Success) {
            var curso = response.Object;
            $("#modalTitle").text('Editar Curso: "' + curso.Nome + '"');

            $("#formId").val(curso.Id);
            $("#formNome").val(curso.Nome);
            $("#formValorMensal").val(curso.ValorMensal);
            $("#formAtivo").prop('checked', curso.Ativo);

            $('#modalFormulario').modal('show');
        }
    });
};

Atelie.Controller.Curso.Salvar = function (e) {
    var cursoData = {
        Id: $("#formId").val(),
        Nome: $("#formNome").val(),
        ValorMensal: $("#formValorMensal").val(),
        Ativo: $("#formAtivo").is(':checked')
    };

    $.post(Atelie.Controller.Curso.UrlSalvarCurso, { curso: cursoData }, function (data) {
        if (data.Success) {
            $('#modalFormulario').modal('hide');
            Atelie.Helpers.ModalMessage.GlobalShow("Sucesso!", data.MessageList, true);
            Atelie.Controller.Curso.CarregarDados();
        } else {
            Atelie.Helpers.ModalMessage.GlobalShow("Erro de Validação", data.MessageList, false);
        }
    }).fail(function (jqXHR) {
        Atelie.Helpers.ModalMessage.GlobalShow("Erro!", jqXHR.dataJSON.MessageList, false);
    });
};

Atelie.Controller.Curso.Excluir = function (id, nome) {
    var message = `Tem certeza que deseja EXCLUIR PERMANENTEMENTE o curso ${nome}? Esta ação não pode ser desfeita.`;
    var title = "Exclusão PERMANENTE!";

    Atelie.Helpers.ModalMessage.ConfirmacaoShow(title, message, () => {
        $.post(Atelie.Controller.Curso.UrlExcluirCurso, { id: id }, function (response) {
            if (response.Success) {
                Atelie.Helpers.ModalMessage.GlobalShow("Sucesso!", response.MessageList, true);
                Atelie.Controller.Curso.CarregarDados();
            } else {
                Atelie.Helpers.ModalMessage.GlobalShow("Erro", response.MessageList, false);
            }
        }).fail(function (jqXHR) {
            Atelie.Helpers.ModalMessage.GlobalShow("Erro!", jqXHR.responseJSON.MessageList, false);
        });
    });
};

Atelie.Controller.Curso.Switch = function (id, nome) {
    var message = `Tem certeza que deseja alterar o status do curso "${nome}"? Caso torne o curso INATIVO, suas turmas serão DESATIVADAS e desmatriculará todos os ALUNOS associados!`;
    var title = "Alteração de Status";

    Atelie.Helpers.ModalMessage.ConfirmacaoShow(title, message, () => {
        $.post(Atelie.Controller.Curso.UrlSwitchCursos, { id: id }, function (response) {
            if (response.Success) {
                Atelie.Controller.Curso.CarregarDados();
            } else {
                Atelie.Helpers.ModalMessage.GlobalShow("Erro de Validação", response.MessageList, false);
            }
        }).fail(function (jqXHR) {
            Atelie.Helpers.ModalMessage.GlobalShow("Erro!", jqXHR.responseJSON.MessageList, false);
        });
    });
};