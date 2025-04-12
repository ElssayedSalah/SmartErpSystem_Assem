using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Models.Inventory
{
   public class ItemBalanceQuantityAndValueReportDs
    {
        public string ItemName { get; set; }
        public int? ItemCode { get; set; }
        public int? ItemId { get; set; }
        public int? GroupId { get; set; }
        public int? BranchId { get; set; }
        public int? StoreId { get; set; }
        public string GroupName { get; set; }
        public string BranchName { get; set; }
        public decimal Quntity { get; set; }  
        public decimal SignedQuntity { get; set; }
        public string UnitName { get; set; }
        public string StoreName { get; set; }
        public decimal AvgPurchasePrice { get; set; }
        public decimal Coast { get; set; }
    }
}
