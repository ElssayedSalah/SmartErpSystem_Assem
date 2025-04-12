using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class addAccountsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountOpenBalance",
                schema: "Financial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    OpenBalanceDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpenBalanceCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionsBalanceDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionsBalanceCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentBalanceDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentBalanceCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinancialPeriodId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountOpenBalance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "Financial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    AccountCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpenDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountNatureId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    ConnectedToCostCenter = table.Column<bool>(type: "bit", nullable: false),
                    LastLevelInTree = table.Column<bool>(type: "bit", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    PostTo = table.Column<int>(type: "int", nullable: true),
                    PostType = table.Column<int>(type: "int", nullable: true),
                    OpenBalanceCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpenBalanceDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountOpenBalance",
                schema: "Financial");

            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "Financial");
        }
    }
}
