using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

namespace DataAccessLayer.Entities.Financial
{
    [Table("Accounts", Schema = "Financial")]
    public class Account
    {
        [Key]
        public int Id { get; set; }       
        public int? ParentId { get; set; }       
        public string AccountCode { get; set; }       
        public string NameAr { get; set; }       
        public string NameEn { get; set; }       
        public DateTime OpenDate { get; set; }      
        public int AccountNatureId { get; set; }      
        public int CurrencyId { get; set; }
        public decimal CurrencyRate { get; set; }
        public bool ConnectedToCostCenter { get; set; }       
        public bool LastLevelInTree { get; set; }
        public int Level { get; set; }      
        public int? PostTo { get; set; }
        public int? PostType { get; set; }    
        public int? JournalId { get; set; }
        public decimal OpenBalanceCredit { get; set; }
        public decimal OpenBalanceDebit { get; set; }
        public decimal CurrentBalanceDebit { get; set; }
        public decimal CurrentBalanceCredit { get; set; }
        public decimal TransactionsBalanceDebit { get; set; }
        public decimal TransactionsBalanceCredit { get; set; }
        public string Name { get { return Thread.CurrentThread.CurrentCulture.Name == "ar" ? NameAr : NameEn; } }

    }
}
