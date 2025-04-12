using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class addEntry_MasterAndDetailsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyEntryMaster",
                schema: "Financial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryNumber = table.Column<int>(type: "int", nullable: false),
                    EntryType = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DailyTypeId = table.Column<int>(type: "int", nullable: false),
                    EntryCreationMethod = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CurrencyFactor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsTransfered = table.Column<int>(type: "int", nullable: false),
                    EntryState = table.Column<int>(type: "int", nullable: false),
                    DocType = table.Column<int>(type: "int", nullable: false),
                    DocNumber = table.Column<int>(type: "int", nullable: false),
                    TotalDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinancialPeriodId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyEntryMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyEntryDetails",
                schema: "Financial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    MasterEntryNumber = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    CostCenterId = table.Column<int>(type: "int", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DefaultCurrencyDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DefaultCurrencyCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinancialPeriodId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyEntryDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyEntryDetails_DailyEntryMaster_MasterId",
                        column: x => x.MasterId,
                        principalSchema: "Financial",
                        principalTable: "DailyEntryMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyEntryDetails_MasterId",
                schema: "Financial",
                table: "DailyEntryDetails",
                column: "MasterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyEntryDetails",
                schema: "Financial");

            migrationBuilder.DropTable(
                name: "DailyEntryMaster",
                schema: "Financial");
        }
    }
}
