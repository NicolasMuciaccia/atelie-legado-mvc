using Atelie.Core.Exceptions;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Abstractions.Repositories;
using Atelie.Infrastructure.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Atelie.Core.Resources;
using Atelie.Core.Enums;
using Atelie.Core.Utils;
using Atelie.Infrastructure.Services.DTOs;
using Atelie.Infrastructure.Services.DTOs.SelectListDTO;

namespace Atelie.Infrastructure.Services
{
    public class TurmaService : ITurmaService
    {
        private readonly ITurmaRepository _turmaRepository;
        private readonly ISession _session;

        public TurmaService(ITurmaRepository turmaRepository, ISession session)
        {
            _turmaRepository = turmaRepository;
            _session = session;
        }

        public List<Turma> ListarTodos()
        {
            return _turmaRepository.GetAll().Where(t => t.Ativo == true).ToList();
        }

        public List<Turma> ListarTodosComFiltro(TurmaFiltroDTO filtro)
        {
            return _turmaRepository.ListarTodosComFiltro(filtro);
        }

        public Turma ObterPorId(long id)
        {
            return _turmaRepository.GetById(id);
        }

        public Turma Salvar(Turma turma)
        {
            var validationErrors = new ValidationError();

            if (!Enum.IsDefined(typeof(DiaDaSemana), turma.DiaDaSemana))
                validationErrors.AddError("Turma.DiaDaSemana", string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DiaDaSemana));

            if (turma.Horario == TimeSpan.Zero)
                validationErrors.AddError("Turma.Horario", string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Horario));

            if (turma.Curso == null || turma.Curso.Id == 0)
                validationErrors.AddError("Curso.Id", string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Curso));

            if (_turmaRepository.ExisteTurmasRepetidas(turma))
                validationErrors.AddError("Turma.Horario", string.Format(GlobalMessages.ErroTurmaMesmoHorario, turma.GetNomeTurma()));

            if(turma.Alunos.Any() && !turma.Ativo)
                validationErrors.AddError("Turma.Ativo", GlobalMessages.ErroTurmaInativaComAluno);

            if (validationErrors.Errors.Any())
                throw new ValidationRuleException(validationErrors.Errors);

            AuditHelper.UpdateAuditFields(turma);

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    var turmaPersistente = _session.Merge(turma);

                    transaction.Commit();
                    return turmaPersistente;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Excluir(long id)
        {
            var turma = _turmaRepository.GetById(id);
            if (turma == null)
                throw new EntityNotFoundException("Turma", id);

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _turmaRepository.Delete(turma);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DesativarEmLote(List<long> turmaIds)
        {
            if (turmaIds == null || !turmaIds.Any()) 
                return;

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _turmaRepository.DesativarEmLote(turmaIds);
                    _turmaRepository.DesativarTurmaAlunosEmLote(turmaIds);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void SwitchAtivo(long id)
        {
            var turma = _turmaRepository.GetById(id);

            if (!turma.Curso.Ativo)
                throw new ValidationRuleException("Curso.Id", GlobalMessages.ErroCursoInativo);

            if (turma == null)
                throw new EntityNotFoundException("Turma", id);

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    turma.Ativo = !turma.Ativo;

                    if (!turma.Ativo)
                        turma.Alunos.Clear();

                    turma = AuditHelper.UpdateAuditFields(turma);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public List<TurmaSelectListDTO> ObterTurmaPorAlunoId(long id)
        {
            var turmas = _turmaRepository.ObterTurmaPorAlunoId(id);

            return turmas.Select(t => new TurmaSelectListDTO
            {
                Id = t.Id,
                Nome = t.GetNomeTurma()
            }).ToList();
        }
    }
}