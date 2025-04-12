using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.Inventory
{
    [Table("Transaction_InvDetails", Schema = "Transactions")]
    public class Transaction_InvDetails : TransactionEntity
    {
        [ForeignKey("Tr_Master_Details_FK")]
        public int MasterId { get; set; }
        [ForeignKey("InvDetails_Items_FK")]
        public int ItemId { get; set; }
        public int? GroupId { get; set; }
        public decimal Quntity { get; set; }
        public int? UnitId { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalesPrice { get; set; }       
        public decimal? PriceAfterDiscount { get; set; }       
        public int? DiscountType { get; set; }       
        public decimal? DiscountValue { get; set; }       
        public decimal? Total { get; set; }
        public decimal? ActualQuntity { get; set; }
        public decimal? DifferenceQuntity { get; set; }
        public Transaction_InvMaster Transaction_InvMaster { get; set; }
        public Item Item { get; set; }
    }


}
