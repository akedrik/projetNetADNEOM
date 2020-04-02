using NetCoreApp.Core.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
                    dbContext.Categories.AddRange(GetCategoriesFromJsonFile());
                    await dbContext.SaveChangesAsync();
                }

                if (!dbContext.Articles.Any())
                {
                    dbContext.Articles.AddRange(GetArticlesFromJsonFile());
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  
        public static IEnumerable<Categorie> GetCategoriesFromJsonFile()
        {
            List<Categorie> categories = new List<Categorie>();
            var file = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot", "JsonFiles", "Categorie.json");
            if (File.Exists(file))
            {
                var json = File.ReadAllText(file);
                categories = JsonConvert.DeserializeObject<List<Categorie>>(json);
            }
            return categories;
        }

        private static IEnumerable<Article> GetArticlesFromJsonFile()
        {
            List<Article> articles = new List<Article>();
            var file = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot", "JsonFiles", "Article.json");
            if (File.Exists(file))
            {
                var json = File.ReadAllText(file);
                articles = JsonConvert.DeserializeObject<List<Article>>(json);
            }
            return articles;
        }
    }
}
