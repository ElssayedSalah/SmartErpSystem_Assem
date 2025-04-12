using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.EGElectronicInvoice
{
   public class Receiver
    {
        public string type { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public Address address { get; set; }
    }
}
