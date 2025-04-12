using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.Identity
{
  public class LoginModel
    {
        [Required(ErrorMessage = "UserNameValidationMsg")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "PasswordValidationMsg")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }

        [Display(Name = "FinancialPeriod")]
        public int? FinancialPeriodId { get; set; }
    }
}
