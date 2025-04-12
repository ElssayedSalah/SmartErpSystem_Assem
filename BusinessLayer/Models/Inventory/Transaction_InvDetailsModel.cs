using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Models.Inventory
{
    public class Transaction_InvDetailsModel:TransactionModel
    {
        public int MasterId { get; set; }
        [Required]
        [Display(Name = "ItemModel")]       
        public int ItemId { get; set; }
        [Display(Name = "ItemGroup")]
        public int GroupId { get; set; }
        [Required]
        [Display(Name = "Quntity")]
        public decimal? Quntity { get; set; }
        [Display(Name = "DefaultUnit")]
        public int UnitId { get; set; }
        [Display(Name = "PurchasePrice")]
        public decimal? PurchasePrice { get; set; }
        [Display(Name = "SalesPrice")]
        public decimal? SalesPrice { get; set; }
        [Display(Name = "PriceAfterDiscount")]
        public decimal? PriceAfterDiscount { get; set; }
        [Display(Name = "DiscountType")]
        public int? DiscountType { get; set; }
        [Display(Name = "DiscountValue")]
        public decimal? DiscountValue { get; set; }
        [Display(Name = "Total")]
        public decimal? Total { get; set; }

        [Display(Name = "ActualQuntity")]
        public decimal? ActualQuntity { get; set; }
        [Display(Name = "DifferenceQuntity")]
        public decimal? DifferenceQuntity { get; set; }

        public ItemModel Item { get; set; }

        public Transaction_InvDetailsModel()
        {
            Quntity = 0;
            PurchasePrice = 0;
            SalesPrice = 0;
            PriceAfterDiscount = 0;
            DiscountType = 1;
            DiscountValue = 0;
            Total = 0;
        }

    }
}
