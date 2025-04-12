namespace Reports.Models.Inventory
{
    public class CustomerBalanceTotalReportDs
    {        
       
        public string CustomerName { get; set; }
        public int CustomerCode { get; set; }
        public decimal AmountDebit { get; set; }
        public decimal AmountCredit { get; set; }
        public decimal OpenBalanceDebit { get; set; }
        public decimal OpenBalanceCredit { get; set; }
        public decimal BalanceDebit { get; set; }
        public decimal BalanceCredit { get; set; }
        public string Notes { get; set; }
       
    }
}
