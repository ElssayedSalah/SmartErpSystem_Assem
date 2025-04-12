using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class addNewColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CountDateFrom",
                schema: "Transactions",
                table: "Transaction_InvMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CountDateTo",
                schema: "Transactions",
                table: "Transaction_InvMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeficitAccountId",
                schema: "Transactions",
                table: "Transaction_InvMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StoreCountId",
                schema: "Transactions",
                table: "Transaction_InvMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "StoreDeficitValue",
                schema: "Transactions",
                table: "Transaction_InvMaster",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "StoreSurplusValue",
                schema: "Transactions",
                table: "Transaction_InvMaster",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SurplusAccountId",
                schema: "Transactions",
                table: "Transaction_InvMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountDateFrom",
                schema: "Transactions",
                table: "Transaction_InvMaster");

            migrationBuilder.DropColumn(
                name: "CountDateTo",
                schema: "Transactions",
                table: "Transaction_InvMaster");

            migrationBuilder.DropColumn(
                name: "DeficitAccountId",
                schema: "Transactions",
                table: "Transaction_InvMaster");

            migrationBuilder.DropColumn(
                name: "StoreCountId",
                schema: "Transactions",
                table: "Transaction_InvMaster");

            migrationBuilder.DropColumn(
                name: "StoreDeficitValue",
                schema: "Transactions",
                table: "Transaction_InvMaster");

            migrationBuilder.DropColumn(
                name: "StoreSurplusValue",
                schema: "Transactions",
                table: "Transaction_InvMaster");

            migrationBuilder.DropColumn(
                name: "SurplusAccountId",
                schema: "Transactions",
                table: "Transaction_InvMaster");
        }
    }
}
