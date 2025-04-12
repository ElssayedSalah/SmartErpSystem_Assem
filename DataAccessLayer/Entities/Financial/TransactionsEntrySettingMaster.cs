using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.Financial
{
    [Table("TransactionsEntrySettingMaster", Schema = "Financial")]
   public class TransactionsEntrySettingMaster
    {
        [Key]
        public int Id { get; set; }        
        /// <summary>
        /// رقم الحركة
        /// </summary>
        public int DocTypeId { get; set; }
        /// <summary>
        /// نوع اليومية
        /// </summary>
        public int DailyTypeId { get; set; }
        /// <summary>
        /// حالة ترحيل القيد
        /// </summary>
        public bool TransferState { get; set; }

        /// <summary>
        /// الطرف الاول للقيد عبارة حساب او مورد او عميل او بنك او خزنة او موظف 
        /// </summary>
        public int FirstSide { get; set; }
        /// <summary>
        /// الطرف الثاني للقيد عبارة حساب او مورد او عميل او بنك او خزنة او موظف 
        /// </summary>
        public int SecondSide { get; set; }

        public int FirstSideSideNaturalId { get; set; }
        public int SecondSideSideNaturalId { get; set; }
        public bool IsFirstSideTaxble { get; set; }
        public bool IsSecondSideTaxble { get; set; }
     

    }
}
