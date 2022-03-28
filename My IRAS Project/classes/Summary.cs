using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_IRAS_Project.classes
{
    public class Summary : SummaryViewModel
    {
        string Title { get; set; }
        string CurrentValue { get; set; }
        string RevisedValue { get; set; }
        public Summary(string title, string currentValue, string revisedValue)
            : base(title, currentValue, revisedValue)
        {
            Title = title;
            CurrentValue = currentValue;
            RevisedValue = revisedValue;
        }
    }
}
