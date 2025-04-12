using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.System
{
    [Table("Taxes", Schema = "System")]
    public class Taxes
    {
        [Key]
        public int Id { get; set; }
        public int TaxId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int AccountId { get; set; }
        public decimal Rate { get; set; }
        public string Type { get; set; }
        public string Name { get { return Thread.CurrentThread.CurrentCulture.Name == "ar" ? NameAr : NameEn; } }
    }
}
