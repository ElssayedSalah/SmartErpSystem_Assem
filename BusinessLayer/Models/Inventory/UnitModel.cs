using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Models.Inventory
{
    public class UnitModel : BasicModel
    {
        [Display(Name = "TaxAuthorityCode")]
        public string TaxAuthorityCode { get; set; }
    }
}
