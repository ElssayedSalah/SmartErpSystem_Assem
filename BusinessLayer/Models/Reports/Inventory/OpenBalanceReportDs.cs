using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Models.Inventory
{
   public class OpenBalanceReportDs
    {
        public string ItemName { get; set; }
        public int? ItemCode { get; set; }
        public int? ItemId { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public decimal? Quntity { get; set; }
        public string UnitName { get; set; }
        public string StoreName { get; set; }
    }
}
