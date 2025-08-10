using System.Collections.Generic;
using Atelie.Core.Entities;

namespace Atelie.Infrastructure.Abstractions.Services
{
    public interface IService<TEntity> where TEntity : Entity
    {
        List<TEntity> ListarTodos();
        TEntity ObterPorId(long id);
        TEntity Salvar(TEntity entity);
        void Excluir(long id);
    }
}
