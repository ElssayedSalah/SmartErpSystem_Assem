using System;

namespace BusinessLayer.Models.Reports.Finance
{
    public class TreasuryStatementOfAccountReportDs
    {
        public int EntryNumber { get; set; }
        public int DocumentNumber { get; set; }
        public string DocumentName { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal OpenBalanceDebit { get; set; }
        public decimal OpenBalanceCredit { get; set; }
        public decimal BalanceDebit { get; set; }
        public decimal BalanceCredit { get; set; }
        public string Notes { get; set; }
    }
}
