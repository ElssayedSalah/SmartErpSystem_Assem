using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.System
{
   public class SystemSettingModel:Model
    {       
        public bool ActivateE_Invoice { get; set; }
        public string IdentityService_Url { get; set; }
        public string System_APIUrl { get; set; }       
        public bool SendE_InvoiceDirectly { get; set; }
        [Display(Name = "E_InvoiceType")]
        public int E_InvoiceType { get; set; }
        [Display(Name = "EnableOpenEntryCreation")]
        public bool EnableOpenEntryCreation { get; set; }
        [Display(Name = "EnableTransactionsEntryCreation")]
        public bool EnableTransactionsEntryCreation { get; set; }
        [Ignore]
        public bool EnableCreate { get; set; }

    }
}
