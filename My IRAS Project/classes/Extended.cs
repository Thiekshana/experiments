using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_IRAS_Project.classes
{
    internal class Extended
    {
        public List<Summary> Summary { get; set; } = new List<Summary>();
        public string Identifier { get; set; }

        public Extended()
        {

        }
    }
}
