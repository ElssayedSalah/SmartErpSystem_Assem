using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class update_TransactionsEntrySettingTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FirstSide",
                schema: "Financial",
                table: "TransactionsEntrySettingMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstSideSideNaturalId",
                schema: "Financial",
                table: "TransactionsEntrySettingMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondSide",
                schema: "Financial",
                table: "TransactionsEntrySettingMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondSideSideNaturalId",
                schema: "Financial",
                table: "TransactionsEntrySettingMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstSide",
                schema: "Financial",
                table: "TransactionsEntrySettingMaster");

            migrationBuilder.DropColumn(
                name: "FirstSideSideNaturalId",
                schema: "Financial",
                table: "TransactionsEntrySettingMaster");

            migrationBuilder.DropColumn(
                name: "SecondSide",
                schema: "Financial",
                table: "TransactionsEntrySettingMaster");

            migrationBuilder.DropColumn(
                name: "SecondSideSideNaturalId",
                schema: "Financial",
                table: "TransactionsEntrySettingMaster");
        }
    }
}
