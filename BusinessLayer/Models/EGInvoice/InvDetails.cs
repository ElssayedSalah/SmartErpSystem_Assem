using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.EGInvoice
{
   public class InvDetails
    {
        public int Id { get; set; }
        public string DocTypeName { get; set; }
        public string DocType { get; set; }
        public int DocCode { get; set; }
        public DateTime DocDate { get; set; }
        /// <summary>
        /// البائع
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// المشتري
        /// </summary>
        public string Receiver { get; set; }
        public string DocState { get; set; }
        public string UUID { get; set; }
    }
}
