using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class Model
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Code")]
        [Required(ErrorMessage = "CodeRequired")]
        public int Code { get; set; }
        [Display(Name = "CreationDate")]
        public DateTime? CreationDate { get; set; }
        [Display(Name = "UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }
        [Display(Name = "CreationUserId")]
        public string CreationUserId { get; set; }
        [Display(Name = "UpdatedUserId")]
        public string UpdatedUserId { get; set; }
        [Display(Name = "Company")]     
        public int? CompanyId { get; set; }

    }
}
