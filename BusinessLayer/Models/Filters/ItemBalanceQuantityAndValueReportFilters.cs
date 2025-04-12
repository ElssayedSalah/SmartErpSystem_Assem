using Reports.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.Filters
{
   public class ItemBalanceQuantityAndValueReportFilters : BaseFiltersModel
    {
        [Display(Name = "Store")]
        public int? StoreId { get; set; }

        [Display(Name = "Branch")]
        public int? BranchId { get; set; }

        [Display(Name = "ItemModel")]
        public int? ItemId { get; set; }

        [Display(Name = "ItemGroup")]
        public int? GroupId { get; set; }
        [Display(Name = "FromDate")]
        public DateTime FromDate { get; set; }
        [Display(Name = "ToDate")]
        public DateTime ToDate { get; set; }
        public ItemBalanceQuantityAndValueReportFilters()
        {
            FromDate = new DateTime(DateTime.Now.Year, 1, 1);
            ToDate = new DateTime(DateTime.Now.Year, 12, 30);
        }

        
    }
}
