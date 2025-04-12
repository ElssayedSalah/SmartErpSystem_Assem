using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.Financial
{
    [Table("ChartOfAccountSettings", Schema = "Financial")]
    public class ChartOfAccountSettings
    {
        [Key]
        public int Id { get; set; }
        public int AccountSettingsCount { get; set; }
        public int AccountSettingsTotalLength { get; set; }
        public List<AccountSetting> Settings { get; set; }
        public ChartOfAccountSettings()
        {
            Settings = new List<AccountSetting>();
        }
    }
    [Table("AccountSetting", Schema = "Financial")]
    public class AccountSetting
    {
        [Key]
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Color { get; set; }
        public int Length { get; set; }
    }
}
