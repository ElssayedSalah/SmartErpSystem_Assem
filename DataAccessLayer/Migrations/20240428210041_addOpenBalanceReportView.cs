using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class addOpenBalanceReportView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE VIEW OpenBalanceReportView AS SELECT Transactions.Transaction_InvMaster.StoreId, Transactions.Transaction_InvMaster.BranchId, Transactions.Transaction_InvDetails.ItemId, Transactions.Transaction_InvDetails.GroupId,Transactions.Transaction_InvDetails.Quntity, Transactions.Transaction_InvDetails.UnitId, Transactions.Transaction_InvMaster.FinancialPeriodId, Transactions.Transaction_InvMaster.CompanyId,Transactions.Transaction_InvMaster.DocTypeId, Inventory.Items.NameAr as ItemNameAr, Inventory.Items.NameEn as ItemNameEn, Inventory.Units.NameAr AS UnitNameAr, Inventory.Units.NameEn AS UnitNameEn,Inventory.Branches.NameAr AS BrancheNameAr, Inventory.Branches.NameEn AS BrancheNameEn, Inventory.ItemGroups.NameAr AS ItemGroupNameAr, Inventory.ItemGroups.NameEn AS ItemGroupNameEn FROM Transactions.Transaction_InvMaster INNER JOIN Transactions.Transaction_InvDetails ON Transactions.Transaction_InvMaster.Id = Transactions.Transaction_InvDetails.MasterId INNER JOIN Inventory.Items ON Transactions.Transaction_InvDetails.ItemId = Inventory.Items.Id INNER JOIN Inventory.Units ON Transactions.Transaction_InvDetails.UnitId = Inventory.Units.Id INNER JOIN Inventory.ItemGroups ON Transactions.Transaction_InvDetails.GroupId = Inventory.ItemGroups.Id INNER JOIN Inventory.Branches ON Transactions.Transaction_InvMaster.BranchId = Inventory.Branches.Id INNER JOIN Inventory.Stores ON Transactions.Transaction_InvMaster.StoreId = Inventory.Stores.Id WHERE Transactions.Transaction_InvMaster.DocTypeId = 1;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
