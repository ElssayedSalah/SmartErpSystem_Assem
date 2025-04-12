using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.Inventory
{
    [Table("ItemGroups", Schema = "Inventory")]
    public class ItemGroup : BasicEntity
    {

    }
}
