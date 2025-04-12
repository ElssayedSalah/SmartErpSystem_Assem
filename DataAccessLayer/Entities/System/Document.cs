using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.System
{
    [Table("Documents", Schema = "System")]
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int DocSign { get; set; }       
        public string DocType { get; set; }
        public int DocTypeId { get; set; }
    }
}
