using Reports.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.Filters
{
   public class ItemDataReportFilters : BaseFiltersModel
    {
        [Display(Name = "Store")]
        public int? StoreId { get; set; }

        [Display(Name = "Branch")]
        public int? BranchId { get; set; }

        [Display(Name = "ItemModel")]
        public int? ItemId { get; set; }

        [Display(Name = "ItemGroup")]
        public int? GroupId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public ItemDataReportFilters()
        {
            FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ToDate = DateTime.Now;
        }

        
    }
}
