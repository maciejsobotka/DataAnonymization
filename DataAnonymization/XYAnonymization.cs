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
            string[] allRows = new string[x.Length + y.Length];
            for (int i = 0; i < x.Length; ++i)
                allRows[i] = x[i];
            for (int i = x.Length; i < x.Length + y.Length; ++i)
                allRows[i] = y[i - x.Length];
            DataView v = new DataView(dt);
            int distYs = v.ToTable(true, y).AsEnumerable().Count();
            if (k > distYs) k = distYs;
            int smallGroup = 0;
            while (smallGroup < k)
            {
                DataTable Xs = v.ToTable(true, x);          // distinct X rows
                DataTable XYs = v.ToTable(true, allRows);   // distinct XY rows
                DataView v2 = new DataView(XYs);
                DataTable X2s = v2.ToTable(false, x);
                int[] groups = new int[Xs.Rows.Count];
                for (int i = 0; i < Xs.Rows.Count; ++i)
                    foreach (DataRow r in X2s.Rows)         // count rows in groups
                        if (r.ItemArray.SequenceEqual(Xs.Rows[i].ItemArray))
                            groups[i]++;
                smallGroup = groups.Min();
                if (smallGroup < k) KAnonymizationStep(x);
                if (v.ToTable(true, x).AsEnumerable().Count() == 1) break;
            }
            return smallGroup;
        }
    }
}
