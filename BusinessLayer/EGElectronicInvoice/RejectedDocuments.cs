using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.EGElectronicInvoice
{
   public class RejectedDocuments
    {
        public string InternalId { get; set; }
        public Error Error;
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string InternalId { get; set; }

        public List<Details> Details;
    }
    public class Details
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

}
