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
    public class PresencaService : IPresencaService 
    {
        private readonly IPresencaRepository _presencaRepository;
        private readonly ISession _session;

        public PresencaService(IPresencaRepository presencaRepository, ISession session)
        {
            _presencaRepository = presencaRepository;
            _session = session;
        }

        public List<Presenca> ListarTodos()
        {
            return _presencaRepository.GetAll().ToList();
        }

        public List<Presenca> ListarTodosPorAulaId(long id)
        {
            return _presencaRepository.ListarPorAulaId(id);
        }

        public Presenca ObterPorId(long id)
        {
            return _presencaRepository.GetById(id);
        }
            

        public Presenca Salvar(Presenca presenca)
        {
            var validationErrors = new ValidationError();

            if (presenca.Aluno == null || presenca.Aluno.Id == 0)
                validationErrors.AddError("Presenca.Aluno.Id", GlobalMessages.ErroPresencaSemAluno);

            if (presenca.Aula == null || presenca.Aula.Id == 0)
                validationErrors.AddError("Presenca.Aula.Id", GlobalMessages.ErroPresencaSemAula);

            if (_presencaRepository.ExisteOutraPresencaDoAlunoNaAula(presenca))
                validationErrors.AddError("Presenca.Presente", GlobalMessages.ErroAlunoComPresenca);

            if (validationErrors.Errors.Any())
                throw new ValidationRuleException(validationErrors.Errors);

            presenca = AuditHelper.UpdateAuditFields(presenca);

            using (var transaction = _session.BeginTransaction())   
            {
                try
                {
                    _presencaRepository.SaveOrUpdate(presenca);

                    transaction.Commit();
                    return presenca;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public List<Presenca> SincronizarPresencas(List<Presenca> presencas)
        {
            if (presencas == null)
                throw new ValidationRuleException("Presenca.Id", GlobalMessages.ErroAulaSemPresenca);

            var validationErrors = new ValidationError();

            var aulaId = presencas.FirstOrDefault()?.Aula?.Id;

            List<Presenca> idsParaRemover = new List<Presenca>();

            if (aulaId == null || aulaId == 0)
            {
                validationErrors.AddError("Presenca.Aula.Id", GlobalMessages.ErroPresencaSemAula);
            }
            else
            {
                idsParaRemover = _presencaRepository.ListarPorAulaId(aulaId.Value);

                foreach (var presenca in presencas)
                {
                    if (presenca.Aluno == null || presenca.Aluno.Id == 0)
                        validationErrors.AddError("Presenca.Aluno", GlobalMessages.ErroPresencaSemAluno);
                }
            }

            if (validationErrors.Errors.Any())
                throw new ValidationRuleException(validationErrors.Errors);

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    foreach (var id in idsParaRemover)
                        _presencaRepository.Delete(id);

                    foreach (var presenca in presencas)
                    {
                        var presencaUpdated = AuditHelper.UpdateAuditFields(presenca);
                        _session.Merge(presencaUpdated);
                    }

                    transaction.Commit();
                    return presencas;
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
            var presenca = _presencaRepository.GetById(id);

            if (presenca == null)
                throw new EntityNotFoundException("Presença", id);

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _presencaRepository.Delete(presenca);

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