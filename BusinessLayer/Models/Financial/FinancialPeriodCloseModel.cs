using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Financial
{
    public class FinancialPeriodCloseModel
    {
        public int CuruntPeriodId { get; set; }

        [Display(Name = "FinancialPeriod")]
        [Required(ErrorMessage = "FinancialPeriodRequired")]       
        public int CuruntYear { get; set; }
        [Display(Name = "FromDate")]
        [Required(ErrorMessage = "DateFromRequired")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CuruntDateFrom { get; set; }
        [Display(Name = "ToDate")]
        [Required(ErrorMessage = "DateToRequired")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CuruntDateTo { get; set; }

        [Display(Name = "FinancialPeriod")]
        [Required(ErrorMessage = "FinancialPeriodRequired")]
        public int NextYear { get; set; }
        [Display(Name = "FromDate")]
        [Required(ErrorMessage = "DateFromRequired")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime NextDateFrom { get; set; }
        [Display(Name = "ToDate")]
        [Required(ErrorMessage = "DateToRequired")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime NextDateTo { get; set; }
        public bool AllowClose { get; set; }



    }
}
