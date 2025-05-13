using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace robert.Migrations
{
    /// <inheritdoc />
    public partial class AddedAllowNullForWorkScheduleId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_WorkSchedules_WorkScheduleId",
                table: "WorkOrders");

            migrationBuilder.AlterColumn<int>(
                name: "WorkScheduleId",
                table: "WorkOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_WorkSchedules_WorkScheduleId",
                table: "WorkOrders",
                column: "WorkScheduleId",
                principalTable: "WorkSchedules",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_WorkSchedules_WorkScheduleId",
                table: "WorkOrders");

            migrationBuilder.AlterColumn<int>(
                name: "WorkScheduleId",
                table: "WorkOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_WorkSchedules_WorkScheduleId",
                table: "WorkOrders",
                column: "WorkScheduleId",
                principalTable: "WorkSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
