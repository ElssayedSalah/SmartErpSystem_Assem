using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Models.Inventory
{
   public class SupplierBalanceTotalReportDs
    {        
       
        public string SupplierName { get; set; }
        public int SupplierCode { get; set; }
        public decimal AmountDebit { get; set; }
        public decimal AmountCredit { get; set; }
        public decimal OpenBalanceDebit { get; set; }
        public decimal OpenBalanceCredit { get; set; }
        public decimal BalanceDebit { get; set; }
        public decimal BalanceCredit { get; set; }
        public string Notes { get; set; }
       
    }
}
