using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class add_EnableTransactionsEntryCreation_Col_ToSystemSettingTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableTransactionsEntryCreation",
                schema: "System",
                table: "SystemSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableTransactionsEntryCreation",
                schema: "System",
                table: "SystemSettings");
        }
    }
}
