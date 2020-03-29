using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreApp.Core.Entities
{
    public abstract class BaseEntity
    {
       
        public virtual int Id { get;  set; }
        public virtual DateTime DateSaisie { get; set; }
        public virtual DateTime DateModification { get; set; }

        public virtual void UpdateDateModification() => DateModification = DateTime.Now;
    }
}
