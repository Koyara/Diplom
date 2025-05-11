using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class trackforeignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Track_Performer_MainGuestPerformerID",
                table: "Track");

            migrationBuilder.DropIndex(
                name: "IX_Track_MainGuestPerformerID",
                table: "Track");

            migrationBuilder.DropColumn(
                name: "MainGuestPerformerID",
                table: "Track");

            migrationBuilder.CreateIndex(
                name: "IX_Track_GuestID",
                table: "Track",
                column: "GuestID");

            migrationBuilder.AddForeignKey(
                name: "FK_Track_Performer_GuestID",
                table: "Track",
                column: "GuestID",
                principalTable: "Performer",
                principalColumn: "PerformerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Track_Performer_GuestID",
                table: "Track");

            migrationBuilder.DropIndex(
                name: "IX_Track_GuestID",
                table: "Track");

            migrationBuilder.AddColumn<int>(
                name: "MainGuestPerformerID",
                table: "Track",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Track_MainGuestPerformerID",
                table: "Track",
                column: "MainGuestPerformerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Track_Performer_MainGuestPerformerID",
                table: "Track",
                column: "MainGuestPerformerID",
                principalTable: "Performer",
                principalColumn: "PerformerID");
        }
    }
}
