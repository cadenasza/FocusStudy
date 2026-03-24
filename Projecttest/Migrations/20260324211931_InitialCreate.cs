using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projecttest.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TargetMinutes",
                table: "StudySessions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Badge",
                table: "StudySessions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Badge",
                table: "StudySessions");

            migrationBuilder.DropColumn(
                name: "TargetMinutes",
                table: "StudySessions");
        }
    }
}
