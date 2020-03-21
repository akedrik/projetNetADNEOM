using NetCoreApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreApp.Core.Interfaces.Repositories
{
    public interface IArticleRepository : IAsyncRepository<Article>
    {
    }
}
