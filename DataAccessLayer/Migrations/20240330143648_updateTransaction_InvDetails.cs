using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class updateTransaction_InvDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                schema: "Transactions",
                table: "Transaction_InvDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountValue",
                schema: "Transactions",
                table: "Transaction_InvDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceAfterDiscount",
                schema: "Transactions",
                table: "Transaction_InvDetails",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountType",
                schema: "Transactions",
                table: "Transaction_InvDetails");

            migrationBuilder.DropColumn(
                name: "DiscountValue",
                schema: "Transactions",
                table: "Transaction_InvDetails");

            migrationBuilder.DropColumn(
                name: "PriceAfterDiscount",
                schema: "Transactions",
                table: "Transaction_InvDetails");
        }
    }
}
