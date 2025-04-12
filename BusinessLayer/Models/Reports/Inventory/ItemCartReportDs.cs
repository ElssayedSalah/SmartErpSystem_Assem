using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Models.Inventory
{
   public class ItemCartReportDs
    {
        public string ItemName { get; set; }
        public int? ItemCode { get; set; }
        public int? ItemId { get; set; }
        public int? GroupId { get; set; }
        public int? BranchId { get; set; }
        public int? StoreId { get; set; }
        public string GroupName { get; set; }
        public string BranchName { get; set; }      
        public string UnitName { get; set; }
        public string StoreName { get; set; }
        public decimal Quntity { get; set; }
        public decimal SignedQuntity { get; set; }
        public decimal InQuntity { get; set; }
        public decimal OutQuntity { get; set; }
        public decimal CurrentQuntity { get; set; }
        public decimal DocValue { get; set; }
        public decimal BalanceValue { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal AvgPurchasePrice { get; set; }
        public string DocName { get; set; }
        public int DocNumber { get; set; }
        public DateTime DocDate { get; set; }











    }
}
