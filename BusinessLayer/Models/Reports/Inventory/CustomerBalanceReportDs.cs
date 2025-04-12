using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Models.Inventory
{
   public class CustomerBalanceReportDs
    {        
       
        public string BranchName { get; set; }
        public string CustomerName { get; set; }
        public decimal AmountDebit { get; set; }
        public decimal AmountCredit { get; set; }
        public decimal OpenBalanceDebit { get; set; }
        public decimal OpenBalanceCredit { get; set; }
        public decimal BalanceDebit { get; set; }
        public decimal BalanceCredit { get; set; }
        public DateTime DocDate { get; set; }
        public string DocName { get; set; }
        public int DocNumber { get; set; }
        public string Notes { get; set; }
       
    }
}
