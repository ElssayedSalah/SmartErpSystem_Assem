using System.Collections.Generic;

namespace Reports.Models.Inventory
{
    public class PurchasesInvoiceDs : TransMaster
    {
        public decimal InvoiceValue { get; set; }
        public decimal TotalAdditions { get; set; }
        public decimal TotalDisounts { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal TaxValue { get; set; }
        public decimal InvoiceNet { get; set; }
        public string CurrencyName { get; set; }
        public string SupplerName { get; set; }
        public List<TransDetails> Details { get; set; }
        public PurchasesInvoiceDs()
        {
            Details = new List<TransDetails>();
        }
    }
}
