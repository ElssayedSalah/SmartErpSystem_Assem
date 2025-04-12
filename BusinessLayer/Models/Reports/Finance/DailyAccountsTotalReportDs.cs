namespace BusinessLayer.Models.Reports.Finance
{
    public class DailyAccountsTotalReportDs
    {
        public int EntryNumber { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string CoastCenterName { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal OpenBalanceDebit { get; set; }
        public decimal OpenBalanceCredit { get; set; }
        public decimal BalanceDebit { get; set; }
        public decimal BalanceCredit { get; set; }
        public string Notes { get; set; }
    }
}
