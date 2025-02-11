using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Data.Migrations
{
    /// <inheritdoc />
    public partial class contr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "BirthDate",
                table: "Contributor",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Contributor",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMale",
                table: "Contributor",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contributor_CountryCode",
                table: "Contributor",
                column: "CountryCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributor_Country_CountryCode",
                table: "Contributor",
                column: "CountryCode",
                principalTable: "Country",
                principalColumn: "CountryCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributor_Country_CountryCode",
                table: "Contributor");

            migrationBuilder.DropIndex(
                name: "IX_Contributor_CountryCode",
                table: "Contributor");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Contributor");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Contributor");

            migrationBuilder.DropColumn(
                name: "IsMale",
                table: "Contributor");
        }
    }
}
