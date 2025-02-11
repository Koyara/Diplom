using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Data.Migrations
{
    /// <inheritdoc />
    public partial class rondoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RondoId",
                table: "Country",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Rondo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rondo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Country_RondoId",
                table: "Country",
                column: "RondoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Country_Rondo_RondoId",
                table: "Country",
                column: "RondoId",
                principalTable: "Rondo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Country_Rondo_RondoId",
                table: "Country");

            migrationBuilder.DropTable(
                name: "Rondo");

            migrationBuilder.DropIndex(
                name: "IX_Country_RondoId",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "RondoId",
                table: "Country");
        }
    }
}
