using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreApp.Core.Entities
{
    public class Article : BaseEntity
    {
        public string Libelle { get; set; }
        public double Prix { get; set; }
        public int Stock { get; set; }
        public Categorie Categorie { get; set; }
    }
}
