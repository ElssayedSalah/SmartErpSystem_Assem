using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Models.Inventory
{
    public class ItemModel : BasicModel
    {
        [Display(Name = "ItemGroup")]
        public int GroupId { get; set; }
        [Display(Name = "ItemType")]
        public int TypeId { get; set; }   
        [Display(Name = "DefaultUnit")]
        public int DefaultUnit { get; set; }
        [Display(Name = "BarCode")]
        public string BarCode { get; set; }
        [Display(Name = "ItemNature")]
        public int ItemNature { get; set; }
        [Display(Name = "TaxAuthorityType")]
        public string TaxAuthorityType { get; set; }
        [Display(Name = "TaxAuthorityCode")]
        public string TaxAuthorityCode { get; set; }

        [Display(Name = "SalesPrice")]
        public decimal SalesPrice { get; set; }
        [Display(Name = "PurchasePrice")]
        public decimal PurchasePrice { get; set; }

        [Display(Name = "ItemImagePath")]
        public string ImagePath { get; set; }
        [Ignore]
        [Display(Name = "ItemImageFile")]
        public IFormFile ItemImageFile { get; set; }
        [Ignore]      
        public string GroupName { get; set; }
        [Ignore]      
        public string UnitName { get; set; }
        [Ignore]
        public string TypeName { get; set; }
        [Ignore]
        public string TaxAuthorityRegestrationNumber { get; set; }
        public bool AllowNegativeOut { get; set; }

        public List<Transaction_InvDetailsModel> InvDetails { get; set; }


    }
}
