using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Code { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreationUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public int? CompanyId { get; set; }

    }
}
