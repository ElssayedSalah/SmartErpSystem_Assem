using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.Identity
{
  public class RoleModel:Model
    {
        public string Id { get; set; }
        [Display(Name = "NameEn")]
        [Required(ErrorMessage = "NameEnRequired")]
        public string Name { get; set; }
        [Display(Name = "NameAr")]
        [Required(ErrorMessage = "NameArRequired")]
        public string NameAr { get; set; }
        [Display(Name = "ActivationState")]
        public bool ActivationState { get; set; }
        [Display(Name = "IsSystemRole")]
        public bool IsSystemRole { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }

        public IList<PermissionsModel> RolePermissions { get; set; }
        public RoleModel()
        {
            RolePermissions = new List<PermissionsModel>();
        }

    }
}
