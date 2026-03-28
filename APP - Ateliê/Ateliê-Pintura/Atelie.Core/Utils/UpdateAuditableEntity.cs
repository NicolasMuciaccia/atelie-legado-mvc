using System;
using Atelie.Core.Entities;

namespace Atelie.Core.Utils
{
    public static class AuditHelper
    {
        public static TEntity UpdateAuditFields<TEntity>(TEntity entity) where TEntity : AuditableEntity
        {
            long adminUserId = 1974;

            if (entity.Id == 0 )
            {
                entity.CriadoEm = DateTime.UtcNow;
                entity.CriadoPor = adminUserId;
            }

            entity.AtualizadoEm = DateTime.UtcNow;
            entity.AtualizadoPor = adminUserId;

            return entity;
        }
    }
}
