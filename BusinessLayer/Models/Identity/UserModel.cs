using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.Identity
{
  public  class UserModel:Model
    {
       
        public string Id { get; set; }      

        [Required(ErrorMessage = "UserNameValidationMsg")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "PasswordValidationMsg")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPasswordValidationMsg")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "EmailValidationMsg")]
        [Display(Name = "Email")]
        public string Email { get; set; }
               
        public int? FinancialPeriodId { get; set; }
        public int? FinancialPeriod { get; set; }

        [Display(Name = "ActivationState")]
        public bool ActivationState { get; set; }
        public bool IsSystemUser { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
        [Display(Name = "Employee")]
        [Required(ErrorMessage = "EmployeeRequired")]
        public int? EmployeeId { get; set; }
        [Display(Name = "Role")]
        [Required(ErrorMessage = "RoleRequired",AllowEmptyStrings =false)] 
        [Ignore]
        public string RoleId { get; set; }
        public int? BranchId { get; set; }

        public UserModel()
        {
            
        }

    }
}
