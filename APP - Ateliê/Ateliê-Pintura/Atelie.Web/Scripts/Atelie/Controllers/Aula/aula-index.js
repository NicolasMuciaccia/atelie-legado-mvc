var Atelie = Atelie || {};
Atelie.Controller = Atelie.Controller || {};
Atelie.Controller.Aula = Atelie.Controller.Aula || {};

$(function () {
    Atelie.Helpers.TableFormat.aplicarLarguraHeader('#table-header-aula', ["34%", "14%", "14%", "14%", "14%", "10%"]);

    $('#filtroFormulario').on('submit', function (event) {
        event.preventDefault();
        Atelie.Controller.Aula.Filtrar();
    });

    $('#formTurmaId').on('change', function () {
        var turmaId = $(this).val();
        Atelie.Controller.Aula.CarregarAlunosParaPresenca(turmaId);
    });

    $("#formPrincipal").on("submit", function (e) {
        e.preventDefault();
        Atelie.Controller.Aula.Salvar();
    });

    $('#selectAdicionarAluno').on('change', function () {
        var alunoId = $(this).val();
        if (alunoId) {
            var alunoNome = $(this).find('option:selected').text();
            Atelie.Controller.Aula.AdicionarAlunoNaPresenca(alunoId, alunoNome);
            $(this).val('');
        }
    });

    $('#tabelaPresencas').on('click', '.btn-remover-presenca', function () {
        $(this).closest('tr').remove();
    });

    if (Atelie.Controller.Aula.TurmaIdFiltroInicial != null && Atelie.Controller.Aula.TurmaIdFiltroInicial !== undefined)
        $("#formTurmaFiltro").val(Atelie.Controller.Aula.TurmaIdFiltroInicial).change();

    Atelie.Controller.Aula.Filtrar();
});

Atelie.Controller.Aula.Filtrar = function () {
    var filtro = Atelie.Controller.Aula.Filtro();
    Atelie.Controller.Aula.CarregarDados(filtro);
};

Atelie.Controller.Aula.Filtro = function () {
    var ativoFiltroVal = $("#formAtivoFiltro").val();

    var ativoFiltro = null;
    if (ativoFiltroVal === "true") ativoFiltro = true;
    else if (ativoFiltroVal === "false") ativoFiltro = false;

    return {
        TurmaId: $("#formTurmaFiltro").val(),
        DataDaAulaDe: $("#formDataFiltroDe").val(),
        DataDaAulaAte: $("#formDataFiltroAte").val(),
        AtivoFiltro: ativoFiltro
    };
};

Atelie.Controller.Aula.CarregarDados = function (filtro) {
    $.get(Atelie.Controller.Aula.UrlListar, filtro, function (data) {
        var tabela = $("#tabelaDados");
        tabela.empty();
        if (data.Success && data.Object.length > 0) {
            data.Object.forEach(function (aula) {   
                var turmaNome = aula.TurmaNome ? aula.TurmaNome : '<span>Não informado!</span>';
                var status = aula.TurmaAtiva ? '<span class="text-success">Sim</span>' : '<span class="text-warning">Não</span>';
                var linha = `
                    <tr>
                        <td>${turmaNome}</td>
                        <td>${status}</td>
                        <td>${Atelie.Controller.Aula.FormatarDataJson(aula.DataAula)}</td>
                        <td>${aula.TotalPresencas}</td>
                        <td>${aula.TotalFaltas}</td>
                        <td>
                            <button class="btn btn-sm btn-info" onclick="Atelie.Controller.Aula.AbrirModalEditar(${aula.Id})" title="Editar / Ver Presenças"><i class="fas fa-edit"></i></button>
                            <button class="btn btn-sm btn-danger" onclick="Atelie.Controller.Aula.Excluir(${aula.Id}, '${aula.Nome}')" title="Excluir"><i class="fas fa-trash"></i></button>
                        </td>
                    </tr>`;
                tabela.append(linha);
            });
        } else {
            var colspan = $("#tabelaDados").closest('table').find('thead th').length;
            tabela.append(`<tr><td colspan="${colspan}" class="text-center">Nenhuma aula encontrada.</td></tr>`);
        }
    });
};

Atelie.Controller.Aula.CarregarAlunosParaPresenca = function (turmaId) {
    var tabela = $('#tabelaPresencas');
    tabela.empty();

    if (!turmaId)
        return;

    var alunos = Atelie.Controller.Aula.TurmasComAlunosMap[turmaId];

    if (alunos && alunos.length > 0) {
        alunos.forEach(function (aluno) {
            Atelie.Controller.Aula.AdicionarAlunoNaPresenca(aluno.Id, aluno.Nome, true);
        });
    }
};

Atelie.Controller.Aula.CarregarAlunosParaEdicao = function (presencas) {
    var tabela = $('#tabelaPresencas');
    tabela.empty();

    if (presencas && presencas.length > 0) {
        presencas.forEach(function (presenca) {
            Atelie.Controller.Aula.AdicionarAlunoNaPresenca(presenca.AlunoId, presenca.AlunoNome, presenca.Presente, presenca.Id);
        });
    } else {
        tabela.append('<tr><td colspan="2" class="text-center">Esta turma não possui alunos matriculados.</td></tr>');
    }
};

Atelie.Controller.Aula.AdicionarAlunoNaPresenca = function (alunoId, alunoNome, isPresente = true, Id = 0) {
    if ($(`#tabelaPresencas tr[data-aluno-id="${alunoId}"]`).length > 0)
        return;

    var checked = isPresente ? 'checked' : '';
    var linha = `
        <tr data-aluno-id="${alunoId}">
            <td>
                ${alunoNome}
                <input type="hidden" class="presenca-id" value="${Id}" />
            </td>
            <td class="text-center">
                <input type="checkbox" class="form-check-input" ${checked}>
                <span>Presente? </span>
            </td>
            <td class="text-center">
                <button type="button" class="btn btn-sm btn-danger btn-remover-presenca" title="Remover Aluno da Chamada"><i class="fas fa-trash"></i></button>
            </td>
        </tr>`;
    $('#tabelaPresencas').append(linha);
};

Atelie.Controller.Aula.AbrirModalNovo = function () {
    $("#modalTitle").text("Agendar Nova Aula");
    $("#formPrincipal")[0].reset();
    $("#formId").val(0);
    $('#tabelaPresencas').empty();
    $('#formTurmaId').show().prop('disabled', false);
    $('#turmaInfo').hide();

    Atelie.Controller.Aula.ConferirTurmaAtiva();
    $('#modalFormulario').modal('show');
};

Atelie.Controller.Aula.AbrirModalEditar = function (aulaId) {
    $.get(Atelie.Controller.Aula.UrlEditar, { id: aulaId }, function (response) {
        if (response.Success) {
            var aula = response.Object;
            $("#modalTitle").text("Editar Aula - " + Atelie.Controller.Aula.FormatarDataJson(aula.DataAula));

            $("#formId").val(aula.Id);
            $("#formDataAula").val(Atelie.Controller.Aula.FormatarDataParaInput(aula.DataAula));
            $("#formTurmaId").val(aula.TurmaId);

            Atelie.Controller.Aula.CarregarAlunosParaEdicao(aula.Presencas);

            setTimeout(function () {
                $('#formTurmaId').val(aula.TurmaId);
            }, 500);

            if (aula.TurmaId) {
                $('#turmaInfo').hide();
            } else {
                $('#turmaInfo').text("Aula referente a uma turma que foi excluída.").show();
            }

            Atelie.Controller.Aula.ConferirTurmaAtiva(aula.TurmaAtiva);

            $('#modalFormulario').modal('show');
        }
    });
};

Atelie.Controller.Aula.Salvar = function () {
    var aulaId = $('#formId').val();
    var presencasData = [];
    $('#tabelaPresencas tr').each(function () {
        var linha = $(this);
        presencasData.push({
            Id: linha.find('.presenca-id').val(),
            Presente: linha.find('input[type="checkbox"]').is(':checked'),
            Aluno: {
                Id: linha.data('aluno-id')
            },
            Aula: {
                Id: aulaId
            }
        });
    });

    var aulaData = {
        Id: $('#formId').val(),
        Turma: { Id: $('#formTurmaId').val() },
        DataAula: $('#formDataAula').val(),
        Presencas: []
    };

    $.post(Atelie.Controller.Aula.UrlSalvarAula, { aula: aulaData, presencas: presencasData }, function (response) {  
        if (response.Success) {
            $('#modalFormulario').modal('hide');
            Atelie.Helpers.ModalMessage.GlobalShow("Sucesso!", response.MessageList, true);
            Atelie.Controller.Aula.Filtrar();
        } else {
            Atelie.Helpers.ModalMessage.GlobalShow("Erro de Validação", response.MessageList, false);
        }
    }).fail(function (jqXHR) {
        Atelie.Helpers.ModalMessage.GlobalShow("Erro!", jqXHR.responseJSON.MessageList, false);
    });
};

Atelie.Controller.Aula.Excluir = function (id, stringReferencia) {
    var message = `Tem certeza que deseja EXCLUIR PERMANENTEMENTE a aula "${stringReferencia}"? Esta ação não pode ser desfeita.`;
    var title = "Exclusão PERMANENTE!";

    Atelie.Helpers.ModalMessage.ConfirmacaoShow(title, message, () => {
        $.post(Atelie.Controller.Aula.UrlExcluirAula, { id: id }, function (response) {
            if (response.Success) {
                Atelie.Helpers.ModalMessage.GlobalShow("Sucesso!", response.MessageList, true);
                Atelie.Controller.Aula.CarregarDados();
            } else {
                Atelie.Helpers.ModalMessage.GlobalShow("Erro", response.MessageList, false);
            }
        }).fail(function (jqXHR) {
            Atelie.Helpers.ModalMessage.GlobalShow("Erro!", jqXHR.responseJSON.MessageList, false);
        });
    });
};

Atelie.Controller.Aula.FormatarDataJson = function (data) {
    if (!data)
        return '-';

    var timestamp = parseInt(data.replace(/\/Date\((\d+)\)\//, "$1"), 10);
    var dataObj = new Date(timestamp);

    var dia = String(dataObj.getDate()).padStart(2, '0');
    var mes = String(dataObj.getMonth() + 1).padStart(2, '0');
    var ano = dataObj.getFullYear();

    return `${dia}/${mes}/${ano}`;
};

Atelie.Controller.Aula.FormatarDataParaInput = function (data) {
    if (!data)
        return '';

    var timestamp = parseInt(data.replace(/\/Date\((\d+)\)\//, "$1"), 10);
    var dataObj = new Date(timestamp);

    var ano = dataObj.getFullYear();
    var mes = String(dataObj.getMonth() + 1).padStart(2, '0');
    var dia = String(dataObj.getDate()).padStart(2, '0');

    return `${ano}-${mes}-${dia}`;
};
Atelie.Controller.Aula.ConferirTurmaAtiva = function (turmaAtiva) {
    if (turmaAtiva || turmaAtiva == undefined) {
        $("#btnSalvar").prop('disabled', false);
        $("#modalEditarInfo").hide();
    } else {
        $("#btnSalvar").prop('disabled', true);
        $("#modalEditarInfo").text("A turma associada está inativa, não é possível alterar esses dados.").show();
    }
}
