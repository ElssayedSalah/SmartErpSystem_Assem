using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class updateAddTaxRatToCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activate_VAT_Tax",
                schema: "System",
                table: "Companys",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "VAT_Tax_Rate",
                schema: "System",
                table: "Companys",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activate_VAT_Tax",
                schema: "System",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "VAT_Tax_Rate",
                schema: "System",
                table: "Companys");
        }
    }
}
