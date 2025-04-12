using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.Financial
{
    [Table("CheckTransaction", Schema = "Financial")]
    public class CheckTransaction : TransactionEntity
    {
        public int BranchId { get; set; }
        public int EntryNumber { get; set; }
        public int EntryId { get; set; }      
        public string CheckNumber { get; set; }
        public string CheckOwner { get; set; }
        public int BankId { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankBranchName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public decimal DiscountAmount { get; set; }
        public int CurrencyId { get; set; }
        public decimal CurrencyFactor { get; set; }
        public int CostCenterId { get; set; }
        public int DiscountAccountId { get; set; }  
        public int FirstSideTypeId { get; set; }
        public int SecondSideTypeId { get; set; }
        public int FirstSideId { get; set; }
        public int SecondSideId { get; set; }
        public int FirstSideAccountId { get; set; }
        public int SecondSideAccountId { get; set; }



    }
}
