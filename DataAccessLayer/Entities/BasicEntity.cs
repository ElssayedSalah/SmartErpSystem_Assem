using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    public class BasicEntity:Entity
    {        
        [Required]
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool? ActivationState { get; set; }
        public string Notes { get; set; }
        public int? FinancialPeriodId { get; set; }





    }
}
