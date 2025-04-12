using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.Financial
{
    public class TransactionsEntrySettingDetailsModel
    {
        [Key]
        public int Id { get; set; }
        public int MasterId { get; set; }
        /// <summary>
        /// طرف اول او طرف ثاني
        /// </summary>
        public int SideTypeId { get; set; }
        /// <summary>
        /// طرف القيد حساب او مورد او عميل او بنك او خزنة او موظف 
        /// </summary>

        public int SideId { get; set; }
        /// <summary>
        /// طبيعة الطرف مدين او داين
        /// </summary>
        public int SideNaturalId { get; set; }
    }
}
