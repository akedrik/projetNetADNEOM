using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreApp.Core.Entities
{
    public class Article : BaseEntity
    {
        [Required]
        [StringLength(250)]
        public string Libelle { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public double Prix { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        [Required]
        public int CategorieId { get; set; }
        public Categorie Categorie { get; set; }

        public Article() { }

        public Article(int id, string libelle, double prix, int stock, int categorieId)
        {
            Id = id;
            Libelle = libelle;
            Prix = prix;
            Stock = stock;
            CategorieId = categorieId;
            DateSaisie = DateTime.Now;
            DateModification = DateTime.Now;
        }
    }
}
