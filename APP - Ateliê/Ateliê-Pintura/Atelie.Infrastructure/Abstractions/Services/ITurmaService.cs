using System.Collections.Generic;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Services.DTOs;
using Atelie.Infrastructure.Services.DTOs.SelectListDTO;

namespace Atelie.Infrastructure.Abstractions.Services
{
    public interface ITurmaService : IService<Turma>
    {
        List<Turma> ListarTodosComFiltro(TurmaFiltroDTO filtro);
        List<TurmaSelectListDTO> ObterTurmaPorAlunoId(long id);
        void DesativarEmLote(List<long> ids);
        void SwitchAtivo(long id);
    }
}