using Reports.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Filters
{
    public class TreasuryStatementOfAccountReportFilters : BaseFiltersModel
    {
        [Display(Name = "FromDate")]
        public DateTime FromDate { get; set; }
        [Display(Name = "ToDate")]
        public DateTime ToDate { get; set; }
        [Display(Name = "TheTreasury")]
        [Required(ErrorMessage = "TreasuryIdRequired")]
        public int TreasuryId { get; set; }

        [Display(Name = "DocType")]
        public int DocTypeId { get; set; }
     


    }
}
