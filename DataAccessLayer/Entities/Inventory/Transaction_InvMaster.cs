using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.Inventory
{
    [Table("Transaction_InvMaster", Schema = "Transactions")]
    public class Transaction_InvMaster : TransactionEntity
    {       
        public int? StoreId { get; set; }
        public int? BranchId { get; set; }
        public int? DepartementId { get; set; }
        public int? SupplierId { get; set; }
        public int? CustomerId { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal TotalAdditions { get; set; }
        public decimal TotalDisounts { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal TaxValue { get; set; }
        public decimal InvoiceNet { get; set; }
        public DateTime DueDate { get; set; }
        public decimal VAT_Rate { get; set; }
        public int CurrencyId { get; set; }
        public  decimal CurrencyFactor { get; set; }
        public int? EntryNumber { get; set; }
        public int EntryId { get; set; }

        public int? CoastCenterId { get; set; }
        public string UUID { get; set; }
        public string InvoiceState { get; set; }
        public DateTime? CountDateFrom { get; set; }       
        public DateTime? CountDateTo { get; set; }       
        public decimal StoreDeficitValue { get; set; }       
        public decimal StoreSurplusValue { get; set; }       
        public int DeficitAccountId { get; set; }
        public int SurplusAccountId { get; set; }       
        public int StoreCountId { get; set; }
        public int TransactionType { get; set; }

        public List<Transaction_InvDetails> Transaction_InvDetails { get; set; }
        public Transaction_InvMaster()
        {
            Transaction_InvDetails = new List<Transaction_InvDetails>();
        }

    }
}
