using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Caretta.Core.Entity;

namespace Caretta.Data.Config
{
    internal class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.HasOne(u => u.User)
                   .WithMany(ur => ur.UserRoles)
                   .HasForeignKey(ur => ur.UserId);

            builder.HasOne(r => r.Role)
                   .WithMany(ur => ur.UserRoles)
                   .HasForeignKey(ur => ur.RoleId);

            builder.Property(ur => ur.UserId).HasColumnName("USER_ID");
            builder.Property(ur => ur.RoleId).HasColumnName("ROLE_ID");
        }
    }
}
