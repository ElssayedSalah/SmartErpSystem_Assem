using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace BusinessLayer.Models.Financial
{
    public class DefaultAccountModel
    {
        [Key]
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string GroupNameAr { get; set; }
        public string GroupNameEn { get; set; }
        public int AccountNameId { get; set; }
        public string AccountNameAr { get; set; }
        public string AccountNameEn { get; set; }
        public int AccountId { get; set; }
        [Display(Name = "Name")]
        public string AccountName { get { return Thread.CurrentThread.CurrentCulture.Name == "ar" ? AccountNameAr : AccountNameEn; } }

    }
    public class DefaultAccountsGroups
    {        
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<DefaultAccountModel> DefaultAccountModel { get; set; }
        public DefaultAccountsGroups()
        {
            DefaultAccountModel = new List<DefaultAccountModel>();
        }
    }

    public class DefaultAccountsDto
    {
       
        public List<DefaultAccountsGroups> DefaultAccountsGroups { get; set; }        
        public List<DefaultAccountModel> DefaultAccountModel { get; set; }
        public DefaultAccountsDto()
        {
            DefaultAccountModel = new List<DefaultAccountModel>();
            
            DefaultAccountsGroups = new List<DefaultAccountsGroups>()
            {
                new DefaultAccountsGroups(){ GroupId=1, GroupName="الحسابات العامة" },
                new DefaultAccountsGroups(){ GroupId=2, GroupName="العملاء والموردين" },
                new DefaultAccountsGroups(){ GroupId=3, GroupName="الإيرادات والمصروفات" },
                new DefaultAccountsGroups(){ GroupId=4, GroupName="الضرائب" },
                new DefaultAccountsGroups(){ GroupId=5, GroupName="قائمة الدخل" },

            };

        }
    }

}
