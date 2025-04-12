using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.Financial
{
    [Table("DefaultAccounts", Schema = "Financial")]
   public class DefaultAccount
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

    }
}
