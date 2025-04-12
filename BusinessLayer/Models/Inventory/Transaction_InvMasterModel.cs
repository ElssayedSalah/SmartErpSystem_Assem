using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Inventory
{
    public class Transaction_InvMasterModel : TransactionModel
    {
        
        [Display(Name = "Store")]
        public int? StoreId { get; set; }
        [Display(Name = "Branch")]
        public int? BranchId { get; set; }
        [Display(Name = "Department")]
        public int? DepartementId { get; set; }
        [Display(Name = "SupplerName")]
        public int? SupplierId { get; set; }
        [Display(Name = "Customer")]
        public int? CustomerId { get; set; }
        [Display(Name = "CoastCenter")]
        public int? CoastCenterId { get; set; }
        [Display(Name = "EntryNumber")]
        public int? EntryNumber { get; set; }
        public int EntryId { get; set; }

        [Display(Name = "InvoiceValue")]
        public decimal InvoiceValue { get; set; }
        [Display(Name = "TotalAdditions")]
        public decimal TotalAdditions { get; set; }
        [Display(Name = "TotalDisounts")]
        public decimal TotalDiscounts { get; set; }
        [Display(Name = "InvoiceTotal")]
        public decimal InvoiceTotal { get; set; }
        [Display(Name = "TaxValue")]
        public decimal TaxValue { get; set; }
        [Display(Name = "InvoiceNet")]
        public decimal InvoiceNet { get; set; }
        [Display(Name = "VAT_Rate")]
        public decimal VAT_Rate { get; set; }
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }
        [Display(Name = "CurrencyFactor")]
        public  decimal CurrencyFactor { get; set; }
        [Display(Name = "UUID")]
        public string UUID { get; set; }
        [Display(Name = "InvoiceState")]
        public string InvoiceState { get; set; }
        [Display(Name = "DueDate")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Branch")]
        public string BranchName { get; set; }
        [Display(Name = "Store")]
        public string StoreName { get; set; }

        /// <summary>
        /// تاريخ بداية الجرد
        /// </summary>
        [Display(Name = "CountDateFrom")]
        public DateTime? CountDateFrom { get; set; }
        /// <summary>
        /// تاريخ نهاية الجرد
        /// </summary>
        [Display(Name = "CountDateTo")]
        public DateTime? CountDateTo { get; set; }
        /// <summary>
        /// قيمة العجز في المخزن بعد الجرد
        /// </summary>
        [Display(Name = "StoreDeficitValue")]
        public decimal StoreDeficitValue { get; set; }
        /// <summary>
        /// قيمة الفائض في المخزن بعد الجرد
        /// </summary>
        [Display(Name = "StoreSurplusValue")]
        public decimal StoreSurplusValue { get; set; }
        /// <summary>
        /// حساب العجز
        /// </summary>
        [Display(Name = "DeficitAccountId")]
        public int DeficitAccountId { get; set; }
        /// <summary>
        /// حساب الفائض
        /// </summary>
        [Display(Name = "SurplusAccountId")]
        public int SurplusAccountId { get; set; }
        /// <summary>
        /// رقم محضر الجرد المخزني
        /// </summary>
        [Display(Name = "StoreCountNumber")]
        public int StoreCountId { get; set; }
        [Display(Name = "StoreCountNumber")]
        [Ignore]
        public int StoreCountNumber { get; set; }
        public int TransactionType { get; set; }

        public List<Transaction_InvDetailsModel> Transaction_InvDetails { get; set; }
        public Transaction_InvMasterModel()
        {
            Transaction_InvDetails = new List<Transaction_InvDetailsModel>() { new Transaction_InvDetailsModel() {} };
        }

    }
}
