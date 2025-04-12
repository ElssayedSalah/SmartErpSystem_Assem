using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessLayer.Helpers.SharedEnums;

namespace BusinessLayer.Models.Identity
{
   public class PermissionsMappingModel
    {
        public IList<PermissionsModel> AvailablePermissions { get; set; }
        public IList<RoleModel> AvailableRoles { get; set; }
        public IList<ActionsModel> AvailableActions { get; set; }

        public PermissionsMappingModel()
        {
            AvailablePermissions = new List<PermissionsModel>();
            AvailableRoles = new List<RoleModel>();          

            AvailableActions = new List<ActionsModel>() {
                new ActionsModel() { Id=(int)PermissionActions.Create, Name = "Create", Selected=false },
                new ActionsModel() { Id=(int)PermissionActions.Edit, Name = "Edit", Selected=false },
                new ActionsModel() { Id=(int)PermissionActions.Delete, Name = "Delete", Selected=false },
                new ActionsModel() { Id=(int)PermissionActions.List, Name = "List", Selected=false },
                new ActionsModel() { Id=(int)PermissionActions.Report, Name = "Report", Selected=false }

            };
        }

    }
}
