using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_IRAS_Project.classes
{
    public class FinalSummaryModel
    {
        public List<PropertyDetail> propertyDetails { get; set; }

    }
    public class PropertyDetail
    {
        public Summary address { get; set; }
        public List<Summary> unitLevels { get; set; }
    }




}
