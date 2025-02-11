using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Data.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contributor_description",
                table: "Contributor");

            migrationBuilder.RenameColumn(
                name: "contributor_name",
                table: "Contributor",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "contributor_id",
                table: "Contributor",
                newName: "ContributorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Contributor",
                newName: "contributor_name");

            migrationBuilder.RenameColumn(
                name: "ContributorID",
                table: "Contributor",
                newName: "contributor_id");

            migrationBuilder.AddColumn<string>(
                name: "contributor_description",
                table: "Contributor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
