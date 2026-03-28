using System.Linq;
using Atelie.Core.Entities;

namespace Atelie.Infrastructure.Abstractions.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        TEntity GetById(long id);
        IQueryable<TEntity> GetAll();
        void SaveOrUpdate(TEntity entity);
        void Delete(TEntity entity);
        void Delete(long id);
    }
}
