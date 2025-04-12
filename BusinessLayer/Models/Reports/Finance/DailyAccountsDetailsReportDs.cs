namespace BusinessLayer.Models.Reports.Finance
{
    public class DailyAccountsDetailsReportDs
    {
        public int EntryNumber { get; set; }
        public int DocumentNumber { get; set; }
        public string DocumentName { get; set; }
        public string AccountName { get; set; }
        public string CoastCenterName { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Notes { get; set; }
    }
}
