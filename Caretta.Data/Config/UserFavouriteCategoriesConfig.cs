using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Caretta.Core.Entity;

namespace Caretta.Data.Config
{
    public class UserFavouriteCategoriesConfig : IEntityTypeConfiguration<UserFavouriteCategories>
    {
        public void Configure(EntityTypeBuilder<UserFavouriteCategories> builder)
        {
            //for associative tables
            builder.HasKey(uc => new { uc.UserId, uc.CategoryId });


            builder.HasOne(u => u.User)
                   .WithMany(uc => uc.UserFavouriteCategories)
                   .HasForeignKey(uc => uc.UserId);

            builder.HasOne(c => c.Category)
                   .WithMany(uc => uc.UserFavouriteCategories)
                   .HasForeignKey(uc => uc.CategoryId);

            builder.Property(cc => cc.CategoryId).HasColumnName("CATEGORY_ID");
            builder.Property(uc => uc.UserId).HasColumnName("USER_ID");
        }
    }
}
