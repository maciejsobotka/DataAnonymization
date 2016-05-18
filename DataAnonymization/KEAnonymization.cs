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

        public int KEAnonymize(string[] pid, string s, int k, int e)
        {
            if (k > dt.Rows.Count) k = dt.Rows.Count;
            DataView v = new DataView(dt);
            int distPIDs = v.ToTable(true, pid).AsEnumerable().Count();
            while (k > (dt.Rows.Count / distPIDs))
            {
                if (distPIDs == 1)
                    break;
                KAnonymizationStep(pid);
                distPIDs = v.ToTable(true, pid).AsEnumerable().Count();
            }
            DataTable distPid = v.ToTable(true, pid);
            DataTable allPid = v.ToTable(false, pid);
            List<int> indexes = new List<int>();
            foreach (DataRow r in distPid.Rows)
            {
                indexes.Clear();
                for (int i = 0; i < allPid.Rows.Count; ++i)    // get indexes with same PID
                    if (allPid.Rows[i].ItemArray.SequenceEqual(r.ItemArray))
                        indexes.Add(i);
                EAnonymizationStep(indexes, s, e);
            }

            return (dt.Rows.Count / distPIDs);
        }

        private void EAnonymizationStep(List<int> rows, string s, int e)
        {
            DataView v = new DataView(dt);
            DataTable tb = v.ToTable(false, s);
            List<int> values = new List<int>();
            int idx = dt.Columns.IndexOf(s);
            foreach(var row in rows)
            {                                   // get values for PID type
                values.Add(Int32.Parse(dt.Rows[row][idx].ToString()));
            }
            int max = values.Max();
            int min = values.Min();
            if (max - min > e)                  // check if need to change values
            {
                int interval = (max - min) - e;
                Random rnd = new Random();      // values changed into intervals
                for(int i = 0; i < rows.Count; ++i)
                {
                    int x = rnd.Next(interval);
                    if (values[i] == max)
                        if(x > (interval - x))
                            dt.Rows[rows[i]][idx] = "[" + (values[i] - x).ToString()
                                + ", " + (values[i] + (interval - x)).ToString() + "]";
                        else
                            dt.Rows[rows[i]][idx] = "[" + (values[i] - (interval - x)).ToString()
                                + ", " + (values[i] + x).ToString() + "]";

                    else if (values[i] == min)
                            if (x > (interval - x))
                                dt.Rows[rows[i]][idx] = "[" + (values[i] - (interval - x)).ToString()
                                    + ", " + (values[i] + x).ToString() + "]";
                            else
                                dt.Rows[rows[i]][idx] = "[" + (values[i] - x).ToString()
                                    + ", " + (values[i] + (interval - x)).ToString() + "]";

                    else
                        dt.Rows[rows[i]][idx] = "[" + (values[i] - x).ToString()
                            + ", " + (values[i] + (interval - x)).ToString() + "]";
                }
            }
        }
    }
}
