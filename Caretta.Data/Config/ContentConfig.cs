using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Caretta.Core.Entity;
using System.Reflection.Emit;

namespace Caretta.Data.Config
{
    public class ContentConfig : IEntityTypeConfiguration<Content>
    {
        public void Configure(EntityTypeBuilder<Content> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(c => c.Body)
            .IsRequired();

            builder.HasMany(cm => cm.Comments)
            .WithOne(c => c.Content)
            .HasForeignKey(cm => cm.ContentId);

            

            builder.Property(c => c.Id).HasColumnName("ID");
            builder.Property(c => c.Title).HasColumnName("TITLE");
            builder.Property(c => c.Body).HasColumnName("BODY");
            //builder.Property(c => c.ContentCategoryId).HasColumnName("CONTENTCATEGORYID");
        }
    }
}
