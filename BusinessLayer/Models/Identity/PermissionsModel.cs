using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessLayer.Helpers.SharedEnums;

namespace BusinessLayer.Models.Identity
{
   public class PermissionsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SystemName { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }

        public IList<ActionsModel> Actions { get; set; }

        public PermissionsModel()
        {

            Actions = new List<ActionsModel>() {
                new ActionsModel() { Id=(int)PermissionActions.Create, Name = "Create", Selected=false ,Allowed=true},
                new ActionsModel() { Id=(int)PermissionActions.Edit, Name = "Edit", Selected=false,Allowed=true },
                new ActionsModel() { Id=(int)PermissionActions.Delete, Name = "Delete", Selected=false ,Allowed=true},
                new ActionsModel() { Id=(int)PermissionActions.List, Name = "List", Selected=false,Allowed=true },
                new ActionsModel() { Id=(int)PermissionActions.Report, Name = "Report", Selected=false ,Allowed=true}

            };
        }
    }
}
