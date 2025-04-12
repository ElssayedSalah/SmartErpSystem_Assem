using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.Financial
{
    [Table("Treasurys", Schema = "Financial")]
    public class Treasury : BasicEntity
    {
        public string BranchId { get; set; }
        public int AccountId { get; set; }
        public int CurrencyId { get; set; }
        public decimal OpenBalanceDebit { get; set; }
        public decimal OpenBalanceCredit { get; set; }
        public int EntryNumber { get; set; }
        public int EntryId { get; set; }
    }
}
