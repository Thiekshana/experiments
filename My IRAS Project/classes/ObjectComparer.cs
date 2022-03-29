using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_IRAS_Project.classes
{
    internal class ObjectComparer : IEqualityComparer<Property>
    {
        public bool Equals(Property x, Property y)
        {
            if (x.Address == y.Address &&
                x.Units == y.Units)
                return true;

                return false;
        }

        public int GetHashCode(Property obj)
        {
            return obj.GetHashCode();
        }
    }
}
