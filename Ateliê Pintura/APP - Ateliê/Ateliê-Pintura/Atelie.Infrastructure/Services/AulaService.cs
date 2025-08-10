using Atelie.Core.Exceptions;
using Atelie.Core.Resources;
using Atelie.Core.Utils;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Abstractions.Repositories;
using Atelie.Infrastructure.Abstractions.Services;
using Atelie.Infrastructure.Services.DTOs;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atelie.Infrastructure.Services
{
    public class AulaService : IAulaService
    {
        private readonly IAulaRepository _aulaRepository;
        private readonly ISession _session;

        public AulaService(IAulaRepository aulaRepository, ISession session)
        {
            _aulaRepository = aulaRepository;
            _session = session;
        }

        public List<Aula> ListarTodos()
        {
            return _aulaRepository.GetAll().ToList();
        }

        public List<Aula> ListarTodosComFiltro(AulaFiltroDTO filtro)
        {
            return _aulaRepository.ListarTodosComFiltro(filtro);
        }

        public Aula ObterPorId(long id)
        {
            return _aulaRepository.GetById(id);
        }

        public Aula Salvar(Aula aula)
        {
            var validationErrors = new ValidationError();

            if (aula.Turma == null || aula.Turma.Id == 0)
                validationErrors.AddError("Aula.Turma.Id", GlobalMessages.ErroAulaSemTurma);

            if (aula.DataAula == DateTime.MinValue)
                validationErrors.AddError("Aula.DataDaAula", string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataDaAula));

            if (validationErrors.Errors.Any())
                throw new ValidationRuleException(validationErrors.Errors);

            aula = AuditHelper.UpdateAuditFields(aula);

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _aulaRepository.SaveOrUpdate(aula);

                    transaction.Commit();
                    return aula;
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
            var aula = _aulaRepository.GetById(id);

            if (aula == null)
                throw new EntityNotFoundException("Aula", id);

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _aulaRepository.Delete(aula);

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