using Caretta.Core.Entity;
using Caretta.Data.Config;
using Microsoft.EntityFrameworkCore;
//using ContentType = Caretta.Core.Entity.ContentType;

namespace Caretta.Data.Context
{
    public class CarettaContext : DbContext

    {
        public CarettaContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<ContentCategories> ContentCategories { get; set; }
        public DbSet<UserLikeContents> UserLikeContents { get; set; }
        public DbSet<UserLikeComments> UserLikeComments { get; set; }
        public DbSet<UserFavouriteCategories> UserFavouriteCategories { get; set; }
        // public DbSet<ContentType> ContentTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            

            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new CommentConfig());
            modelBuilder.ApplyConfiguration(new ContentConfig());
            modelBuilder.ApplyConfiguration(new UserFavouriteCategoriesConfig());
            modelBuilder.ApplyConfiguration(new ContentCategoriesConfig());
            modelBuilder.ApplyConfiguration(new UserLikeContentsConfig());
            modelBuilder.ApplyConfiguration(new UserLikeCommentsConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new UserRoleConfig());
        }

    }
}
