using Atelie.Core.Entities;
using NHibernate;
using System.Linq;
using Atelie.Infrastructure.Abstractions.Repositories;

namespace Atelie.Infrastructure.Repositories
{
    public class NHibernateRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly ISession _session;

        public NHibernateRepository(ISession session)
        {
            _session = session;
        }

        public TEntity GetById(long id)
        {
            return _session.Get<TEntity>(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _session.Query<TEntity>();
        }

        public void SaveOrUpdate(TEntity entity)
        {
            _session.SaveOrUpdate(entity);
        }

        public void Delete(TEntity entity)
        {
            _session.Delete(entity);
        }

        public void Delete(long id)
        {
            var entity = GetById(id);

            if (entity != null)
                Delete(entity);
        }
    }
}
