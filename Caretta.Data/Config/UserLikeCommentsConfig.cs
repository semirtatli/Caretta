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
    public class UserLikeCommentsConfig : IEntityTypeConfiguration<UserLikeComments>
    {
        public void Configure(EntityTypeBuilder<UserLikeComments> builder)
        {
            //for associative tables
            builder.HasKey(cc => new { cc.CommentId, cc.UserId });


            builder.HasOne(c => c.Comment)
                   .WithMany(cc => cc.UserLikeComments)
                   .HasForeignKey(cc => cc.CommentId);

            builder.HasOne(c => c.User)
                   .WithMany(cc => cc.UserLikeComments)
                   .HasForeignKey(cc => cc.UserId);

            builder.Property(cc => cc.CommentId).HasColumnName("COMMENT_ID");
            builder.Property(cc => cc.UserId).HasColumnName("USER_ID");

        }
    }
}
