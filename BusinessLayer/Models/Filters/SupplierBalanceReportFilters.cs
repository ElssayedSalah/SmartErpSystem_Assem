using Reports.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.Filters
{
   public class SupplierBalanceReportFilters : BaseFiltersModel
    {
        [Display(Name = "SupplerName")]
        public int? SupplierId { get; set; }
        [Display(Name = "FromDate")]
        public DateTime FromDate { get; set; }
        [Display(Name = "ToDate")]
        public DateTime ToDate { get; set; }
        public SupplierBalanceReportFilters()
        {
            FromDate = new DateTime(DateTime.Now.Year, 1, 1);
            ToDate = new DateTime(DateTime.Now.Year, 12, 30);
        }

        
    }
}
