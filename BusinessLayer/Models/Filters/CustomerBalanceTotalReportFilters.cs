using Reports.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Filters
{
    public class CustomerBalanceTotalReportFilters : BaseFiltersModel
    {
        [Display(Name = "Branch")]
        public int? BranchId { get; set; }
        [Display(Name = "SupplerName")]
        public int? CustomerId { get; set; }
        [Display(Name = "FromDate")]
        public DateTime FromDate { get; set; }
        [Display(Name = "ToDate")]
        public DateTime ToDate { get; set; }
        public CustomerBalanceTotalReportFilters()
        {
            FromDate = new DateTime(DateTime.Now.Year, 1, 1);
            ToDate = new DateTime(DateTime.Now.Year, 12, 30);
        }

        
    }
}
