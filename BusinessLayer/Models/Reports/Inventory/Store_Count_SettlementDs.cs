using System;
using System.Collections.Generic;

namespace Reports.Models.Inventory
{
    public class Store_Count_SettlementDs : TransMaster
    {
        public DateTime CountDateFrom { get; set; }
        public DateTime CountDateTo { get; set; }
        public decimal StoreSurplusValue { get; set; }
        public decimal StoreDeficitValue { get; set; }
        public int StoreCountNumber { get; set; }
        public int EntryNumber { get; set; }
        public List<TransDetails> Details { get; set; }
        public Store_Count_SettlementDs()
        {
            Details = new List<TransDetails>();
        }
    }
}
