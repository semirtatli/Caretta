using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Caretta.Data.Migrations
{
    public partial class readdUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Contents_Content_ID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentCategories_Categories_Category_ID",
                table: "ContentCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentCategories_Contents_Content_ID",
                table: "ContentCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_Role_ID",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_User_ID",
                table: "UserRoles");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "USERNAME");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Users",
                newName: "SURNAME");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "NAME");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "EMAIL");

            migrationBuilder.RenameColumn(
                name: "Role_ID",
                table: "UserRoles",
                newName: "ROLE_ID");

            migrationBuilder.RenameColumn(
                name: "User_ID",
                table: "UserRoles",
                newName: "USER_ID");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_Role_ID",
                table: "UserRoles",
                newName: "IX_UserRoles_ROLE_ID");

            migrationBuilder.RenameColumn(
                name: "RoleType",
                table: "Roles",
                newName: "ROLETYPE");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Contents",
                newName: "TITLE");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Contents",
                newName: "BODY");

            migrationBuilder.RenameColumn(
                name: "Category_ID",
                table: "ContentCategories",
                newName: "CATEGORY_ID");

            migrationBuilder.RenameColumn(
                name: "Content_ID",
                table: "ContentCategories",
                newName: "CONTENT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_ContentCategories_Category_ID",
                table: "ContentCategories",
                newName: "IX_ContentCategories_CATEGORY_ID");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Comments",
                newName: "TEXT");

            migrationBuilder.RenameColumn(
                name: "Content_ID",
                table: "Comments",
                newName: "CONTENT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_Content_ID",
                table: "Comments",
                newName: "IX_Comments_CONTENT_ID");

            migrationBuilder.RenameColumn(
                name: "Category_Name",
                table: "Categories",
                newName: "CATEGORY_NAME");

            migrationBuilder.AddColumn<string>(
                name: "PASSWORD",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TC",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Contents_CONTENT_ID",
                table: "Comments",
                column: "CONTENT_ID",
                principalTable: "Contents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentCategories_Categories_CATEGORY_ID",
                table: "ContentCategories",
                column: "CATEGORY_ID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentCategories_Contents_CONTENT_ID",
                table: "ContentCategories",
                column: "CONTENT_ID",
                principalTable: "Contents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_ROLE_ID",
                table: "UserRoles",
                column: "ROLE_ID",
                principalTable: "Roles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_USER_ID",
                table: "UserRoles",
                column: "USER_ID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Contents_CONTENT_ID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentCategories_Categories_CATEGORY_ID",
                table: "ContentCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentCategories_Contents_CONTENT_ID",
                table: "ContentCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_ROLE_ID",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_USER_ID",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "PASSWORD",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TC",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "USERNAME",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "SURNAME",
                table: "Users",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "NAME",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "EMAIL",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "ROLE_ID",
                table: "UserRoles",
                newName: "Role_ID");

            migrationBuilder.RenameColumn(
                name: "USER_ID",
                table: "UserRoles",
                newName: "User_ID");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_ROLE_ID",
                table: "UserRoles",
                newName: "IX_UserRoles_Role_ID");

            migrationBuilder.RenameColumn(
                name: "ROLETYPE",
                table: "Roles",
                newName: "RoleType");

            migrationBuilder.RenameColumn(
                name: "TITLE",
                table: "Contents",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "BODY",
                table: "Contents",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "CATEGORY_ID",
                table: "ContentCategories",
                newName: "Category_ID");

            migrationBuilder.RenameColumn(
                name: "CONTENT_ID",
                table: "ContentCategories",
                newName: "Content_ID");

            migrationBuilder.RenameIndex(
                name: "IX_ContentCategories_CATEGORY_ID",
                table: "ContentCategories",
                newName: "IX_ContentCategories_Category_ID");

            migrationBuilder.RenameColumn(
                name: "TEXT",
                table: "Comments",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "CONTENT_ID",
                table: "Comments",
                newName: "Content_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CONTENT_ID",
                table: "Comments",
                newName: "IX_Comments_Content_ID");

            migrationBuilder.RenameColumn(
                name: "CATEGORY_NAME",
                table: "Categories",
                newName: "Category_Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Contents_Content_ID",
                table: "Comments",
                column: "Content_ID",
                principalTable: "Contents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentCategories_Categories_Category_ID",
                table: "ContentCategories",
                column: "Category_ID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentCategories_Contents_Content_ID",
                table: "ContentCategories",
                column: "Content_ID",
                principalTable: "Contents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_Role_ID",
                table: "UserRoles",
                column: "Role_ID",
                principalTable: "Roles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_User_ID",
                table: "UserRoles",
                column: "User_ID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
