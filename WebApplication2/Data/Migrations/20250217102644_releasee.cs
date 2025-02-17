using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Data.Migrations
{
    /// <inheritdoc />
    public partial class releasee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Release",
                columns: table => new
                {
                    ReleaseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MainGenreCode = table.Column<int>(type: "int", nullable: true),
                    MainGenreGenreID = table.Column<int>(type: "int", nullable: true),
                    SecondGenreCode = table.Column<int>(type: "int", nullable: true),
                    SecondGenreGenreID = table.Column<int>(type: "int", nullable: true),
                    ReleaseCover = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Release", x => x.ReleaseID);
                    table.ForeignKey(
                        name: "FK_Release_Genre_MainGenreGenreID",
                        column: x => x.MainGenreGenreID,
                        principalTable: "Genre",
                        principalColumn: "GenreID");
                    table.ForeignKey(
                        name: "FK_Release_Genre_SecondGenreGenreID",
                        column: x => x.SecondGenreGenreID,
                        principalTable: "Genre",
                        principalColumn: "GenreID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Release_MainGenreGenreID",
                table: "Release",
                column: "MainGenreGenreID");

            migrationBuilder.CreateIndex(
                name: "IX_Release_SecondGenreGenreID",
                table: "Release",
                column: "SecondGenreGenreID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Release");
        }
    }
}
