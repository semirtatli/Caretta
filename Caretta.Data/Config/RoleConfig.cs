using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Caretta.Core.Entity;

namespace Caretta.Data.Config
{
    internal class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.RoleType)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(r => r.Id).HasColumnName("ID");
            builder.Property(r => r.RoleType).HasColumnName("ROLETYPE");
        }
    }
}
