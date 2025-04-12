using BusinessLayer.Models.Identity;
using DataAccessLayer.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
   public interface IPermissionProvider
    {
        IEnumerable<AspNetClaims> GetPermissions();
        IList<ActionsModel> AvailableActions { get; set; }

    }
}
