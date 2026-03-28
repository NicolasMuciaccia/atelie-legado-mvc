CREATE OR REPLACE VIEW VW_RELATORIO_PAGAMENTOS AS
SELECT
    a.ID_ALUNO,
    c.ID_CURSO,
    t.ID_TURMA,
    p.ID_PAGAMENTO,
    a.NM_ALUNO,
    CASE
        WHEN a.TP_CONTATO = 0 THEN 'Não informado'
        WHEN a.TP_CONTATO = 1 THEN 'Telefone'
        WHEN a.TP_CONTATO = 2 THEN 'Celular'
        WHEN a.TP_CONTATO = 3 THEN 'Email'
        ELSE 'Não Definido'
    END AS DS_TP_CONTATO_ALUNO,
    a.DS_CONTATO,
    p.DT_REFERENCIA,
    p.VL_PAGO,
    p.FL_PENDENTE,
    p.DT_EFETIVACAO,
    CASE
        WHEN p.TP_PAGAMENTO = 0 THEN 'Aguardando'
        WHEN p.TP_PAGAMENTO = 1 THEN 'Crédito'
        WHEN p.TP_PAGAMENTO = 2 THEN 'Débito'
        WHEN p.TP_PAGAMENTO = 3 THEN 'PIX'
        WHEN p.TP_PAGAMENTO = 4 THEN 'Dinheiro'
        ELSE 'Não Definido'
    END AS DS_TP_PAGAMENTO,
    c.NM_CURSO,
    CASE
        WHEN t.TP_DIA_SEMANA = 1 THEN 'Domingo'
        WHEN t.TP_DIA_SEMANA = 2 THEN 'Segunda-feira'
        WHEN t.TP_DIA_SEMANA = 3 THEN 'Terça-feira'
        WHEN t.TP_DIA_SEMANA = 4 THEN 'Quarta-feira'
        WHEN t.TP_DIA_SEMANA = 5 THEN 'Quinta-feira'
        WHEN t.TP_DIA_SEMANA = 6 THEN 'Sexta-feira'
        WHEN t.TP_DIA_SEMANA = 7 THEN 'Sábado'
        ELSE 'Dia não definido'
    END AS DS_DIA_SEMANA,
    t.HR_AULA,
    c.VL_MENSAL AS VL_MENSAL_CURSO

FROM
    PAGAMENTOS p
JOIN
    ALUNOS a ON p.ID_ALUNO = a.ID_ALUNO
LEFT JOIN
    TURMAS t ON p.ID_TURMA = t.ID_TURMA
LEFT JOIN
    CURSOS c ON t.ID_CURSO = c.ID_CURSO;