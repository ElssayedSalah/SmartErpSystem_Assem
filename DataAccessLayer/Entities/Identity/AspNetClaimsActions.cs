using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.Identity
{
    //role permissions actions
    [Table("AspNetRoleClaimActions", Schema = "dbo")]
    public class AspNetRoleClaimActions
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public int ClaimId { get; set; }
        public int ActionId { get; set; }
        public bool Selected { get; set; }       
    }
}
