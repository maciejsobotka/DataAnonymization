using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnonymization
{
    class AKAnonymization : KAnonymization
    {
        public AKAnonymization(DataTable dt) : base(dt)
        {

        }

        public double AKAnonymize(string[] pid, string s, int k, double a)
        {
            if (k > dt.Rows.Count) k = dt.Rows.Count;

            string[] allRows = new string[pid.Length + 1];
            for (int i = 0; i < pid.Length; ++i)
                allRows[i] = pid[i];
            allRows[allRows.Length - 1] = s;

            DataView v = new DataView(dt);
            int smallK = 0;
            double bigA = 1.0;
            int distS = v.ToTable(true, s).AsEnumerable().Count();
            DataTable distSTable = v.ToTable(true, s);
            while (smallK < k || bigA > a)
            {
                DataTable PIDs = v.ToTable(true, pid);          // distinct PID rows
                DataTable allR = v.ToTable(false, allRows);     // all rows with s
                int[] kk = new int[PIDs.Rows.Count];
                double[] aa = new double[PIDs.Rows.Count];
                int[][] groups = new int[PIDs.Rows.Count][];
                for(int i=0; i < PIDs.Rows.Count; ++i)
                    groups[i] = new int[distS];
                for (int i = 0; i < PIDs.Rows.Count; ++i)
                {
                    foreach (DataRow r in allR.Rows)            // count rows in groups
                        for(int j = 0; j < distS; ++j)
                            if (PIDs.Rows[i].ItemArray.All(x => r.ItemArray.Contains(x))
                                && distSTable.Rows[j].ItemArray.All(x => r.ItemArray.Contains(x)))
                                groups[i][j]++;
                }
                for (int i = 0; i < PIDs.Rows.Count; ++i)
                {                                               // get k and a for each group
                    kk[i] = groups[i].Sum();
                    aa[i] = (double) groups[i].Max() / groups[i].Sum();
                }
                smallK = kk.Min();
                bigA = aa.Max();
                if (smallK < k || bigA > a) KAnonymizationStep(pid);
                if (groups.Length == 1) break;
            }
            return bigA;
        }
    }
}
