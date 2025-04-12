using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class add_TransactionsEntrySettingTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionsEntrySettingDetails",
                schema: "Financial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    SideTypeId = table.Column<int>(type: "int", nullable: false),
                    SideId = table.Column<int>(type: "int", nullable: false),
                    SideNaturalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionsEntrySettingDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionsEntrySettingMaster",
                schema: "Financial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocTypeId = table.Column<int>(type: "int", nullable: false),
                    DailyTypeId = table.Column<int>(type: "int", nullable: false),
                    TransferState = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionsEntrySettingMaster", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionsEntrySettingDetails",
                schema: "Financial");

            migrationBuilder.DropTable(
                name: "TransactionsEntrySettingMaster",
                schema: "Financial");
        }
    }
}
