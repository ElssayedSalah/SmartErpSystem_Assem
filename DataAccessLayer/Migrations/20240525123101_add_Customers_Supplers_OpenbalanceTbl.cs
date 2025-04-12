using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class add_Customers_Supplers_OpenbalanceTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerOpenBalance",
                schema: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OpeningBalanceDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpeningBalanceCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    FinancialPeriodId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    EntryNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOpenBalance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplerOpenBalance",
                schema: "Purchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplerId = table.Column<int>(type: "int", nullable: false),
                    OpeningBalanceDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpeningBalanceCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    FinancialPeriodId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    EntryNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplerOpenBalance", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerOpenBalance",
                schema: "Sales");

            migrationBuilder.DropTable(
                name: "SupplerOpenBalance",
                schema: "Purchases");
        }
    }
}
