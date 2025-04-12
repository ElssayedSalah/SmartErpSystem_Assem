using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.Identity
{
    //permissions
    [Table("AspNetClaims", Schema = "dbo")]
    public class AspNetClaims
    {
        public int Id { get; set; }       
        public string Name { get; set; }
        public string SystemName { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        /// <summary>
        /// to view forms that has VisibleForSuperOnly=true for super user only
        /// </summary>
        [NotMapped]
        public bool VisibleForSuperOnly { get; set; }  
        [NotMapped]
        public List<int> AllowedActions { get; set; }

        public AspNetClaims()
        {
            AllowedActions = new List<int>();
        }
    }
}
