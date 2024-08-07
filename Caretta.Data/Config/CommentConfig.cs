using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Caretta.Core.Entity;
using System.Reflection.Emit;

namespace Caretta.Data.Config
{
    internal class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Text)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(c => c.Id).HasColumnName("ID");
            builder.Property(c => c.ContentId).HasColumnName("CONTENT_ID");
            builder.Property(c => c.Text).HasColumnName("TEXT");
        }
    }
}

