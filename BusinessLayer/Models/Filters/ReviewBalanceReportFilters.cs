using Reports.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Filters
{
    public class ReviewBalanceReportFilters : BaseFiltersModel
    {
        [Display(Name = "FromDate")]
        public DateTime FromDate { get; set; }
        [Display(Name = "ToDate")]
        public DateTime ToDate { get; set; }
        [Display(Name = "Account")]
        [Required(ErrorMessage = "AccountIdRequired")]
        public int AccountId { get; set; }
        [Display(Name = "HideZeroBalance")]
        public bool HideZeroBalance { get; set; }      
       
    }
}
