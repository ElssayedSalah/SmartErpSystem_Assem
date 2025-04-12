using Reports.Model;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Models.Reports.Finance
{
    public class DailyEntryRptPrintDs : ReportBaseModel
    {
        public int DocNumber { get; set; }
        public string DocumentName { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DailyType { get; set; } 
        public string Currency { get; set; }
        public decimal CurrencyFactor { get; set; }
        public string TransferState { get; set; }       
        public string BalanceState { get; set; }
        public int EntryNumber { get; set; }       
        public string EntryCreationMethod { get; set; }
        public string Notes { get; set; }
        public List<DailyEntryRptPrintDetailsDs> Details { get; set; }

        public DailyEntryRptPrintDs()
        {
            Details = new List<DailyEntryRptPrintDetailsDs>();
        }

    }

    public class DailyEntryRptPrintDetailsDs
    {
        public string AccountName { get; set; }
        public string AccountCode { get; set; }
        public string CostCenterName { get; set; }
        public string CostCenterCode { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }       
        public string Notes { get; set; }
       
    }

}
