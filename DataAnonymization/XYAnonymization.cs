using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnonymization
{
    class XYAnonymization : KAnonymization
    {
        public XYAnonymization(DataTable dt) : base(dt)
        {

        }

        public int xyAnonymize(string[] x, string[] y, int k)
        {
            if (k > dt.Rows.Count) k = dt.Rows.Count;
            DataView v1 = new DataView(dt);
            int distXs = v1.ToTable(true, x).AsEnumerable().Count();
            DataView v2 = new DataView(dt);
            int distYs = v2.ToTable(true, y).AsEnumerable().Count();
            if (k > distYs) k = distYs;
            while (k > (distYs / distXs))
            {
                if (distXs == 1)
                    break;
                KAnonymizationStep(x);
                distXs = v1.ToTable(true, x).AsEnumerable().Count();
            }

            return distYs / distXs;
        }
    }
}
