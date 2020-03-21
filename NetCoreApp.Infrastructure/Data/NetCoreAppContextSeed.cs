using NetCoreApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreApp.Infrastructure.Data
{
    public class NetCoreAppContextSeed
    {

        public static async Task SeedAsync(NetCoreAppContext dbContext)
        {
            try
            {
                if (!dbContext.Categories.Any())
                {
                    dbContext.Categories.AddRange(GetCategories());
                    await dbContext.SaveChangesAsync();
                }

                if (!dbContext.Articles.Any())
                {
                    dbContext.Articles.AddRange(GetArticles());
                    await dbContext.SaveChangesAsync();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static IEnumerable<Categorie> GetCategories()
        {
            return new List<Categorie>()
            {
                new Categorie(1,"Teeshirt"),
                new Categorie(2,"Pantalon"),
                new Categorie(3,"Maillot"),
                new Categorie(4,"Chaussure"),
                new Categorie(5,"Chaussette"),
                new Categorie(6,"Châpeau"),
                new Categorie(7,"Ballon"),
                new Categorie(8,"Sifflet"),
                new Categorie(9,"TV"),
                new Categorie(10,"PC")
            };
        }

        private static IEnumerable<Article> GetArticles()
        {
            return new List<Article>()
            {
                new Article(1,"Polo Lacoste",10,35,1),
                new Article(2,"Polo Tommy HilFiger",20,75,1),
                new Article(3,"Polo Ralph Laurent",5,5,1)

            };
        }


    }
}
