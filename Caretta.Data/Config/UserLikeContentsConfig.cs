using Caretta.Core.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Data.Config
{
    public class UserLikeContentsConfig : IEntityTypeConfiguration<UserLikeContents>
    {
        public void Configure(EntityTypeBuilder<UserLikeContents> builder)
        {
            //for associative tables
            builder.HasKey(cc => new { cc.ContentId, cc.UserId });


            builder.HasOne(c => c.Content)
                   .WithMany(cc => cc.UserLikeContents)
                   .HasForeignKey(cc => cc.ContentId);

            builder.HasOne(c => c.User)
                   .WithMany(cc => cc.UserLikeContents)
                   .HasForeignKey(cc => cc.UserId);

            builder.Property(cc => cc.ContentId).HasColumnName("CONTENT_ID");
            builder.Property(cc => cc.UserId).HasColumnName("USER_ID");
            
        }
    }
}
