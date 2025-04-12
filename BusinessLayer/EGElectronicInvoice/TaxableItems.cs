using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.EGElectronicInvoice
{
   public class TaxableItems
    {
        public string taxType { get; set; }
        public decimal amount { get; set; }
        public string subType { get; set; }
        public decimal rate { get; set; }
    }
}
