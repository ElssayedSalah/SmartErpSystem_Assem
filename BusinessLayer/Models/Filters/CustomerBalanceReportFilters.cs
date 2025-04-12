using Reports.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Filters
{
    public class CustomerBalanceReportFilters : BaseFiltersModel
    {
        [Display(Name = "CustomerName")]
        public int? CustomerId { get; set; }
        [Display(Name = "FromDate")]
        public DateTime FromDate { get; set; }
        [Display(Name = "ToDate")]
        public DateTime ToDate { get; set; }
        public CustomerBalanceReportFilters()
        {
            FromDate = new DateTime(DateTime.Now.Year, 1, 1);
            ToDate = new DateTime(DateTime.Now.Year, 12, 30);
        }

        
    }
}
