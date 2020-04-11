using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreApp.Core.Entities
{
    public class Categorie : BaseEntity
    {
        [Required(ErrorMessage = "Le champ Libellé est requis.")]
        [StringLength(250, MinimumLength = 2, 
            ErrorMessage ="La longeur du libellé doit entre 2 et 250 caratères.")]
        [Display(Name ="Libellé")]
        public string Libelle { get; set; }
        public virtual List<Article> Articles { get; set; }

        public Categorie() { }       
        
        public Categorie(int id, string libelle)
        {
            Id = id;
            Libelle = libelle;
            DateSaisie = DateTime.Now;
            DateModification = DateTime.Now;
        }

        public void SetId(int id) => Id = id;
    }
}
