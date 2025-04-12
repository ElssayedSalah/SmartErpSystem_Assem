using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.Inventory
{
    [Table("ItemOpenBalance", Schema = "Inventory")]
    public class ItemOpenBalance
    {
        [Key]
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int StoreId { get; set; }
        public int BranchId { get; set; }
        public int FinancialPeriodId { get; set; }
        public int CompanyId { get; set; }
        public decimal PostedBalance { get; set; }
        public decimal CurruntBalance { get; set; }
        public decimal TotalBalance { get { return PostedBalance + CurruntBalance; } set {value= PostedBalance + CurruntBalance; } }

    }
}
