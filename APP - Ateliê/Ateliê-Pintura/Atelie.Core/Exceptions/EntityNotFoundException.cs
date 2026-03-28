using System;

namespace Atelie.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, object id)
            : base($"A entidade '{entityName}' com o Id '{id}' não foi encontrada.")
        {
        }
    }
}
