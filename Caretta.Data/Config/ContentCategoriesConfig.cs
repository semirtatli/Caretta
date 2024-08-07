using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Caretta.Core.Entity;

namespace Caretta.Data.Config
{
    public class ContentCategoriesConfig : IEntityTypeConfiguration<ContentCategories>
    {
        public void Configure(EntityTypeBuilder<ContentCategories> builder)
        {
            //for associative tables
            builder.HasKey(cc => new { cc.ContentId, cc.CategoryId });


            builder.HasOne(c => c.Content)
                   .WithMany(cc => cc.ContentCategories)
                   .HasForeignKey(cc => cc.ContentId);

            builder.HasOne(c => c.Category)
                   .WithMany(cc => cc.ContentCategories)
                   .HasForeignKey(cc => cc.CategoryId);

            builder.Property(cc => cc.CategoryId).HasColumnName("CATEGORY_ID");
            builder.Property(cc => cc.ContentId).HasColumnName("CONTENT_ID");
        }
    }
}
