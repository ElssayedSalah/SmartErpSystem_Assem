using Reports.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Filters
{
    public class BankStatementOfAccountReportFilters : BaseFiltersModel
    {
        [Display(Name = "FromDate")]
        public DateTime FromDate { get; set; }
        [Display(Name = "ToDate")]
        public DateTime ToDate { get; set; }
        [Display(Name = "BankModel")]
        [Required(ErrorMessage = "BanckIdRequired")]
        public int BanckId { get; set; }

        [Display(Name = "DocType")]
        public int DocTypeId { get; set; }
     


    }
}
