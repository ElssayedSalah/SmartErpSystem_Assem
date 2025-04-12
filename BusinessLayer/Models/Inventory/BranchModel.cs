using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Inventory
{
    public class BranchModel: BasicModel
    {
        [Display(Name = "TaxAuthorityCode")]
        public string TaxAuthorityCode { get; set; }
       
    }
}
