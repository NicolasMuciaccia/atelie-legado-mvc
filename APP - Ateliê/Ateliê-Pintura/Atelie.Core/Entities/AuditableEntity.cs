using System;

namespace Atelie.Core.Entities
{
    public abstract class AuditableEntity : Entity
    {
        public virtual long CriadoPor { get; set; }
        public virtual DateTime CriadoEm { get; set; } = DateTime.Now;
        public virtual long AtualizadoPor { get; set; }
        public virtual DateTime AtualizadoEm { get; set; } = DateTime.Now;
    }
}
    