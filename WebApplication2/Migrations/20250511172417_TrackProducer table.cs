using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class TrackProducertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrackProducer",
                columns: table => new
                {
                    TrackProducerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackID = table.Column<int>(type: "int", nullable: false),
                    ProducerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackProducer", x => x.TrackProducerID);
                    table.ForeignKey(
                        name: "FK_TrackProducer_Performer_ProducerID",
                        column: x => x.ProducerID,
                        principalTable: "Performer",
                        principalColumn: "PerformerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackProducer_Track_TrackID",
                        column: x => x.TrackID,
                        principalTable: "Track",
                        principalColumn: "TrackID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackProducer_ProducerID",
                table: "TrackProducer",
                column: "ProducerID");

            migrationBuilder.CreateIndex(
                name: "IX_TrackProducer_TrackID",
                table: "TrackProducer",
                column: "TrackID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackProducer");
        }
    }
}
