using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Financial
{
    public class DailyEntryMasterModel: Model
    {
        public DailyEntryMasterModel()
        {
            DailyEntryDetails = new List<DailyEntryDetailsModel>() { new DailyEntryDetailsModel() { } };
        }
        [Display(Name = "EntryNumber")]
        public int EntryNumber { get; set; }
        /// <summary>
        /// نوع القيد افتتاحي او حركة
        /// </summary>        
        public int EntryType { get; set; }
        [Display(Name = "DocDate")]
        public DateTime TransactionDate { get; set; }
        /// <summary>
        /// رقم يومية الحسابات
        /// </summary>
        [Display(Name = "DailyAccounts_DefModel")]
        public int DailyTypeId { get; set; }
        /// <summary>
        /// طريقة انشاء القيد ألي او يدوي
        /// </summary>
        [Display(Name = "EntryCreationMethod")]
        public int EntryCreationMethod { get; set; }
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }
        [Display(Name = "CurrencyChangrRate")]
        public decimal CurrencyFactor { get; set; }
        [Display(Name = "TransferState")]
        public int IsTransfered { get; set; }
        /// <summary>
        /// حالة القيد متزن او غير متزن
        /// </summary>
        [Display(Name = "EntryBalanceState")]
        public int EntryState { get; set; }
        /// <summary>
        /// نوع الحركة التي انشأت القيد
        /// </summary>
        [Display(Name = "DocType")]
        public int DocType { get; set; }
        /// <summary>
        /// رقم الحركة التي أنشأت القيد
        /// </summary>
        [Display(Name = "DocCode")]
        public int DocNumber { get; set; }
        [Display(Name = "TotalDebit")]
        public decimal TotalDebit { get; set; }
        [Display(Name = "TotalCredit")]
        public decimal TotalCredit { get; set; }
        public int FinancialPeriodId { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Ignore]
        [Display(Name = "DocType")]
        public string DocTypeName { get; set; }      
        


        public List<DailyEntryDetailsModel> DailyEntryDetails { get; set; }
    }
}
