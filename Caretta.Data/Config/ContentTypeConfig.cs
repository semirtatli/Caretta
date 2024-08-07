using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Caretta.Core.Entity;
using System.Reflection.Emit;

namespace Caretta.Data.Config
{
    /*
    internal class ContentTypeConfig : IEntityTypeConfiguration<ContentType>
    {
        public void Configure(EntityTypeBuilder<ContentType> builder)
        {
            builder.HasKey(ct => ct.Id);

            builder.Property(ct => ct.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasMany(c => c.Contents)
                   .WithOne(ct => ct.ContentType)
                   .HasForeignKey(c => c.ContentTypeId);


            builder.Property(ct => ct.Id).HasColumnName("ID");
            builder.Property(ct => ct.Name).HasColumnName("Name");
        }
    }*/
}
