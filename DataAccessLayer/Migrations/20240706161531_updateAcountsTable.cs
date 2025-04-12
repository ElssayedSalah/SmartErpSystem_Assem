using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class updateAcountsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountSettingsTotalLength",
                schema: "Financial",
                table: "ChartOfAccountSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                schema: "Financial",
                table: "Accounts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrencyRate",
                schema: "Financial",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentBalanceCredit",
                schema: "Financial",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentBalanceDebit",
                schema: "Financial",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "JournalId",
                schema: "Financial",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TransactionsBalanceCredit",
                schema: "Financial",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TransactionsBalanceDebit",
                schema: "Financial",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountSettingsTotalLength",
                schema: "Financial",
                table: "ChartOfAccountSettings");

            migrationBuilder.DropColumn(
                name: "CurrencyRate",
                schema: "Financial",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "CurrentBalanceCredit",
                schema: "Financial",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "CurrentBalanceDebit",
                schema: "Financial",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "JournalId",
                schema: "Financial",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "TransactionsBalanceCredit",
                schema: "Financial",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "TransactionsBalanceDebit",
                schema: "Financial",
                table: "Accounts");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                schema: "Financial",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
