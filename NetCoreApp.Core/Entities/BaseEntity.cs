using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreApp.Core.Entities
{
    public abstract class BaseEntity
    {
        [Required]
        public virtual int Id { get; protected set; }
        public virtual DateTime DateSaisie { get; protected set; }
        public virtual DateTime DateModification { get; protected set; }

        public virtual void UpdateDateModification()
        {
            DateModification = DateTime.Now;
        }
    }
}
