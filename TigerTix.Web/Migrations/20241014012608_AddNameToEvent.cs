using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TigerTix.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "dateYear",
                table: "Events",
                newName: "DateYear");

            migrationBuilder.RenameColumn(
                name: "dateMonth",
                table: "Events",
                newName: "DateMonth");

            migrationBuilder.RenameColumn(
                name: "dateDay",
                table: "Events",
                newName: "DateDay");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "DateYear",
                table: "Events",
                newName: "dateYear");

            migrationBuilder.RenameColumn(
                name: "DateMonth",
                table: "Events",
                newName: "dateMonth");

            migrationBuilder.RenameColumn(
                name: "DateDay",
                table: "Events",
                newName: "dateDay");
        }
    }
}
