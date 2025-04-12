using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.EGInvoice
{
    public class EGInvoiceModel
    {
        public InvMaster Master { get; set; }
        public List<InvDetails> InvDetails { get; set; }
        public EGInvoiceModel()
        {
            InvDetails = new List<InvDetails>();
        }
    }
}
