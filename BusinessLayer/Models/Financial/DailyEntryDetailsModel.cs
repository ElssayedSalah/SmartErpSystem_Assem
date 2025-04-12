using AutoMapper.Configuration.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Financial
{
    public class DailyEntryDetailsModel
    {
        [Key]
        public int Id { get; set; }
        public int MasterId { get; set; }
        public int MasterEntryNumber { get; set; }
        public int AccountId { get; set; }
        public int CostCenterId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal DefaultCurrencyDebit { get; set; }
        public decimal DefaultCurrencyCredit { get; set; }
        public int FinancialPeriodId { get; set; }
        public int CompanyId { get; set; }
        public string Notes { get; set; }
        [Ignore]
        public string AccountName { get; set; }
    }
}
