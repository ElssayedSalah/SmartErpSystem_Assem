using Reports.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Filters
{
    public class DailyAccountsTotalReportFilters : BaseFiltersModel
    {
        [Display(Name = "FromDate")]
        public DateTime FromDate { get; set; }
        [Display(Name = "ToDate")]
        public DateTime ToDate { get; set; }
        [Display(Name = "Account")]
        [Required(ErrorMessage = "AccountIdRequired")]
        public int AccountId { get; set; }
        [Display(Name = "CoastCenter")]
        public int CoastCenterId { get; set; }
        [Display(Name = "DailyEntry")]
        public int DailyAccountsId { get; set; }


    }
}
