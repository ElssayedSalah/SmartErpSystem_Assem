using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.Financial
{
    [Table("DailyEntryMaster", Schema = "Financial")]
   public class DailyEntryMaster: Entity
    {       
        public int EntryNumber { get; set; }
        /// <summary>
        /// نوع القيد افتتاحي او حركة
        /// </summary>
        public int EntryType { get; set; }
        public DateTime TransactionDate { get; set; }
        /// <summary>
        /// رقم يومية الحسابات
        /// </summary>
        public int DailyTypeId { get; set; }
        /// <summary>
        /// طريقة انشاء القيد ألي او يدوي
        /// </summary>
        public int EntryCreationMethod { get; set; }      
        public int CurrencyId { get; set; }
        public decimal CurrencyFactor { get; set; }       
        public int IsTransfered { get; set; }
        /// <summary>
        /// حالة القيد متزن او غير متزن
        /// </summary>
        public int EntryState { get; set; }
        /// <summary>
        /// نوع الحركة التي انشأت القيد
        /// </summary>
        public int DocType { get; set; }
        /// <summary>
        /// رقم الحركة التي أنشأت القيد
        /// </summary>
        public int DocNumber { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }

        public int FinancialPeriodId { get; set; }       
        public string Notes { get; set; }      

        public List<DailyEntryDetails> DailyEntryDetails { get; set; }
        public DailyEntryMaster()
        {
            DailyEntryDetails = new List<DailyEntryDetails>();
        }

    }
}
