using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_IRAS_Project.classes
{
    internal class PropertyCardDto
    {
      
        public string Address { get; set; }
       
        public string number { get; set; }
     
        public string date { get; set; }
      
        public string myEnum { get; set; }

        public List<string> Moms { get; set; }
      
        public string IsMyEnum { get; set; }

    }
}
