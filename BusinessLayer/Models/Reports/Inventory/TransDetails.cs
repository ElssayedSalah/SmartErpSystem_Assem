using Reports.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Models.Inventory
{
   public class TransDetails : ISubDs
    {
        public string ItemName { get; set; }
        public int ItemCode { get; set; }
        public string GroupName { get; set; }
        public decimal Quntity { get; set; }
        public string UnitName { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public decimal PurchasePrice { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal Total { get; set; }
        public decimal? ActualQuntity { get; set; }
        public decimal? DifferenceQuntity { get; set; }
    }
}
