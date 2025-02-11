using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Data.Migrations
{
    /// <inheritdoc />
    public partial class perrfff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Performer_PerformerType_PerformerTypeID",
                table: "Performer");

            migrationBuilder.AlterColumn<int>(
                name: "PerformerTypeID",
                table: "Performer",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Performer_PerformerType_PerformerTypeID",
                table: "Performer",
                column: "PerformerTypeID",
                principalTable: "PerformerType",
                principalColumn: "PerformerTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Performer_PerformerType_PerformerTypeID",
                table: "Performer");

            migrationBuilder.AlterColumn<int>(
                name: "PerformerTypeID",
                table: "Performer",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Performer_PerformerType_PerformerTypeID",
                table: "Performer",
                column: "PerformerTypeID",
                principalTable: "PerformerType",
                principalColumn: "PerformerTypeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
