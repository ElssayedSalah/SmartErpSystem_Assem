using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.EGElectronicInvoice
{
   public class SubmissionDocumentResult
    {
        public string SubmissionId { get; set; }
        public List<AcceptedDocuments> AcceptedDocuments;
        public List<RejectedDocuments> RejectedDocuments;
    }
}
