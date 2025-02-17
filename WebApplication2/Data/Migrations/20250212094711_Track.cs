using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Track : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Track",
                columns: table => new
                {
                    TrackID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Length = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsSong = table.Column<bool>(type: "bit", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuestID = table.Column<int>(type: "int", nullable: true),
                    MainGuestPerformerID = table.Column<int>(type: "int", nullable: true),
                    SecondGuestID = table.Column<int>(type: "int", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ScaleID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Track", x => x.TrackID);
                    table.ForeignKey(
                        name: "FK_Track_Language_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Language",
                        principalColumn: "LanguageCode");
                    table.ForeignKey(
                        name: "FK_Track_Performer_MainGuestPerformerID",
                        column: x => x.MainGuestPerformerID,
                        principalTable: "Performer",
                        principalColumn: "PerformerID");
                    table.ForeignKey(
                        name: "FK_Track_Performer_SecondGuestID",
                        column: x => x.SecondGuestID,
                        principalTable: "Performer",
                        principalColumn: "PerformerID");
                    table.ForeignKey(
                        name: "FK_Track_Scale_ScaleID",
                        column: x => x.ScaleID,
                        principalTable: "Scale",
                        principalColumn: "ScaleId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Track_LanguageCode",
                table: "Track",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_Track_MainGuestPerformerID",
                table: "Track",
                column: "MainGuestPerformerID");

            migrationBuilder.CreateIndex(
                name: "IX_Track_ScaleID",
                table: "Track",
                column: "ScaleID");

            migrationBuilder.CreateIndex(
                name: "IX_Track_SecondGuestID",
                table: "Track",
                column: "SecondGuestID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Track");
        }
    }
}
