using System.Collections.Generic;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Services.DTOs;

namespace Atelie.Infrastructure.Abstractions.Services
{
    public interface IAlunoService : IService<Aluno>
    {
        List<Aluno> ListarTodosComFiltro(AlunoFiltroDTO filtro);
        void SwitchAtivo(long id);
    }
}