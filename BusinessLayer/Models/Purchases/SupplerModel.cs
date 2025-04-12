using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Purchases
{
    public class SupplerModel : BasicModel
    {
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Tel")]
        public string Tel { get; set; }
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Fax")]
        public string Fax { get; set; }
        [Display(Name = "Account")]
        public int AccountId { get; set; }
        [Display(Name = "OpeningBalanceDebit")]
        public decimal OpeningBalanceDebit { get; set; }
        [Display(Name = "OpeningBalanceCredit")]
        public decimal OpeningBalanceCredit { get; set; }
        [Display(Name = "EntryNumber")]
        public int EntryNumber { get; set; }
        [Display(Name = "TaxNumber")]
        public string TaxRegesterationNumber { get; set; }
        [Display(Name = "NationalID")]
        public string NationalID { get; set; }
        [Display(Name = "ClassType")]
        public string ClassType { get; set; }
        [Display(Name = "Country")]
        public string CountryCode { get; set; }
        [Display(Name = "Governate")]
        public string Governate { get; set; }
        [Display(Name = "RegionCity")]
        public string RegionCity { get; set; }
        [Display(Name = "Street")]
        public string Street { get; set; }
        [Display(Name = "BuildingNumber")]
        public string BuildingNumber { get; set; }
        [Display(Name = "IDType")]
        public string IDType { get; set; }
    }
}
