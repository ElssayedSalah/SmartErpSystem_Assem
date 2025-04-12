using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Financial
{
   public class ChartOfAccountSettingsModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "AccountSettingsCount")]
        public int AccountSettingsCount { get; set; }
        [Display(Name = "AccountSettingsTotalLength")]
        public int AccountSettingsTotalLength { get; set; }
        public List<AccountSettingModel> AccountSettingsModel { get; set; }
        public ChartOfAccountSettingsModel()
        {
            AccountSettingsModel = new List<AccountSettingModel>() { new AccountSettingModel() };
        }
    }
    public class AccountSettingModel
    {
        [Key]
        public int Id { get; set; }       
        [Required(ErrorMessage = "NameArRequired")]
        public string NameAr { get; set; }        
        public string NameEn { get; set; }
        public string Color { get; set; }
        public int Length { get; set; }
    }
}
