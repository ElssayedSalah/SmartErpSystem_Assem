using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.Financial
{
    [Table("TreasuryOpenBalance", Schema = "Financial")]
    public class TreasuryOpenBalance
    {
        [Key]
        public int Id { get; set; }
        public int TreasuryId { get; set; }
        public int? FinancialPeriodId { get; set; }
        public int? CompanyId { get; set; }
        public int? EntryNumber { get; set; }
        public int EntryId { get; set; }
        public decimal OpenBalanceDebit { get; set; }
        public decimal OpenBalanceCredit { get; set; }
    }
}
