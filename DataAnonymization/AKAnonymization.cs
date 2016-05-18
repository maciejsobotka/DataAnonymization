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

        //       {
        //    if (k > dt.Rows.Count) k = dt.Rows.Count;
        //    DataView v = new DataView(dt);
        //    int smallGroup = 0;
        //    while (smallGroup < k)
        //    {
        //        DataTable allPIDs = v.ToTable(false, pid);  // all rows
        //        DataTable PIDs = v.ToTable(true, pid);      // distinct rows
        //        int[] groups = new int[PIDs.Rows.Count];
        //        for (int i = 0; i < PIDs.Rows.Count; ++i)
        //            foreach (DataRow r in allPIDs.Rows)     // count rows in groups
        //                if (r.ItemArray.SequenceEqual(PIDs.Rows[i].ItemArray))
        //                    groups[i]++;
        //        smallGroup = groups.Min();
        //        if (smallGroup < k) KAnonymizationStep(pid);
        //    }
        //    return smallGroup;
        //}
        public double AKAnonymize(string[] pid, string s, int k, double a)
        {
            if (k > dt.Rows.Count) k = dt.Rows.Count;

            string[] allRows = new string[pid.Length + 1];
            for (int i = 0; i < pid.Length; ++i)
                allRows[i] = pid[i];
            allRows[allRows.Length - 1] = s;

            DataView v = new DataView(dt);
            int smallGroup = 0;
            double bigA = 1.0;
            while (smallGroup < k || bigA > a)
            {
                DataTable PIDs = v.ToTable(true, pid);          // distinct PID rows
                DataTable distAll = v.ToTable(true, allRows);   // distinct all rows
                DataView v2 = new DataView(distAll);
                DataTable PID2s = v2.ToTable(false, pid);
                DataTable allPID2s = v.ToTable(false, pid);
                int[] groups = new int[PIDs.Rows.Count];
                int[] groups2 = new int[PIDs.Rows.Count];
                for (int i = 0; i < PIDs.Rows.Count; ++i){
                    foreach (DataRow r in PID2s.Rows)     // count rows in groups
                        if (r.ItemArray.SequenceEqual(PIDs.Rows[i].ItemArray))
                            groups[i]++;
                    foreach (DataRow r in allPID2s.Rows)     // count rows in groups
                        if (r.ItemArray.SequenceEqual(PIDs.Rows[i].ItemArray))
                            groups2[i]++;
                }
                double[] aS = new double[PIDs.Rows.Count];
                for (int i = 0; i < PIDs.Rows.Count; ++i)
                {
                    aS[i] = (double)((groups2[i]-groups[i]+1)/(double)groups2[i]);
                }
                smallGroup = groups.Min();
                bigA = aS.Max();
                if (smallGroup < k || bigA > a) KAnonymizationStep(pid);
                if (groups.Length == 1) break;
            }
            return bigA;
        }
    }
}
