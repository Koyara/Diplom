using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Data.Migrations
{
    /// <inheritdoc />
    public partial class perforr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Performer",
                columns: table => new
                {
                    PerformerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerformerTypeID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainGenreID = table.Column<int>(type: "int", nullable: true),
                    SecondaryGenreID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Performer", x => x.PerformerID);
                    table.ForeignKey(
                        name: "FK_Performer_Genre_MainGenreID",
                        column: x => x.MainGenreID,
                        principalTable: "Genre",
                        principalColumn: "GenreID");
                    table.ForeignKey(
                        name: "FK_Performer_Genre_SecondaryGenreID",
                        column: x => x.SecondaryGenreID,
                        principalTable: "Genre",
                        principalColumn: "GenreID");
                    table.ForeignKey(
                        name: "FK_Performer_PerformerType_PerformerTypeID",
                        column: x => x.PerformerTypeID,
                        principalTable: "PerformerType",
                        principalColumn: "PerformerTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Performer_MainGenreID",
                table: "Performer",
                column: "MainGenreID");

            migrationBuilder.CreateIndex(
                name: "IX_Performer_PerformerTypeID",
                table: "Performer",
                column: "PerformerTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Performer_SecondaryGenreID",
                table: "Performer",
                column: "SecondaryGenreID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Performer");
        }
    }
}
