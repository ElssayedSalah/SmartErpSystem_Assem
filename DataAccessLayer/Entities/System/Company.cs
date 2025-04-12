using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.System
{
    [Table("Companys", Schema = "System")]
    public class Company: BasicEntity
    {      
        /// <summary>
        /// الرقم الضريبي
        /// </summary>
        public string TaxNumber { get; set; }     
        /// <summary>
        /// رقم التسجيل في مصلحة الضرايب-رقم السجل التجاري
        /// </summary>
        public string TaxAuthorityRegestrationNumber { get; set; }
        /// <summary>
        /// كود نشاط الشركة
        /// </summary>
        public string TaxPayerActivityCode { get; set; }
        /// <summary>
        /// نو الشركة
        /// </summary>
        public string ClassType { get; set; }
        public string CountryCode { get; set; }
        public string Governate { get; set; }
        public string RegionCity { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }    
        /// <summary>
        /// رقم السجل التجاري
        /// </summary>
        public string CommercialNnumber { get; set; }  
        public bool ActivateEInvoice { get; set; }
        public string Client_ID { get; set; }
        public string Client_Secret { get; set; }
        public string TokenPassword { get; set; }
        public bool DefaultCompany { get; set; }
        public bool Activate_VAT_Tax { get; set; }
        public decimal VAT_Tax_Rate { get; set; }
        public string ImagePath { get; set; }     
        
    }
}
