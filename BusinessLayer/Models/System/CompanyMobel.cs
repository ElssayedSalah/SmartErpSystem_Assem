using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.System
{
    public class CompanyMobel: BasicModel
    {
        [Display(Name = "TaxNumber")]
        public string TaxNumber { get; set; }
        [Display(Name = "TaxAuthorityRegestrationNumber")]
        public string TaxAuthorityRegestrationNumber { get; set; }
        [Display(Name = "TaxPayerActivityCode")]
        public string TaxPayerActivityCode { get; set; }
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
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Display(Name = "Fax")]
        public string Fax { get; set; }
        [Display(Name = "CommercialNnumber")]
        public string CommercialNnumber { get; set; }
        [Display(Name = "ActivateEInvoice")]
        public bool ActivateEInvoice { get; set; }
        [Display(Name = "Client_ID")]
        public string Client_ID { get; set; }
        [Display(Name = "Client_Secret")]
        public string Client_Secret { get; set; }
        [Display(Name = "TokenPassword")]
        public string TokenPassword { get; set; }
        [Display(Name = "DefaultCompany")]
        public bool DefaultCompany { get; set; }
        [Display(Name = "ItemImagePath")]
        public string ImagePath { get; set; }
        [Display(Name = "Activate_VAT_Tax")]
        public bool Activate_VAT_Tax { get; set; }
        [Display(Name = "VAT_Tax_Rate")]
        public decimal VAT_Tax_Rate { get; set; }

        [Ignore]
        [Display(Name = "ItemImageFile")]
        public IFormFile ImageFile { get; set; }
    }
}
