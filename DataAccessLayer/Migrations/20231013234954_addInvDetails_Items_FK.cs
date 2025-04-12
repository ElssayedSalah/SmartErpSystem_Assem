using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class addInvDetails_Items_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quntity",
                schema: "Transactions",
                table: "Transaction_InvDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                schema: "Transactions",
                table: "Transaction_InvDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_InvDetails_ItemId",
                schema: "Transactions",
                table: "Transaction_InvDetails",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_InvDetails_Items_ItemId",
                schema: "Transactions",
                table: "Transaction_InvDetails",
                column: "ItemId",
                principalSchema: "Inventory",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_InvDetails_Items_ItemId",
                schema: "Transactions",
                table: "Transaction_InvDetails");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_InvDetails_ItemId",
                schema: "Transactions",
                table: "Transaction_InvDetails");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quntity",
                schema: "Transactions",
                table: "Transaction_InvDetails",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                schema: "Transactions",
                table: "Transaction_InvDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
