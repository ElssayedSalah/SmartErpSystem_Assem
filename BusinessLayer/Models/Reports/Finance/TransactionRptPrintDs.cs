using Reports.Model;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Models.Reports.Finance
{
    public class TransactionRptPrintDs : ReportBaseModel
    {
        public int DocNumber { get; set; }        
        public DateTime DocumentDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Currency { get; set; }
        public decimal CurrencyFactor { get; set; }
        public int EntryNumber { get; set; } 
        public string CostCenterName { get; set; }
        public string FirstSideType { get; set; }
        public string FirstSideName { get; set; }
        public string FirstSideAccount { get; set; }
        public string SecondSideType { get; set; }
        public string SecondSideName { get; set; }
        public string SecondSideAccount { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Branch { get; set; }
        public string Bank { get; set; }
        public string CheckNumber { get; set; }
        public string DiscountAccount { get; set; }
        public string Notes { get; set; }

    }   

}
