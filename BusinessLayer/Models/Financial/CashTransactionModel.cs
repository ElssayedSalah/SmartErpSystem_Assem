using AutoMapper.Configuration.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Financial
{
    public class CashTransactionModel: TransactionModel
    {
        [Display(Name = "Branch")]
        [Required(ErrorMessage = "BranchRequired")]
        public int BranchId { get; set; }
        [Display(Name = "EntryNumber")]
        public int? EntryNumber { get; set; }
        public int EntryId { get; set; }

        [Display(Name = "Amount")]
        [Required(ErrorMessage = "AmountRequired")]
        public decimal Amount { get; set; }
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
        /// <summary>
        /// نوع الطرف الاول مثل حساب او عميل او مورد او خزنة او بنك
        /// </summary>
        [Display(Name = "PayedFrom")]
        public int FirstSideTypeId { get; set; }
        /// <summary>
        /// نوع الطرف الثاني مثل حساب او عميل او مورد او خزنة او بنك
        /// </summary>
        [Display(Name = "PayedType")]
        public int SecondSideTypeId { get; set; }
        /// <summary>
        /// رقم الطرف الاول البريمرلي كي للحساب او المورد او العميل او الخزنة او البنك
        /// </summary>
        [Display(Name = "Name")]
        public int FirstSideId { get; set; }
        /// <summary>
        /// رقم الطرف الثاني البريمرلي كي للحساب او المورد او العميل او الخزنة او البنك
        /// </summary>
        [Display(Name = "Name")]
        public int SecondSideId { get; set; }
        /// <summary>
        /// رقم حساب الطرف الاول 
        /// </summary>
        [Display(Name = "Account")]
        public int FirstSideAccountId { get; set; }
        /// <summary>
        /// رقم حساب الطرف الثاني
        /// </summary>
        [Display(Name = "Account")]
        public int SecondSideAccountId { get; set; }


        [Ignore]
        [Display(Name = "Branch")]
        public string BranchName { get; set; }
    }
}
