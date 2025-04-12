using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.Financial
{
    [Table("FinancialPeriods", Schema = "Financial")]
    public class FinancialPeriod
    {
        [Key]
        public int Id { get; set; }
        public int Year { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }       
        public bool isClosed { get; set; }
        public DateTime CloseDate { get; set; }
    }
}
