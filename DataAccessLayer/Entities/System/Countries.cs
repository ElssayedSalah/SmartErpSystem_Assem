using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

namespace DataAccessLayer.Entities.System
{
    [Table("Countries", Schema = "System")]
    public class Countries
    {
        [Key]
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Code { get; set; }     
        public string Name { get { return Thread.CurrentThread.CurrentCulture.Name=="ar"? NameAr: NameEn; } }


    }
}
