using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCoreApp.Core.Entities;

namespace NetCoreApp.Infrastructure.Data.Configuration
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasOne(a => a.Categorie)
               .WithMany(c => c.Articles)
               .HasForeignKey(a => a.CategorieId)
               .HasConstraintName("ForeignKey_Article_Categorie")
               .IsRequired();
        }
    }
}
