using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Financial
{
   public class TreasuryModel : BasicModel
    {
        [Display(Name = "Branch")]
        public string BranchId { get; set; }
        [Display(Name = "Account")]
        public int AccountId { get; set; }
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }
        [Display(Name = "OpeningBalanceDebit")]
        public decimal OpenBalanceDebit { get; set; }
        [Display(Name = "OpeningBalanceCredit")]
        public decimal OpenBalanceCredit { get; set; }
        [Display(Name = "EntryNumber")]
        public int EntryNumber { get; set; }
    }
}
