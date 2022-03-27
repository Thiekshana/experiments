using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace My_IRAS_Project.classes
{
    public static class Compare
    {
        public static List<Variance> DetailedCompare<T>(this T val1, T val2)
        {
            List<Variance> variances = new List<Variance>();
            var fi = val1.GetType().GetProperties();
            foreach (PropertyInfo f in fi)
            {
                Variance v = new Variance();
                v.Prop = f.Name;
                v.ValA = f.GetValue(val1);
                v.ValB = f.GetValue(val2);
                if (!Equals(v.ValA, v.ValB))
                    variances.Add(v);
            }

            return variances;

        }
    }

    public class Variance
    {
        public string Prop { get; set; }
        public object ValA { get; set; }
        public object ValB { get; set; }
    }

}
