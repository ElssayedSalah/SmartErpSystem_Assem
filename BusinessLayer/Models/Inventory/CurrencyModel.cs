using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Inventory
{
    public class CurrencyModel : BasicModel
    {
        [Display(Name = "CurrencyChangrRate")]
        public decimal CurrencyChangrRate { get; set; }
        [Display(Name = "CurrencyCostExpend")]
        public decimal CurrencyCostExpend { get; set; }
        [Display(Name = "DefaultCurrency")]
        public bool DefaultCurrency { get; set; }
        [Display(Name = "TaxAuthorityCode")]
        public string TaxAuthorityCode { get; set; }
    }
}
