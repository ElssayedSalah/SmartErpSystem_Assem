using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.Inventory
{
    [Table("Departments", Schema = "Inventory")]
    public class Department : BasicEntity
    {
        public int? BranchId { get; set; }
    }
}
