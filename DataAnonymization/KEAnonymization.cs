using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnonymization
{
    class KEAnonymization : KAnonymization
    {
        public KEAnonymization(DataTable dt) : base(dt)
        {

        }

        public double KEAnonymize(string[] pid, string s, int k, double a)
        {
            if (k > dt.Rows.Count) k = dt.Rows.Count;
            DataView v = new DataView(dt);
            int distPIDs = v.ToTable(true, pid).AsEnumerable().Count();
            DataView v2 = new DataView(dt);
            int distSs = v2.ToTable(true, s).AsEnumerable().Count();
               while (k > (dt.Rows.Count / distPIDs) || (double)distSs/(dt.Rows.Count/distPIDs) < a)
            {
                if (distPIDs == 1)
                    break;
                KAnonymizationStep(pid);
                distPIDs = v.ToTable(true, pid).AsEnumerable().Count();
            }

            return (double)distSs / (dt.Rows.Count / distPIDs);
        }
    }
}
