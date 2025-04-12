using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities.Purchases
{
    [Table("Supplers", Schema = "Purchases")]
   public class Suppler : BasicEntity
    {
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public int AccountId { get; set; }
        public decimal OpeningBalanceDebit { get; set; }
        public decimal OpeningBalanceCredit { get; set; }
        public int EntryNumber { get; set; }
        public int EntryId { get; set; }

        public string TaxRegesterationNumber { get; set; }
        public string NationalID { get; set; }
        public string ClassType { get; set; }
        public string CountryCode { get; set; }
        public string Governate { get; set; }
        public string RegionCity { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string IDType { get; set; }
    }
}
