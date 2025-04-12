using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.Inventory
{
    [Table("Stores", Schema = "Inventory")]
    public class Store : BasicEntity
    {
        public int? BranchId { get; set; }
    }
}
