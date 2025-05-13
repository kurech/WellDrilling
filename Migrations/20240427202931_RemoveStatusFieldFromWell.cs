using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace robert.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStatusFieldFromWell : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Wells");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Wells",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
