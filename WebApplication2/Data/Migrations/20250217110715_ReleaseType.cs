using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReleaseType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReleaseTypeID",
                table: "Release",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReleaseType",
                columns: table => new
                {
                    ReleaseTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReleaseTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseType", x => x.ReleaseTypeID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Release_ReleaseTypeID",
                table: "Release",
                column: "ReleaseTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Release_ReleaseType_ReleaseTypeID",
                table: "Release",
                column: "ReleaseTypeID",
                principalTable: "ReleaseType",
                principalColumn: "ReleaseTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Release_ReleaseType_ReleaseTypeID",
                table: "Release");

            migrationBuilder.DropTable(
                name: "ReleaseType");

            migrationBuilder.DropIndex(
                name: "IX_Release_ReleaseTypeID",
                table: "Release");

            migrationBuilder.DropColumn(
                name: "ReleaseTypeID",
                table: "Release");
        }
    }
}
