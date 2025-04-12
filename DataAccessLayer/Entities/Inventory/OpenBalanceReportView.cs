namespace DataAccessLayer.Entities.Inventory
{
    public class OpenBalanceReportView
    {
        public int StoreId { get; set; }
        public int BranchId { get; set; }
        public int ItemId { get; set; }
        public int GroupId { get; set; }
        public decimal Quntity { get; set; }
        public int UnitId { get; set; }
        public int FinancialPeriodId { get; set; }
        public int CompanyId { get; set; }
        public int DocTypeId { get; set; }
        public string ItemNameAr { get; set; }
        public string ItemNameEn { get; set; }
        public string UnitNameAr { get; set; }
        public string UnitNameEn { get; set; }
        public string BrancheNameAr { get; set; }
        public string BrancheNameEn { get; set; }
        public string ItemGroupNameAr { get; set; }
        public string ItemGroupNameEn { get; set; }
    }
}
