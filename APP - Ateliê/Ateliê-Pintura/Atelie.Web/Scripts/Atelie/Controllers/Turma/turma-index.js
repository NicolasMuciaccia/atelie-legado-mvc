var Atelie = Atelie || {};
Atelie.Controller = Atelie.Controller || {};
Atelie.Controller.Turma = Atelie.Controller.Turma || {};

$(function () {
    Atelie.Helpers.TableFormat.aplicarLarguraHeader('#table-header-turma', ["20%", "20%", "15%", "14%", "15%", "16%"]);

    Atelie.Controller.Turma.AlunosOptionsCache = $('#selectAdicionarAluno').html();

    $('#filtroFormulario').on('submit', function (event) {
        event.preventDefault();
        Atelie.Controller.Turma.Filtrar();
    });

    $('#selectAdicionarAluno').on('change', function () {
        var alunoId = $(this).val();
        if (alunoId) {
            var alunoNome = $(this).find('option:selected').text();
            Atelie.Controller.Turma.AdicionarAlunoNaTabela(alunoId, alunoNome);
            $(this).val('');
        }
    });

    $('#tabelaAlunosMatriculados').on('click', '.btn-remover-aluno', function () {
        var linha = $(this).closest('tr');
        var alunoId = linha.data('aluno-id');
        var alunoNome = linha.find('td:first').text();

        Atelie.Controller.Turma.RemoverAlunoDaTabela(alunoId, alunoNome);
    });

    $("#formPrincipal").on("submit", function (e) {
        e.preventDefault();
        Atelie.Controller.Turma.Salvar();
    });

    Atelie.Controller.Turma.Filtrar();
});

Atelie.Controller.Turma.Filtrar = function () {
    var filtro = Atelie.Controller.Turma.Filtro();
    Atelie.Controller.Turma.CarregarDados(filtro);
}

Atelie.Controller.Turma.Filtro = function () {
    var ativoFiltroVal = $("#formAtivoFiltro").val();
    return {
        CursoId: $("#formCursoFiltro").val(),
        DiaDaSemana: $("#formDiaSemanaFiltro").val(),
        Ativo: ativoFiltroVal === "" ? null : ativoFiltroVal === "true"
    };
}

Atelie.Controller.Turma.CarregarDados = function (filtro) {
    $.get(Atelie.Controller.Turma.UrlListar, filtro, function (data) {
        var tabela = $("#tabelaDados");
        tabela.empty();
        if (data.Success && data.Object.length > 0) {
            data.Object.forEach(function (turma) {
                var status = turma.Ativo ? '<span class="text-success">Ativa</span>' : '<span class="text-warning">Inativa</span>';
                var horario = Atelie.Controller.Turma.FormatStringHorario(turma.Horario);
                var disabled = turma.Ativo ? '' : 'disabled';

                var linha = `
                    <tr>
                        <td>${turma.CursoNome}</td>
                        <td>${turma.DiaDaSemanaDisplay}</td>
                        <td>${horario}</td>
                        <td>${status}</td>
                        <td>${turma.TotalAlunos}</td>
                        <td>
                            <button class="btn btn-sm btn-info" onclick="Atelie.Controller.Turma.AbrirModalEditar(${turma.Id})" title="Editar"><i class="fas fa-edit"></i></button>
                            <button class="btn btn-sm btn-secondary" onclick="Atelie.Controller.Turma.VisualizarAulas(${turma.Id})" title="Visualizar Aulas" ${disabled}><i class="fas fa-calendar-alt"></i></button>
                            <button class="btn btn-sm btn-warning" onclick="Atelie.Controller.Turma.Switch(${turma.Id}, '${turma.NomeCompleto}')" title="Alterar Status"><i class="fas fa-toggle-on"></i></button>
                            <button class="btn btn-sm btn-danger" onclick="Atelie.Controller.Turma.Excluir(${turma.Id}, '${turma.NomeCompleto}')" title="Excluir Turma"><i class="fas fa-trash"></i></button>
                        </td>
                    </tr>`;
                tabela.append(linha);
            });
        } else {
            var colspan = $("#tabelaDados").closest('table').find('thead th').length;
            tabela.append(`<tr><td colspan="${colspan}" class="text-center">Nenhuma turma encontrada.</td></tr>`);
        }
    });
};

Atelie.Controller.Turma.AbrirModalNovo = function () {
    $("#modalTitle").text("Cadastrar Nova Turma");
    $("#formPrincipal")[0].reset();
    $("#formId").val(0);
    $("#formAtivo").prop('checked', true);

    $('#tabelaAlunosMatriculados').empty();
    Atelie.Controller.Turma.AtualizarAlunosIdsHidden();

    $('#selectAdicionarAluno').html(Atelie.Controller.Turma.AlunosOptionsCache);

    $('#modalFormulario').modal('show');
};

Atelie.Controller.Turma.AbrirModalEditar = function (id) {
    $.get(Atelie.Controller.Turma.UrlEditar, { id: id }, function (response) {
        if (response.Success) {
            var turma = response.Object;
            $("#modalTitle").text(`Editar Turma: "${turma.CursoNome} - ${turma.DiaDaSemanaDisplay}/${Atelie.Controller.Turma.FormatStringHorario(turma.Horario)}"`);

            $("#formId").val(turma.Id);
            $("#formCursoId").val(turma.CursoId);
            $("#formDiaDaSemana").val(turma.DiaDaSemana);
            $("#formHorario").val(Atelie.Controller.Turma.FormatStringHorario(turma.Horario));
            $("#formAtivo").prop('checked', turma.Ativo);

            $('#tabelaAlunosMatriculados').empty();
            $('#selectAdicionarAluno').html(Atelie.Controller.Turma.AlunosOptionsCache);

            if (turma.Alunos && turma.Alunos.length > 0) {
                turma.Alunos.forEach(function (aluno) {
                    Atelie.Controller.Turma.AdicionarAlunoNaTabela(aluno.Id, aluno.Nome);
                });
            }
            Atelie.Controller.Turma.AtualizarAlunosIdsHidden();

            $('#modalFormulario').modal('show');
        }
    });
};

Atelie.Controller.Turma.Salvar = function () {
    var alunosParaEnviar = [];

    $('#tabelaAlunosMatriculados tr').each(function () {
        var alunoId = $(this).data('aluno-id');
        var alunoNome = $(this).find('td:first').text();
        alunosParaEnviar.push({ Id: alunoId, Nome: alunoNome });
    });

    var turmaData = {
        Id: $("#formId").val(),
        DiaDaSemana: $("#formDiaDaSemana").val(),
        Horario: $("#formHorario").val(),
        Ativo: $("#formAtivo").is(':checked'),
        Curso: {
            Id: $("#formCursoId").val()
        },
        Alunos: alunosParaEnviar
    };

    $.post(Atelie.Controller.Turma.UrlSalvarTurma, { turma: turmaData }, function (data) {
        if (data.Success) {
            $('#modalFormulario').modal('hide');
            Atelie.Helpers.ModalMessage.GlobalShow("Sucesso!", data.MessageList, true);
            Atelie.Controller.Turma.Filtrar();
        } else {
            Atelie.Helpers.ModalMessage.GlobalShow("Erro de Validação", data.MessageList, false);
        }
    }).fail(function (jqXHR) {
        Atelie.Helpers.ModalMessage.GlobalShow("Erro!", jqXHR.responseJSON.MessageList, false);
    });
};

Atelie.Controller.Turma.Excluir = function (id, nome) {
    var message = `Tem certeza que deseja EXCLUIR PERMANENTEMENTE a turma ${nome}? Os ALUNOS e PRESENÇAS não estarão mais associados a essa turma, porém, continuirão existindo. Esta ação não pode ser desfeita.`;
    var title = "Exclusão PERMANENTE!";

    Atelie.Helpers.ModalMessage.ConfirmacaoShow(title, message, () => {
        $.post(Atelie.Controller.Turma.UrlExcluirTurma, { id: id }, function (response) {
            if (response.Success) {
                Atelie.Helpers.ModalMessage.GlobalShow("Sucesso!", response.MessageList, true);
                Atelie.Controller.Turma.Filtrar();
            } else {
                Atelie.Helpers.ModalMessage.GlobalShow("Erro", response.MessageList, false);
            }
        }).fail(function (jqXHR) {
            Atelie.Helpers.ModalMessage.GlobalShow("Erro!", jqXHR.responseJSON.MessageList, false);
        });
    });
};

Atelie.Controller.Turma.Switch = function (id, nome) {
    var message = `Tem certeza que deseja alterar o status da turma "${nome}"? Caso torne a turma INATIVA, ela desmatriculará todos os ALUNOS associados!`;
    var title = "Alteração de Status";

    Atelie.Helpers.ModalMessage.ConfirmacaoShow(title, message, () => {
        $.post(Atelie.Controller.Turma.UrlSwitchTurmas, { id: id }, function (response) {
            if (response.Success) {
                Atelie.Controller.Turma.Filtrar();
            } else {
                Atelie.Helpers.ModalMessage.GlobalShow("Erro de Validação", response.MessageList, false);
            }
        }).fail(function (jqXHR) {
            Atelie.Helpers.ModalMessage.GlobalShow("Erro!", jqXHR.responseJSON.MessageList, false);
        });
    });
};

Atelie.Controller.Turma.VisualizarAulas = function (turmaId) {
    window.location.href = Atelie.Controller.Turma.UrlAulas + '?id=' + turmaId;
};

Atelie.Controller.Turma.AdicionarAlunoNaTabela = function (id, nome) {
    if ($(`#tabelaAlunosMatriculados tr[data-aluno-id="${id}"]`).length > 0) {
        return;
    }

    var novaLinha = `<tr data-aluno-id="${id}"><td>${nome}</td><td class="text-end"><button type="button" class="btn btn-sm btn-danger btn-remover-aluno"><i class="fas fa-times"></i></button></td></tr>`;
    $('#tabelaAlunosMatriculados').append(novaLinha);
    $(`#selectAdicionarAluno option[value="${id}"]`).remove();

    this.AtualizarAlunosIdsHidden();
};

Atelie.Controller.Turma.RemoverAlunoDaTabela = function (id, nome) {
    $(`#tabelaAlunosMatriculados tr[data-aluno-id="${id}"]`).remove();
    $('#selectAdicionarAluno').append($('<option>', { value: id, text: nome }));

    this.AtualizarAlunosIdsHidden();
};

Atelie.Controller.Turma.AtualizarAlunosIdsHidden = function () {
    var ids = [];

    $('#tabelaAlunosMatriculados tr').each(function () {
        ids.push($(this).data('aluno-id'));
    });

    $('#alunosIdsHidden').val(ids.join(','));
};

Atelie.Controller.Turma.FormatStringHorario = function (horario) {
    return String(horario.Hours).padStart(2, '0') + ':' + String(horario.Minutes).padStart(2, '0');
}