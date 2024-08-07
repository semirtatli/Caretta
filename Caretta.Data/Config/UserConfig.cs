using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Caretta.Core.Entity;

namespace Caretta.Data.Config
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.SurName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.UserName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(100);
            /*
            builder.Property(u => u.PasswordHash)
                   .IsRequired(false)
                   .HasMaxLength(100);

            builder.Property(u => u.PasswordSalt)
                   .IsRequired(false)
                   .HasMaxLength(100);
            */
            builder.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(256);
                

            builder.Property(e => e.PasswordSalt)
                .IsRequired()
                .HasMaxLength(256);
                

            builder.Property(u => u.TC)
                   .IsRequired(false)
                   .HasMaxLength(100);


            builder.Property(u => u.Id).HasColumnName("ID");
            builder.Property(u => u.Name).HasColumnName("NAME");
            builder.Property(u => u.SurName).HasColumnName("SURNAME");
            builder.Property(u => u.UserName).HasColumnName("USERNAME");
            builder.Property(u => u.Email).HasColumnName("EMAIL");
            builder.Property(u => u.PasswordSalt).HasColumnName("PASSWORDSALT");
            builder.Property(u => u.PasswordHash).HasColumnName("PASSWORDHASH");
            builder.Property(u => u.TC).HasColumnName("TC");
        }
    }
}
