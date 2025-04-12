using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.Purchases
{
    [Table("SupplerOpenBalance", Schema = "Purchases")]
   public class SupplerOpenBalance
    {
        [Key]
        public int Id { get; set; }
        public int SupplerId { get; set; }
        public decimal OpeningBalanceDebit { get; set; }
        public decimal OpeningBalanceCredit { get; set; }
        public int AccountId { get; set; }
        public int? FinancialPeriodId { get; set; }
        public int? CompanyId { get; set; }
        public int? EntryNumber { get; set; }
        public int EntryId { get; set; }

    }
}
