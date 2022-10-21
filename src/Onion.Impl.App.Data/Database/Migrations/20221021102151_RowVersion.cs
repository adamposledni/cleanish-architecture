using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onion.Impl.App.Data.Database.Migrations
{
    public partial class RowVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "User",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "TodoList",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "TodoItem",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "RefreshToken",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "TodoList");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "TodoItem");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "RefreshToken");
        }
    }
}
