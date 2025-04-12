using Reports.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Filters
{
    public class AlAstazAccountReportFilters: BaseFiltersModel
    {
        [Display(Name = "FromDate")]
        public DateTime FromDate { get; set; }
        [Display(Name = "ToDate")]
        public DateTime ToDate { get; set; }
        [Display(Name = "Account")]
        [Required(ErrorMessage = "AccountIdRequired")]
        public int AccountId { get; set; }
        [Display(Name = "EntryNumber")]
        public int EntryNumber { get; set; }
        [Display(Name = "DocType")]
        public int DocumentTypeId { get; set; }
        /// <summary>
        /// رقم يومية الحسابات
        /// </summary>
        [Display(Name = "DailyAccounts_DefModel")]
        public int DailyAccountsId { get; set; }
       
    }
}
