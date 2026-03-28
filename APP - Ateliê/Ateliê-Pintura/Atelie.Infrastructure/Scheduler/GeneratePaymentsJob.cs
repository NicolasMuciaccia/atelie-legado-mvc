using NHibernate;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Atelie.Infrastructure.Scheduler
{
    [DisallowConcurrentExecution]
    public class GeneratePaymentsJob : IJob
    {
        private readonly ISession _session;

        public GeneratePaymentsJob(ISession session)
        {
            _session = session;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var sql = @"
                INSERT INTO PAGAMENTOS (
                    ID_CRIADO_POR, DT_ATUALIZADO_EM, ID_ATUALIZADO_POR,
                    DT_REFERENCIA, VL_PAGO, DT_EFETIVACAO,
                    TP_PAGAMENTO, FL_PENDENTE, ID_ALUNO, ID_TURMA
                )
                SELECT
                    1974, CURRENT_TIMESTAMP, 1974,
                    CAST(DATE_TRUNC('month', CURRENT_DATE) AS DATE),
                    c.VL_MENSAL, NULL, 0, TRUE,
                    ta.ID_ALUNO, ta.ID_TURMA
                FROM
                    TURMAS_ALUNOS ta
                    INNER JOIN TURMAS t ON ta.ID_TURMA = t.ID_TURMA
                    INNER JOIN CURSOS c ON t.ID_CURSO = c.ID_CURSO
                    INNER JOIN ALUNOS a ON ta.ID_ALUNO = a.ID_ALUNO
                WHERE
                    t.FL_ATIVA = TRUE AND c.FL_ATIVA = TRUE AND a.FL_ATIVA = TRUE
                AND
                    NOT EXISTS (
                        SELECT 1 FROM PAGAMENTOS p
                        WHERE p.ID_ALUNO = ta.ID_ALUNO
                          AND p.ID_TURMA = ta.ID_TURMA
                          AND DATE_TRUNC('month', p.DT_REFERENCIA) = DATE_TRUNC('month', CURRENT_DATE)
                    );
            ";

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    var query = _session.CreateSQLQuery(sql);
                    query.ExecuteUpdate();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return Task.CompletedTask;
        }
    }
}
