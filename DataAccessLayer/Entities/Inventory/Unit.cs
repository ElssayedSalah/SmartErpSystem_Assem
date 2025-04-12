using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.Inventory
{
    [Table("Units", Schema = "Inventory")]
    public class Unit : BasicEntity
    {
        public string TaxAuthorityCode { get; set; }
        //public Item Item { get; set; }


    }
}
