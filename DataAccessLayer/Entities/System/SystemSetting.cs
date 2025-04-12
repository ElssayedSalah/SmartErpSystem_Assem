using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.System
{
    [Table("SystemSettings", Schema = "System")]
    public class SystemSetting
    {
        [Key]
        public int Id { get; set; }
        public bool ActivateE_Invoice { get; set; }
        public string IdentityService_Url { get; set; }
        public string System_APIUrl { get; set; }       
        public bool SendE_InvoiceDirectly { get; set; }
        public int E_InvoiceType { get; set; }
        /// <summary>
        /// تفعيل انشاء القيود الافتتاحية
        /// </summary>
        public bool EnableOpenEntryCreation { get; set; }
        public bool EnableTransactionsEntryCreation { get; set; }

    }
}
