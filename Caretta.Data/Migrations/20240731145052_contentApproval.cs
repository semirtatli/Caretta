using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Caretta.Data.Migrations
{
    public partial class contentApproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatus",
                table: "Contents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Contents",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Contents");
        }
    }
}
