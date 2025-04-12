using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class add_CashTransaction_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CashTransaction",
                schema: "Financial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    EntryNumber = table.Column<int>(type: "int", nullable: false),
                    EntryId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CurrencyFactor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostCenterId = table.Column<int>(type: "int", nullable: false),
                    DiscountAccountId = table.Column<int>(type: "int", nullable: false),
                    FirstSideTypeId = table.Column<int>(type: "int", nullable: false),
                    SecondSideTypeId = table.Column<int>(type: "int", nullable: false),
                    FirstSideId = table.Column<int>(type: "int", nullable: false),
                    SecondSideId = table.Column<int>(type: "int", nullable: false),
                    FirstSideAccountId = table.Column<int>(type: "int", nullable: false),
                    SecondSideAccountId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    DocTypeId = table.Column<int>(type: "int", nullable: true),
                    DocDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinancialPeriodId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashTransaction", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashTransaction",
                schema: "Financial");
        }
    }
}
