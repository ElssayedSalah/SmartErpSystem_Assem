namespace Reports.Models.Inventory
{
    public class ItemSalesAnaysisReportDs
    {        
       
        public string BranchName { get; set; }
        public string CustomerName { get; set; }
        public string ItemName { get; set; }
        public decimal SalesQuntity { get; set; }
        public decimal SalesValue { get; set; }
        public decimal SalesReturnQuntity { get; set; }
        public decimal SalesReturnValue { get; set; }
        public decimal SalesNetQuntity { get; set; }
        public decimal SalesNetValue { get; set; }
        public decimal SalesPrice { get; set; }
       
    }
}
