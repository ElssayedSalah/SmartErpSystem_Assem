using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class TransactionEntity : Entity
    {
        public int? DocTypeId { get; set; }
        public DateTime DocDate { get; set; }
        public int? FinancialPeriodId { get; set; }
        public string Notes { get; set; }





    }
}
