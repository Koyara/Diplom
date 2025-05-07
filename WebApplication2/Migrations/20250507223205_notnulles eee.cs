using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class notnulleseee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Track_Performer_MainGuestPerformerID",
                table: "Track");

            migrationBuilder.AlterColumn<int>(
                name: "MainGuestPerformerID",
                table: "Track",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GuestID",
                table: "Track",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Track_Performer_MainGuestPerformerID",
                table: "Track",
                column: "MainGuestPerformerID",
                principalTable: "Performer",
                principalColumn: "PerformerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Track_Performer_MainGuestPerformerID",
                table: "Track");

            migrationBuilder.AlterColumn<int>(
                name: "MainGuestPerformerID",
                table: "Track",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GuestID",
                table: "Track",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Track_Performer_MainGuestPerformerID",
                table: "Track",
                column: "MainGuestPerformerID",
                principalTable: "Performer",
                principalColumn: "PerformerID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
