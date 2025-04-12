using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.Financial
{
    [Table("BankOpenBalance", Schema = "Financial")]
    public class BankOpenBalance
    {
        [Key]
        public int Id { get; set; }
        public int BankId { get; set; }
        public int? FinancialPeriodId { get; set; }
        public int? CompanyId { get; set; }
        public int? EntryNumber { get; set; }
        public int EntryId { get; set; }

        public decimal OpenBalanceDebit { get; set; }
        public decimal OpenBalanceCredit { get; set; }
    }
}
