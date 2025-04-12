using System;

namespace BusinessLayer.Models.Reports.Finance
{
   public class ReviewBalanceReportDs
    {
        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public int AccountLength { get; set; }
        public string AccountName { get; set; }
        public string AccountNameEn { get; set; }
        public int MainAccountId { get; set; }
        public string MainAccountName { get; set; }       
        public decimal BalanceBeforDebit { get; set; }
        public decimal BalanceBeforCredit { get; set; }
        public decimal BalanceInDebit { get; set; }
        public decimal BalanceInCredit { get; set; }
        public decimal BalanceTotalDebit { get; set; }
        public decimal BalanceTotalCredit { get; set; }
        public DateTime TransactionDate { get; set; }
        public int ParentId { get; set; }
        public bool IsParent { get; set; }
        public int Level { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal OpenBalanceDebit { get; set; }
        public decimal OpenBalanceCredit { get; set; }
        public bool IsLastLevel { get; set; }
    }
}
