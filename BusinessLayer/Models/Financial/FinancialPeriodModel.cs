using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Financial
{
    public class FinancialPeriodModel : BasicModel
    {
        [Display(Name = "FinancialPeriod")]
        [Required(ErrorMessage = "FinancialPeriodRequired")]       
        public int Year { get; set; }
        [Display(Name = "FromDate")]
        [Required(ErrorMessage = "DateFromRequired")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateFrom { get; set; }
        [Display(Name = "ToDate")]
        [Required(ErrorMessage = "DateToRequired")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTo { get; set; }
        [Display(Name = "isClosed")]
        public bool isClosed { get; set; }
        [Display(Name = "CloseDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime CloseDate { get; set; }

        public string NameAr { get { return Year.ToString(); } }

    }
}
