using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace robert.Migrations
{
    /// <inheritdoc />
    public partial class AddedNameFieldToWell : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Wells",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Wells");
        }
    }
}
