using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_IRAS_Project.classes
{
    internal class TransactionDto
    {
        public string FileRefereneNo { get; set; }

        public string DocumentRefNo { get; set; }

        public bool IsSignedSingapore { get; set; }

        public bool IsPhysicalDocument { get; set; }
  
        public DateTimeOffset DateDocument { get; set; }

        public DateTimeOffset NotifactionDate { get; set; }

    }
}
