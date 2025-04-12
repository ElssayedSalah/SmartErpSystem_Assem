using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.Financial
{
    [Table("AccountOpenBalance", Schema = "Financial")]
    public class AccountOpenBalance
    {
        [Key]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal OpenBalanceDebit { get; set; }
        public decimal OpenBalanceCredit { get; set; }
        public decimal TransactionsBalanceDebit { get; set; }
        public decimal TransactionsBalanceCredit { get; set; }
        public decimal CurrentBalanceDebit { get; set; }
        public decimal CurrentBalanceCredit { get; set; }
        public int FinancialPeriodId { get; set; }
        public int CompanyId { get; set; }
    }
}
