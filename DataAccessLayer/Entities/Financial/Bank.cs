using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.Financial
{
    [Table("Banks", Schema = "Financial")]
    public class Bank :BasicEntity
    {       
        public string AccountNumber { get; set; }
        public string BranchName { get; set; }
        public int AccountId { get; set; }
        public int CurrencyId { get; set; }
        public string Address { get; set; }       
        public decimal OpenBalanceDebit { get; set; }
        public decimal OpenBalanceCredit { get; set; }  
        public int EntryNumber { get; set; }
        public int EntryId { get; set; }

    }
}
