using NetCoreApp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreApp.Core.Interfaces.Repositories
{
    public interface IArticleRepository : IAsyncRepository<Article>
    {
        Article GetByLibelleAsync(string libelle);
        Task<IEnumerable<Article>> GetContainsLibelleAsync(string libelle);
        Article GetByLibelleWithNoIdAsync(int id, string libelle);
        int MaxId();
    }
}
