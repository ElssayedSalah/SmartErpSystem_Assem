using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class updatecomany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Active",
                schema: "System",
                table: "Companys",
                newName: "ActivationState");

            migrationBuilder.AlterColumn<string>(
                name: "NameAr",
                schema: "System",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "System",
                table: "Companys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                schema: "System",
                table: "Companys",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                schema: "System",
                table: "Companys",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreationUserId",
                schema: "System",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "System",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "System",
                table: "Companys",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUserId",
                schema: "System",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                schema: "System",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "System",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                schema: "System",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "CreationUserId",
                schema: "System",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "System",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "System",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                schema: "System",
                table: "Companys");

            migrationBuilder.RenameColumn(
                name: "ActivationState",
                schema: "System",
                table: "Companys",
                newName: "Active");

            migrationBuilder.AlterColumn<string>(
                name: "NameAr",
                schema: "System",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
