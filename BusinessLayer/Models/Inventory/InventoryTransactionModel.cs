using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Models.Inventory
{
    public class InventoryTransactionModel
    {
        public Transaction_InvMasterModel Master { get; set; }
        public List<Transaction_InvDetailsModel> Details { get; set; }
        public InventoryTransactionModel()
        {
            Details = new List<Transaction_InvDetailsModel>() { new Transaction_InvDetailsModel() { ItemId = 0, Quntity = 0 } };
        }

    }
}
