using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreApp.Core.Entities
{
    public class Categorie : BaseEntity
    {
        public string Libelle { get; set; }

        public Categorie(int id, string libelle)
        {
            Id = id;
            Libelle = libelle;
            DateSaisie = DateTime.Now;
            DateModification = DateTime.Now;
        }
    }
}
