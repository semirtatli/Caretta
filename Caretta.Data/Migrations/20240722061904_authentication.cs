using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Caretta.Data.Migrations
{
    public partial class authentication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PASSWORD",
                table: "Users");

            migrationBuilder.AddColumn<byte[]>(
                name: "PASSWORDHASH",
                table: "Users",
                type: "varbinary(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PASSWORDSALT",
                table: "Users",
                type: "varbinary(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PASSWORDHASH",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PASSWORDSALT",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "PASSWORD",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
