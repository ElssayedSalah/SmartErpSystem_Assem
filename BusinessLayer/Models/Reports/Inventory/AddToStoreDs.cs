using System.Collections.Generic;

namespace Reports.Models.Inventory
{
    public class AddToStoreDs : TransMaster
    {
     
        public string SupplierName { get; set; }
       public List<TransDetails> Details { get; set; }
        public AddToStoreDs()
        {
            Details = new List<TransDetails>();
        }
    }
}
