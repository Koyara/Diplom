using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class tryto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Track_Performer_GuestID",
                table: "Track");

            migrationBuilder.AddColumn<int>(
                name: "MainGuestPerformerID",
                table: "Track",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Track_MainGuestPerformerID",
                table: "Track",
                column: "MainGuestPerformerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Track_Performer_MainGuestPerformerID",
                table: "Track",
                column: "MainGuestPerformerID",
                principalTable: "Performer",
                principalColumn: "PerformerID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddForeignKey(
                name: "FK_Track_Performer_GuestID",
                table: "Track",
                column: "GuestID",
                principalTable: "Performer",
                principalColumn: "PerformerID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
