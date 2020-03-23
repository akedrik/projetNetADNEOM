using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreApp.Core.Entities
{
    public class Categorie : BaseEntity
    {
        [Required]
        [StringLength(250)]
        public string Libelle { get; set; }
        public List<Article> Articles { get; set; }

        public Categorie()
        {
                
        }
        
        public Categorie(int id, string libelle)
        {
            Id = id;
            Libelle = libelle;
            DateSaisie = DateTime.Now;
            DateModification = DateTime.Now;
        }

        public void  setId(int id)
        {
            Id = id;
        }
    }
}
