using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class add_EntryId_Col_ToTransactionsTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntryId",
                schema: "Financial",
                table: "Treasurys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntryId",
                schema: "Financial",
                table: "TreasuryOpenBalance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntryId",
                schema: "Transactions",
                table: "Transaction_InvMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntryId",
                schema: "Purchases",
                table: "Supplers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntryId",
                schema: "Purchases",
                table: "SupplerOpenBalance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntryId",
                schema: "Sales",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntryId",
                schema: "Sales",
                table: "CustomerOpenBalance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntryId",
                schema: "Financial",
                table: "Banks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntryId",
                schema: "Financial",
                table: "BankOpenBalance",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntryId",
                schema: "Financial",
                table: "Treasurys");

            migrationBuilder.DropColumn(
                name: "EntryId",
                schema: "Financial",
                table: "TreasuryOpenBalance");

            migrationBuilder.DropColumn(
                name: "EntryId",
                schema: "Transactions",
                table: "Transaction_InvMaster");

            migrationBuilder.DropColumn(
                name: "EntryId",
                schema: "Purchases",
                table: "Supplers");

            migrationBuilder.DropColumn(
                name: "EntryId",
                schema: "Purchases",
                table: "SupplerOpenBalance");

            migrationBuilder.DropColumn(
                name: "EntryId",
                schema: "Sales",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "EntryId",
                schema: "Sales",
                table: "CustomerOpenBalance");

            migrationBuilder.DropColumn(
                name: "EntryId",
                schema: "Financial",
                table: "Banks");

            migrationBuilder.DropColumn(
                name: "EntryId",
                schema: "Financial",
                table: "BankOpenBalance");
        }
    }
}
