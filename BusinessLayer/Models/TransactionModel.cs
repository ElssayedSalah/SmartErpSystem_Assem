using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class TransactionModel : Model
    {
        [Display(Name = "DocType")]
        public int? DocTypeId { get; set; }
        [Required]
        [Display(Name = "DocDate")]
        public DateTime DocDate { get; set; }
        [Display(Name = "FinancialPeriod")]
        public int? FinancialPeriodId { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }





    }
}
