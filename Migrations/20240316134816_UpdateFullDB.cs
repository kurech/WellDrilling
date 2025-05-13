using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace robert.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFullDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_WorkSchedules_WorkScheduleId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSchedules_Wells_WellId",
                table: "WorkSchedules");

            migrationBuilder.DropIndex(
                name: "IX_WorkSchedules_WellId",
                table: "WorkSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Users_WorkScheduleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WellId",
                table: "WorkSchedules");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "WorkScheduleId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "WorkType",
                table: "WorkOrders",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "WorkScheduleId",
                table: "WorkOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WellUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WellId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WorkScheduleId = table.Column<int>(type: "int", nullable: false),
                    IsReady = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WellUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WellUsers_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WellUsers_WorkSchedules_WorkScheduleId",
                        column: x => x.WorkScheduleId,
                        principalTable: "WorkSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_WorkScheduleId",
                table: "WorkOrders",
                column: "WorkScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_WellUsers_UserId",
                table: "WellUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WellUsers_WellId",
                table: "WellUsers",
                column: "WellId");

            migrationBuilder.CreateIndex(
                name: "IX_WellUsers_WorkScheduleId",
                table: "WellUsers",
                column: "WorkScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_WorkSchedules_WorkScheduleId",
                table: "WorkOrders",
                column: "WorkScheduleId",
                principalTable: "WorkSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_WorkSchedules_WorkScheduleId",
                table: "WorkOrders");

            migrationBuilder.DropTable(
                name: "WellUsers");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_WorkScheduleId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "WorkScheduleId",
                table: "WorkOrders");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "WorkOrders",
                newName: "WorkType");

            migrationBuilder.AddColumn<int>(
                name: "WellId",
                table: "WorkSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "WorkOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "WorkScheduleId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedules_WellId",
                table: "WorkSchedules",
                column: "WellId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_WorkScheduleId",
                table: "Users",
                column: "WorkScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_WorkSchedules_WorkScheduleId",
                table: "Users",
                column: "WorkScheduleId",
                principalTable: "WorkSchedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSchedules_Wells_WellId",
                table: "WorkSchedules",
                column: "WellId",
                principalTable: "Wells",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
