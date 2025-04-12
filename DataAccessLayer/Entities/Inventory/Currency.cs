using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.Inventory
{

    [Table("Currencys", Schema = "Inventory")]
    public class Currency : BasicEntity
    {
        public decimal CurrencyChangrRate { get; set; }
        public decimal CurrencyCostExpend { get; set; }
        public bool DefaultCurrency { get; set; }
        public string TaxAuthorityCode { get; set; }

    }

}
