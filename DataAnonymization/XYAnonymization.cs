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

        public int XYAnonymize(string[] x, string[] y, int k)
        {
            if (k > dt.Rows.Count) k = dt.Rows.Count;
            string[] allRows = new string[x.Length + y.Length];
            for (int i = 0; i < x.Length; ++i)
                allRows[i] = x[i];
            for (int i = x.Length; i < x.Length + y.Length; ++i)
                allRows[i] = y[i - x.Length];
            DataView v = new DataView(dt);
            int distXs = v.ToTable(true, x).AsEnumerable().Count();
            int distYs = v.ToTable(true, y).AsEnumerable().Count();
            int distXYs = v.ToTable(true, allRows).AsEnumerable().Count();
            if (k > distYs) k = distYs;
            while (k > (distXYs / distXs))
            {
                if (distXs == 1)
                    break;
                KAnonymizationStep(x);
                distXs = v.ToTable(true, x).AsEnumerable().Count();
                distXYs = v.ToTable(true, allRows).AsEnumerable().Count();
            }

            return distXYs / distXs;
        }
    }
}
