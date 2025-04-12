using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class addActualQuntityAndDifferenceQuntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ActualQuntity",
                schema: "Transactions",
                table: "Transaction_InvDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DifferenceQuntity",
                schema: "Transactions",
                table: "Transaction_InvDetails",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualQuntity",
                schema: "Transactions",
                table: "Transaction_InvDetails");

            migrationBuilder.DropColumn(
                name: "DifferenceQuntity",
                schema: "Transactions",
                table: "Transaction_InvDetails");
        }
    }
}
