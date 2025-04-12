using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.EGElectronicInvoice
{
   public class UnitValue
    {
        public string currencySold { get; set; }
        public double amountEGP { get; set; }
        public double amountSold { get; set; }
        public double currencyExchangeRate { get; set; }
    }
}
