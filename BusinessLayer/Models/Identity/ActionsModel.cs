using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.Identity
{
   public class ActionsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public bool Allowed { get; set; }

    }
}
