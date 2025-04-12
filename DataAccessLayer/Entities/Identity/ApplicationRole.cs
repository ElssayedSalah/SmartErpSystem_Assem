using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.Identity
{
   public class ApplicationRole : IdentityRole
    {        
        public string NameAr { get; set; }
        public bool ActivationState { get; set; }
        public bool IsSystemRole { get; set; }
        public string Notes { get; set; }
        //public string Name { get { return NameEn; } set { value = NameEn; } }

    }
}
