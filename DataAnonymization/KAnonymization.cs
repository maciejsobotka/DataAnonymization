using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnonymization
{
    class KAnonymization
    {
        protected DataTable dt;
        protected DataReplaceTree drt;
        protected Func<DataRow, DataRow, bool> EqualRow;
        public KAnonymization(DataTable dt)
        {
            this.EqualRow = (r, r2) => r.ItemArray.SequenceEqual(r2.ItemArray);
            this.dt = dt;
            this.drt = new DataReplaceTree();
        }

        public int KAnonymize(string[] pid,int k)
        {
            if (k > dt.Rows.Count) k = dt.Rows.Count;
            DataView v = new DataView(dt);
            int smallK = 0;
            while (smallK < k)
            {
                DataTable allPIDs = v.ToTable(false, pid);  // all rows
                DataTable PIDs = v.ToTable(true, pid);      // distinct rows
                int[] groups = new int[PIDs.Rows.Count];
                for (int i = 0; i < PIDs.Rows.Count; ++i)
                {
                    groups[i] = allPIDs.AsEnumerable()
                        .GroupBy(r => EqualRow(r, PIDs.Rows[i]))
                        .Select(grp => grp.Count()).Min();
                }
                smallK = groups.Min();

                if (smallK < k) KAnonymizationStep(pid);
            }
            return smallK;
        }

        protected void KAnonymizationStep(string[] pid)
        {
            List<int> dist = new List<int>();
            DataView v = new DataView(dt);
            // distincts in PID columns
            foreach (string col in pid)
                dist.Add(v.ToTable(true, col).AsEnumerable().Count());
            int max = dist.Max();
            // we change only column with biggest distinction
            int indx = dist.IndexOf(max);
            foreach (DataRow row in dt.Rows)
                // change value with a higher one in tree
                row[indx] = drt.DataReplace(row[indx]);
        }
    }
}
