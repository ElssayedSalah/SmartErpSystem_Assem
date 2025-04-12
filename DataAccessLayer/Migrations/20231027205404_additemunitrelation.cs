using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class additemunitrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateIndex(
            //    name: "IX_Items_DefaultUnit",
            //    schema: "Inventory",
            //    table: "Items",
            //    column: "DefaultUnit",
            //    unique: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Items_Units_DefaultUnit",
            //    schema: "Inventory",
            //    table: "Items",
            //    column: "DefaultUnit",
            //    principalSchema: "Inventory",
            //    principalTable: "Units",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Items_Units_DefaultUnit",
            //    schema: "Inventory",
            //    table: "Items");

            //migrationBuilder.DropIndex(
            //    name: "IX_Items_DefaultUnit",
            //    schema: "Inventory",
            //    table: "Items");
        }
    }
}
