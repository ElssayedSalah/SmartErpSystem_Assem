using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.Financial
{
    class AccountOpenBalanceModel
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
