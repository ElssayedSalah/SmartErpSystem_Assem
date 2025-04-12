using AutoMapper.Configuration.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Financial
{
    public class AccountModel
    {
        [Key]
        public int Id { get; set; }
        public int ParentId { get; set; }
        [Display(Name = "AccountCode")]
        public string AccountCode { get; set; }
        [Display(Name = "NameAr")]
        [Required(ErrorMessage = "NameArRequired")]
        public string NameAr { get; set; }
        [Display(Name = "NameEn")]
        public string NameEn { get; set; }
        [Display(Name = "AccountCreatetionDate")]
        public DateTime OpenDate { get; set; }
        [Display(Name = "AccountNature")]
        public int AccountNatureId { get; set; }
        public int CurrencyId { get; set; }
        public decimal CurrencyRate { get; set; }
        [Display(Name = "ConnectedToCostCenter")]
        public bool ConnectedToCostCenter { get; set; }
        [Display(Name = "LastLevelInTree")]
        public bool LastLevelInTree { get; set; }
        public int Level { get; set; }
        [Display(Name = "AccountPostTo")]
        public int? PostTo { get; set; }
        [Display(Name = "AccountPostType")]
        public int? PostType { get; set; }
        public int? JournalId { get; set; }
        public decimal OpenBalanceCredit { get; set; }
        public decimal OpenBalanceDebit { get; set; }
        public decimal CurrentBalanceDebit { get; set; }
        public decimal CurrentBalanceCredit { get; set; }
        public decimal TransactionsBalanceDebit { get; set; }
        public decimal TransactionsBalanceCredit { get; set; }
        [Ignore]
        public bool AllowPost { get; set; }
        [Ignore]
        public bool AllowUpdateOpenBalance { get; set; }

    }
}
