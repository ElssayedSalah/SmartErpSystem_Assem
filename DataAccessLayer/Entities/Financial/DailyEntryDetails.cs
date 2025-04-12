using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.Financial
{
    [Table("DailyEntryDetails", Schema = "Financial")]
   public class DailyEntryDetails
    {
        [Key]
        public int Id { get; set; }
        //[ForeignKey("Master_Details_FK")]
        [ForeignKey("DailyEntryMaster")]
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
        public DailyEntryMaster DailyEntryMaster { get; set; }


    }
}
