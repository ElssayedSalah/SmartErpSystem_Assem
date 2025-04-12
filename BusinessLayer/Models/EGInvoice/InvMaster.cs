using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessLayer.Helpers.SharedEnums;

namespace BusinessLayer.Models.EGInvoice
{
    public class InvMaster
    {
        [Display(Name = "FromDate")]
        public DateTime From { get; set; }
        [Display(Name = "ToDate")]
        public DateTime To { get; set; }
        [Display(Name = "DocType")]
        public EGInvoiceDocumentType DocumentType { get; set; }
        [Display(Name = "DocumentState")]
        public EGInvoiceDocumentState DocumentState { get; set; }

    }
}
