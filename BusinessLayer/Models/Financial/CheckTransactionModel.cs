using AutoMapper.Configuration.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Financial
{
    public class CheckTransactionModel : TransactionModel
    {
        [Display(Name = "Branch")]
        [Required(ErrorMessage = "BranchRequired")]
        public int BranchId { get; set; }
        [Display(Name = "EntryNumber")]
        public int EntryNumber { get; set; }
        public int EntryId { get; set; }
        [Display(Name = "CheckNumber")]
        [Required(ErrorMessage = "CheckNumberRequired")]
        public string CheckNumber { get; set; }
        [Display(Name = "CheckOwner")]
        public string CheckOwner { get; set; }
        [Display(Name = "BankModel")]
        [Required(ErrorMessage = "BankRequired")]
        public int BankId { get; set; }
        [Display(Name = "BankAccountNumber")]
        [Required(ErrorMessage = "BankAccountNumberRequired")]
        public string BankAccountNumber { get; set; }
        [Display(Name = "BankBranchName")]
        public string BankBranchName { get; set; }
        [Display(Name = "Amount")]
        [Required(ErrorMessage = "AmountRequired")]
        public decimal Amount { get; set; }
        [Display(Name = "DueDate")]
        [Required(ErrorMessage = "DueDateRequired")]
        public DateTime DueDate { get; set; }
        [Display(Name = "DiscountAmount")]
        public decimal DiscountAmount { get; set; }
        [Display(Name = "Currency")]
        [Required(ErrorMessage = "CurrencyRequired")]
        public int CurrencyId { get; set; }
        [Display(Name = "CurrencyChangrRate")]
        public decimal CurrencyFactor { get; set; }
        [Display(Name = "CoastCenter")]
        public int CostCenterId { get; set; }
        [Display(Name = "DiscountAccount")]
        public int DiscountAccountId { get; set; }
        [Display(Name = "PayedFrom")]
        public int FirstSideTypeId { get; set; }
        [Display(Name = "PayedType")]
        public int SecondSideTypeId { get; set; }
        [Display(Name = "Name")]
        public int FirstSideId { get; set; }
        [Display(Name = "Name")]
        public int SecondSideId { get; set; }
        [Display(Name = "Account")]
        public int FirstSideAccountId { get; set; }
        [Display(Name = "Account")]
        public int SecondSideAccountId { get; set; }
        [Ignore]
        public string BranchName { get; set; }
    }
}
