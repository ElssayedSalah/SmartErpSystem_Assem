using System;

namespace DataAccessLayer.Entities.Financial
{
    public class AlAstazAccountReportView
    {    
        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public string AccountNameAr { get; set; }
        public string AccountNameEn { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public int EntryId { get; set; }
        public int EntryNumber { get; set; }
        public int EntryType { get; set; }
        public int DailyTypeId { get; set; }
        public int EntryCreationMethod { get; set; }
        public int CurrencyId { get; set; }
        public decimal CurrencyFactor { get; set; }
        public int IsTransfered { get; set; }
        public int EntryState { get; set; }
        public int DocType { get; set; }
        public int DocNumber { get; set; }
        public int FinancialPeriodId { get; set; }
        public int CompanyId { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string MasterNotes { get; set; }
        public string DetailsNotes { get; set; }
        public decimal BalanceDebit { get; set; }
        public decimal BalanceCredit { get; set; }
        public int CostCenterId { get; set; }


    }
}
