using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Models.Inventory
{
   
    public class DepartmentModel : BasicModel
    {
        [Display(Name = "Branch")]
        public int? BranchId { get; set; }
        [Ignore]
        [Display(Name = "Branch")]
        public string BranchName { get; set; }
    }
}
