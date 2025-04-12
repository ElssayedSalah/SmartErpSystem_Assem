namespace Reports.Models.Inventory
{
    public class ItemPurshasAnaysisReportDs
    {        
       
        public string BranchName { get; set; }
        public string SupplierName { get; set; }
        public string ItemName { get; set; }
        public decimal PurshasQuntity { get; set; }
        public decimal PurshasValue { get; set; }
        public decimal PurshasReturnQuntity { get; set; }
        public decimal PurshasReturnValue { get; set; }
        public decimal PurshasNetQuntity { get; set; }
        public decimal PurshasNetValue { get; set; }
        public decimal PurchasePrice { get; set; }
       
    }
}
