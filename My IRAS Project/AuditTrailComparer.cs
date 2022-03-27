using System.Collections.Generic;

namespace My_IRAS_Project
{
    public class AuditTrailComparer : IComparer <AuditTrail>
    {
        public int Compare(AuditTrail x, AuditTrail y)
        {
            if(x.tablename == "AdjustmentDetails" && y.tablename == "AdjustmentDetails")
            {
               return x.datetime.CompareTo(y.datetime);
            }

            else
            {
                return 0;
            }

        }
    }
}
