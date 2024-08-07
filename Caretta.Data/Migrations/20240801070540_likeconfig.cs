using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Caretta.Data.Migrations
{
    public partial class likeconfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLikeContents_Contents_USER_ID",
                table: "UserLikeContents");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikeContents_Users_CONTENT_ID",
                table: "UserLikeContents");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikeContents_Contents_CONTENT_ID",
                table: "UserLikeContents",
                column: "CONTENT_ID",
                principalTable: "Contents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikeContents_Users_USER_ID",
                table: "UserLikeContents",
                column: "USER_ID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLikeContents_Contents_CONTENT_ID",
                table: "UserLikeContents");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikeContents_Users_USER_ID",
                table: "UserLikeContents");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikeContents_Contents_USER_ID",
                table: "UserLikeContents",
                column: "USER_ID",
                principalTable: "Contents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikeContents_Users_CONTENT_ID",
                table: "UserLikeContents",
                column: "CONTENT_ID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
