using System;

namespace DataAccessLayer.Entities.Inventory
{
    public class ItemSalesAnaysisReportView
    {
        public int StoreId { get; set; }
        public int BranchId { get; set; }
        public int ItemId { get; set; }
        public int GroupId { get; set; }
        public int CustomerId { get; set; }
        public decimal Quntity { get; set; }       
        public decimal SignedQuntity { get; set; }      
        public int UnitId { get; set; }
        public int FinancialPeriodId { get; set; }
        public int CompanyId { get; set; }
        public int DocTypeId { get; set; }
        public int DocSign { get; set; }
        public string ItemNameAr { get; set; }
        public string ItemNameEn { get; set; }
        public string UnitNameAr { get; set; }
        public string UnitNameEn { get; set; }
        public string BrancheNameAr { get; set; }
        public string BrancheNameEn { get; set; }
        public string StoreNameAr { get; set; }
        public string StoreNameEn { get; set; }
        public string ItemGroupNameAr { get; set; }
        public string ItemGroupNameEn { get; set; }
        public decimal SalesPrice { get; set; }
        public DateTime DocDate { get; set; }
        public string DocNameAr { get; set; }
        public string DocNameEn { get; set; }
        public string CustomerNameAr { get; set; }
        public string CustomerNameEn { get; set; }
        public int DocNumber { get; set; }

    }
}
