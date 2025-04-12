using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class add_IsFirstSideTaxbleAndIsSecondSideTaxble_Cols_TransactionsEntrySettingTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFirstSideTaxble",
                schema: "Financial",
                table: "TransactionsEntrySettingMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSecondSideTaxble",
                schema: "Financial",
                table: "TransactionsEntrySettingMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFirstSideTaxble",
                schema: "Financial",
                table: "TransactionsEntrySettingMaster");

            migrationBuilder.DropColumn(
                name: "IsSecondSideTaxble",
                schema: "Financial",
                table: "TransactionsEntrySettingMaster");
        }
    }
}
