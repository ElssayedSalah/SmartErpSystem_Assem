using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class updateTransaction_InvMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoastCenterId",
                schema: "Transactions",
                table: "Transaction_InvMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                schema: "Transactions",
                table: "Transaction_InvMaster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EntryNumber",
                schema: "Transactions",
                table: "Transaction_InvMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InvoiceTotal",
                schema: "Transactions",
                table: "Transaction_InvMaster",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoastCenterId",
                schema: "Transactions",
                table: "Transaction_InvMaster");

            migrationBuilder.DropColumn(
                name: "DueDate",
                schema: "Transactions",
                table: "Transaction_InvMaster");

            migrationBuilder.DropColumn(
                name: "EntryNumber",
                schema: "Transactions",
                table: "Transaction_InvMaster");

            migrationBuilder.DropColumn(
                name: "InvoiceTotal",
                schema: "Transactions",
                table: "Transaction_InvMaster");
        }
    }
}
