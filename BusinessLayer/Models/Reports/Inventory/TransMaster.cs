using Reports.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Models.Inventory
{
   public class TransMaster : ReportBaseModel
    {
        public int DocCode { get; set; }
        public DateTime DocDate { get; set; }
        public string BranchName { get; set; }
        public string StoreName { get; set; }
        public string DocumentName { get; set; }
       
        
    }
}
