using Atelie.Infrastructure.Persistence.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Atelie.Infrastructure.Persistence
{
    public static class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        public static ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory == null)
            {
                _sessionFactory = BuildSessionFactory();
            }

            return _sessionFactory;
        }

        private static ISessionFactory BuildSessionFactory()
        {
            return Fluently.Configure()
                .Database(
                    PostgreSQLConfiguration.Standard
                        .ConnectionString(c => c.FromConnectionStringWithKey("AtelieDB"))
                        .ShowSql()
                )
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<AlunoMap>()
                )
                .BuildSessionFactory();
        }
    }
}