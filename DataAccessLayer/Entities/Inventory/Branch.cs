using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.Inventory
{
    [Table("Branches", Schema = "Inventory")]
    public class Branch: BasicEntity
    {      
        public string TaxAuthorityCode { get; set; }
       
    }
}
