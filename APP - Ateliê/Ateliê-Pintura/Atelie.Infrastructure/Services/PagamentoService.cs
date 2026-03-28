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

namespace Atelie.Infrastructure.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly ISession _session;

        public PagamentoService(IPagamentoRepository pagamentoRepository, ISession session)
        {
            _pagamentoRepository = pagamentoRepository;
            _session = session;
        }

        public List<Pagamento> ListarTodos()
        {
            return _pagamentoRepository.GetAll().ToList();
        }

        public List<Pagamento> ListarTodosComFiltro(PagamentoFiltroDTO filtro)
        {
            return _pagamentoRepository.ListarTodosComFiltro(filtro);
        }

        public Pagamento ObterPorId(long id)
        {
            return _pagamentoRepository.GetById(id);
        }

        public Pagamento Salvar(Pagamento pagamento)
        {
            var validationErrors = new ValidationError();

            if (pagamento.ValorPago <= 0)
                validationErrors.AddError("Pagamento.ValorPago", GlobalMessages.ValorPagamentoInvalido);

            if (pagamento.Aluno == null || pagamento.Aluno.Id == 0)
                validationErrors.AddError("Pagamento.Aluno.Id" , GlobalMessages.ErroPagamentoSemAluno);

            if (pagamento.Pendente)
            {
                if (pagamento.DataEfetivacao.HasValue)
                    validationErrors.AddError("Pagamento.DataEfetivacao", GlobalMessages.PagamentoPendenteEfetivacao);

                if (pagamento.TipoPagamento != TipoPagamento.Aguardando)
                    validationErrors.AddError("Pagamento.TipoPagamento", GlobalMessages.PagamentoPendenteTipoAdequado);
            }
            else
            {
                if (!pagamento.DataEfetivacao.HasValue)
                    validationErrors.AddError("Pagamento.DataEfetivacao", GlobalMessages.PagamentoEfetivadoDataEfetivacao);

                if (pagamento.TipoPagamento == TipoPagamento.Aguardando)
                    validationErrors.AddError("Pagamento.TipoPagamento", GlobalMessages.PagamentoEfetivadoTipoPagamentoValido);
            }

            if (_pagamentoRepository.ExisteOutroPagamentoNoMesmoMes(pagamento))
                validationErrors.AddError("Pagamento.DataReferencia", GlobalMessages.PagamentoDuplicadoMes);

            if (validationErrors.Errors.Any())
                throw new ValidationRuleException(validationErrors.Errors);

            pagamento = AuditHelper.UpdateAuditFields(pagamento);

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _pagamentoRepository.SaveOrUpdate(pagamento);

                    transaction.Commit();
                    return pagamento;
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
            var pagamento = _pagamentoRepository.GetById(id);
            if (pagamento == null)
                throw new EntityNotFoundException("Pagamento", id);

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _pagamentoRepository.Delete(pagamento);

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