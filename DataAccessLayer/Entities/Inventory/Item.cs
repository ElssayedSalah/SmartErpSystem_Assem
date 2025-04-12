using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.Inventory
{
    [Table("Items", Schema = "Inventory")]
    public class Item : BasicEntity
    {   
        public int GroupId { get; set; }
        public int TypeId { get; set; }
        public string ImagePath { get; set; }       
        public int DefaultUnit { get; set; }
        public string BarCode { get; set; }
        public int ItemNature { get; set; }
        public string TaxAuthorityType { get; set; }
        public string TaxAuthorityCode { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public bool AllowNegativeOut { get; set; }


    }
}
