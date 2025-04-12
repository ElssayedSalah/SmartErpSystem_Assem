using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class addItemSalesAnaysisReportView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE VIEW ItemSalesAnaysisReportView AS SELECT Transactions.Transaction_InvMaster.StoreId,Transactions.Transaction_InvMaster.BranchId, Transactions.Transaction_InvDetails.ItemId,Transactions.Transaction_InvMaster.CustomerId,Sales.Customers.NameAr as CustomerNameAr, Sales.Customers.NameEn as CustomerNameEn,Transactions.Transaction_InvDetails.GroupId,Transactions.Transaction_InvDetails.Quntity,Transactions.Transaction_InvDetails.UnitId,Transactions.Transaction_InvMaster.FinancialPeriodId, Transactions.Transaction_InvMaster.CompanyId, Transactions.Transaction_InvMaster.DocTypeId, Documents.DocSign,Transactions.Transaction_InvMaster.Code AS DocNumber, Documents.NameAr AS DocNameAr, Documents.NameEn AS DocNameEn,Inventory.Items.NameAr as ItemNameAr, Inventory.Items.NameEn as ItemNameEn, Inventory.Units.NameAr AS UnitNameAr,Inventory.Units.NameEn AS UnitNameEn, Inventory.Branches.NameAr AS BrancheNameAr, Inventory.Branches.NameEn AS BrancheNameEn,Inventory.Stores.NameAr AS StoreNameAr, Inventory.Stores.NameEn AS StoreNameEn, Inventory.ItemGroups.NameAr AS ItemGroupNameAr,Inventory.ItemGroups.NameEn AS ItemGroupNameEn,Transactions.Transaction_InvDetails.Quntity * System.Documents.DocSign AS SignedQuntity, Transactions.Transaction_InvDetails.SalesPrice AS SalesPrice, Transactions.Transaction_InvMaster.DocDate AS DocDate FROM Transactions.Transaction_InvMaster INNER JOIN Transactions.Transaction_InvDetails ON Transactions.Transaction_InvMaster.Id = Transactions.Transaction_InvDetails.MasterId INNER JOIN Inventory.Items ON Transactions.Transaction_InvDetails.ItemId = Inventory.Items.Id INNER JOIN Inventory.Units ON Transactions.Transaction_InvDetails.UnitId = Inventory.Units.Id INNER JOIN Inventory.ItemGroups ON Transactions.Transaction_InvDetails.GroupId = Inventory.ItemGroups.Id INNER JOIN Inventory.Branches ON Transactions.Transaction_InvMaster.BranchId = Inventory.Branches.Id INNER JOIN Inventory.Stores ON Transactions.Transaction_InvMaster.StoreId = Inventory.Stores.Id INNER JOIN System.Documents ON Transactions.Transaction_InvMaster.DocTypeId = System.Documents.DocTypeId INNER JOIN Sales.Customers ON Transactions.Transaction_InvMaster.CustomerId = Sales.Customers.Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW ItemSalesAnaysisReportView");

        }
    }
}
