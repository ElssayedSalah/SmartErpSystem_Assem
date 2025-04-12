using AutoMapper.Configuration.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Financial
{
    public class TransactionsEntrySettingMasterModel
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// رقم الحركة
        /// </summary>
        [Display(Name = "DocType")]
        [Required(ErrorMessage = "DocTypeRequired")]
        public int DocTypeId { get; set; }
        /// <summary>
        /// نوع اليومية
        /// </summary>
        [Display(Name = "DailyAccounts_DefModel")]
        [Required(ErrorMessage = "DailyTypeIdRequired")]
        public int DailyTypeId { get; set; }
        /// <summary>
        /// حالة ترحيل القيد
        /// </summary>
        [Display(Name = "TransferState")]
        public bool TransferState { get; set; }
        
        [Display(Name = "FirstSide")]
        [Required(ErrorMessage = "FirstSideRequired")]
        public int FirstSide { get; set; }

        [Display(Name = "SecondSide")]
        [Required(ErrorMessage = "SecondSideRequired")]
        public int SecondSide { get; set; }

        [Display(Name = "FirstSideSideNatural")]
        public int FirstSideSideNaturalId { get; set; }

        [Display(Name = "SecondSideSideNatural")]
        public int SecondSideSideNaturalId { get; set; }

        [Display(Name = "IsTaxble")]
        public bool IsFirstSideTaxble { get; set; }
        [Display(Name = "IsTaxble")]
        public bool IsSecondSideTaxble { get; set; }
        [Ignore]
        public string DocTypeName { get; set; }
        [Ignore]
        [Display(Name = "FirstSide")]
        public List<int> FirstSideIds { get; set; }
        [Ignore]
        [Display(Name = "SecondSide")]
        public List<int> SecondSideIds { get; set; }     

        public TransactionsEntrySettingMasterModel()
        {
            //FirstSideIds = new List<string>();
            //SecondSideIds = new List<string>();
        }
    }
}
