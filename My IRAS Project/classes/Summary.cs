using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_IRAS_Project.classes
{
    internal class Summary : Values
    {
        public Summary(string title, string currentValue, string revisedValue)
            : base(title, currentValue, revisedValue)
        {
        }
    }
}
