using System.Collections.Generic;

namespace Reports.Models.Inventory
{
    public class SalesInvoiceDs : TransMaster
    {
        public decimal InvoiceValue { get; set; }
        public decimal TotalAdditions { get; set; }
        public decimal TotalDisounts { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal TaxValue { get; set; }
        public decimal InvoiceNet { get; set; }
        public string CurrencyName { get; set; }
        public string CustomerName { get; set; }
       public List<TransDetails> Details { get; set; }
        public SalesInvoiceDs()
        {
            Details = new List<TransDetails>();
        }
    }
}
