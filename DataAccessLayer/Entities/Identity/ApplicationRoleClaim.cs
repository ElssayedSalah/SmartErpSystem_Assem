using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.Identity
{
    //role permissions
   public class ApplicationRoleClaim: IdentityRoleClaim<string>
    {
        public int ClaimId { get; set; }

    }
}
