using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_IRAS_Project
{
    public class AuditTrail
    {
        public int id { get; set; }
        public DateTimeOffset datetime { get; set; }
        public string tablename { get; set; }
    }
}
