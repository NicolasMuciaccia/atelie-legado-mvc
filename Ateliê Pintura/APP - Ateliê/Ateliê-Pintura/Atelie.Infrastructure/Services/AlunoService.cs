using Atelie.Core.Enums;
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
using System.Net.Mail;

namespace Atelie.Infrastructure.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly ISession _session;

        public AlunoService(IAlunoRepository alunoRepository, ISession session)
        {
            _alunoRepository = alunoRepository;
            _session = session;
        }

        public List<Aluno> ListarTodos()
        {
            return _alunoRepository.GetAll().ToList();
        }

        public List<Aluno> ListarTodosComFiltro(AlunoFiltroDTO filtro)
        {
            return _alunoRepository.ListarTodosComFiltro(filtro);
        }

        public Aluno ObterPorId(long id)
        {
            return _alunoRepository.GetById(id);
        }

        public Aluno Salvar(Aluno aluno)
        {
            var validationErrors = new ValidationError();

            if (string.IsNullOrWhiteSpace(aluno.Nome))
                validationErrors.AddError("Aluno.Nome", string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome));

            if (string.IsNullOrWhiteSpace(aluno.DescricaoContato))
                validationErrors.AddError("Aluno.DescricaoContato", string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DescricaoContato));

            if (aluno.TipoContato == TipoContato.NaoInformado)
                validationErrors.AddError("Aluno.TipoContato", string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.TipoContato));

            if (aluno.DiaPagamentoPreferencial < 1 || aluno.DiaPagamentoPreferencial > 31)
                validationErrors.AddError("Aluno.DiaPagamentoPreferencial", GlobalMessages.DiaPagamentoInvalido);

            if (_alunoRepository.NomeDeAlunoExiste(aluno))
                validationErrors.AddError("Aluno.Nome", string.Format(GlobalMessages.NomeExistente, GlobalMessages.Aluno));

            if (aluno.TipoContato == TipoContato.Email && !string.IsNullOrWhiteSpace(aluno.DescricaoContato))
            {
                try
                {
                    var emailAddress = new MailAddress(aluno.DescricaoContato);
                }
                catch (FormatException)
                {
                    validationErrors.AddError("Aluno.DescricaoContato", GlobalMessages.EmailInvalido);
                }
            }

            if (validationErrors.Errors.Any())
                throw new ValidationRuleException(validationErrors.Errors);

            aluno = AuditHelper.UpdateAuditFields(aluno);

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _alunoRepository.SaveOrUpdate(aluno);
                    transaction.Commit();
                    return aluno;
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
            var aluno = _alunoRepository.GetById(id);

            if (aluno == null)
                throw new EntityNotFoundException("Aluno", id);

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _alunoRepository.Delete(aluno);
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
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    var aluno = _alunoRepository.GetById(id);
                    if (aluno == null)
                        throw new EntityNotFoundException("Aluno", id);

                    aluno.Ativo = !aluno.Ativo;
                    AuditHelper.UpdateAuditFields(aluno);

                    _alunoRepository.DesativarTurmaAlunos(aluno.Id);

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