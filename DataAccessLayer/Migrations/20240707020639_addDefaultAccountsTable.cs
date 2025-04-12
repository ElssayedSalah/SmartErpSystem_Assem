using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class addDefaultAccountsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DefaultAccounts",
                schema: "Financial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    GroupNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNameId = table.Column<int>(type: "int", nullable: false),
                    AccountNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultAccounts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefaultAccounts",
                schema: "Financial");
        }
    }
}
