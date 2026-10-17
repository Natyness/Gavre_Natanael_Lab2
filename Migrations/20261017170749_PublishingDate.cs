using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gavre_Natnael_Lab2.Migrations
{
    /// <inheritdoc />
    public partial class PublishingDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Prince",
                table: "Book",
                type: "decimal(6, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishingDate",
                table: "Book",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishingDate",
                table: "Book");

            migrationBuilder.AlterColumn<decimal>(
                name: "Prince",
                table: "Book",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6, 2)");
        }
    }
}
