using Atelie.Core.Exceptions;
using Atelie.Core.Resources;
using Atelie.Core.Utils;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Abstractions.Repositories;
using Atelie.Infrastructure.Abstractions.Services;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atelie.Infrastructure.Services
{
    public class CursoService : ICursoService
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly ITurmaRepository _turmaRepository;
        private readonly ISession _session;

        public CursoService(ICursoRepository cursoRepository, ITurmaRepository turmaRepository, ISession session)
        {
            _cursoRepository = cursoRepository;
            _turmaRepository = turmaRepository;
            _session = session;
        }

        public List<Curso> ListarTodos()
        {
            return _cursoRepository.GetAll().ToList();
        }

        public Curso ObterPorId(long id)
        {
            return _cursoRepository.GetById(id);
        }

        public List<long> ObterTurmaIdPorCursoId(long id)
        {
            return _turmaRepository.ObterTurmaIdPorCursoId(id);
        }

        public Curso Salvar(Curso curso)
        {
            var validationErrors = new ValidationError();

            if (string.IsNullOrWhiteSpace(curso.Nome))
                validationErrors.AddError("Curso.Nome", string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome));

            if (curso.ValorMensal <= 0)
                validationErrors.AddError("Curso.ValorMensal", string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.ValorMensal));

            if (_cursoRepository.NomeDeCursoExiste(curso))
                validationErrors.AddError("Curso.Nome", string.Format(GlobalMessages.NomeExistente, GlobalMessages.Curso));

            if (validationErrors.Errors.Any())
                throw new ValidationRuleException(validationErrors.Errors);

            curso = AuditHelper.UpdateAuditFields(curso);

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _cursoRepository.SaveOrUpdate(curso);
                    transaction.Commit();
                    return curso;
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
            var curso = _cursoRepository.GetById(id);

            if (curso == null)
                throw new EntityNotFoundException("Curso", id);

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _cursoRepository.Delete(id);
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
            var cursosNomes = _turmaRepository.ObterNomesDeCursosAtivosPorCursoId(id);

            if (cursosNomes.Any())
                throw new BusinessRuleException(string.Format(GlobalMessages.ErroDesativarCurso , cursosNomes));

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    var curso = _cursoRepository.GetById(id);
                    if (curso == null)
                        throw new EntityNotFoundException("Curso", id);

                    _cursoRepository.SwitchAtivo(curso);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}