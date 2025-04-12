using Reports.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.Filters
{
   public class OpenBalanceReportFilters: BaseFiltersModel
    {
        [Display(Name = "Store")]
        public int? StoreId { get; set; }

        [Display(Name = "Branch")]
        public int? BranchId { get; set; }

        [Display(Name = "ItemModel")]
        public int? ItemId { get; set; }

        [Display(Name = "ItemGroup")]
        public int? GroupId { get; set; }
    }
}
