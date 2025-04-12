using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class updateTreasuryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                schema: "Financial",
                table: "Treasurys");

            migrationBuilder.DropColumn(
                name: "Address",
                schema: "Financial",
                table: "Treasurys");

            migrationBuilder.RenameColumn(
                name: "BranchName",
                schema: "Financial",
                table: "Treasurys",
                newName: "BranchId");

            migrationBuilder.RenameColumn(
                name: "BankId",
                schema: "Financial",
                table: "TreasuryOpenBalance",
                newName: "TreasuryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BranchId",
                schema: "Financial",
                table: "Treasurys",
                newName: "BranchName");

            migrationBuilder.RenameColumn(
                name: "TreasuryId",
                schema: "Financial",
                table: "TreasuryOpenBalance",
                newName: "BankId");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                schema: "Financial",
                table: "Treasurys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "Financial",
                table: "Treasurys",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
