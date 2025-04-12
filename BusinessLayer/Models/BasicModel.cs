using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class BasicModel:Model
    {

        [Display(Name = "NameAr")]
        [Required(ErrorMessage = "NameArRequired")]
        public string NameAr { get; set; }
        [Display(Name = "NameEn")]
        public string NameEn { get; set; }
        [Display(Name = "ActivationState")]
        public bool ActivationState { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "Name")]      
        public string Name { get { return Thread.CurrentThread.CurrentCulture.Name=="ar"? NameAr: NameEn; } }
        public int? FinancialPeriodId { get; set; }

    }
}
