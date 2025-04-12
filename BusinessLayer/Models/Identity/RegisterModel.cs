using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.Identity
{
  public  class RegisterModel
    {
        [Key]
        public int UserId { get; set; }

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
        [Display(Name = "Company")]
        public int? CompanyId { get; set; }
        [Display(Name = "Employee")]
        public int? EmployeeId { get; set; }
    }
}
