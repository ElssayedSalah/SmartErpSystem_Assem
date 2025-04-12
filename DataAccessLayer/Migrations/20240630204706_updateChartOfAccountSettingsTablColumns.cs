using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class updateChartOfAccountSettingsTablColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivationState",
                schema: "Financial",
                table: "ChartOfAccountSettings");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Financial",
                table: "ChartOfAccountSettings");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "Financial",
                table: "ChartOfAccountSettings");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                schema: "Financial",
                table: "ChartOfAccountSettings");

            migrationBuilder.DropColumn(
                name: "CreationUserId",
                schema: "Financial",
                table: "ChartOfAccountSettings");

            migrationBuilder.DropColumn(
                name: "NameAr",
                schema: "Financial",
                table: "ChartOfAccountSettings");

            migrationBuilder.DropColumn(
                name: "NameEn",
                schema: "Financial",
                table: "ChartOfAccountSettings");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Financial",
                table: "ChartOfAccountSettings");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "Financial",
                table: "ChartOfAccountSettings");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                schema: "Financial",
                table: "ChartOfAccountSettings");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                schema: "Financial",
                table: "AccountSetting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                schema: "Financial",
                table: "AccountSetting",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAr",
                schema: "Financial",
                table: "AccountSetting");

            migrationBuilder.DropColumn(
                name: "NameEn",
                schema: "Financial",
                table: "AccountSetting");

            migrationBuilder.AddColumn<bool>(
                name: "ActivationState",
                schema: "Financial",
                table: "ChartOfAccountSettings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "Financial",
                table: "ChartOfAccountSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                schema: "Financial",
                table: "ChartOfAccountSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                schema: "Financial",
                table: "ChartOfAccountSettings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreationUserId",
                schema: "Financial",
                table: "ChartOfAccountSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                schema: "Financial",
                table: "ChartOfAccountSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                schema: "Financial",
                table: "ChartOfAccountSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Financial",
                table: "ChartOfAccountSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "Financial",
                table: "ChartOfAccountSettings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUserId",
                schema: "Financial",
                table: "ChartOfAccountSettings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
