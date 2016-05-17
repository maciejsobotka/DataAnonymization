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
            DataView v = new DataView(dt);
            string[] allRows = new string[pid.Length + 1];
            for (int i = 0; i < pid.Length; ++i)
                allRows[i] = pid[i];
            allRows[allRows.Length - 1] = s;
            int distPIDs = v.ToTable(true, pid).AsEnumerable().Count();
            int distRows = v.ToTable(true, allRows).AsEnumerable().Count();
               while (k > (dt.Rows.Count / distPIDs) || (double)distPIDs/distRows < a)
            {
                if (distPIDs == 1)
                    break;
                KAnonymizationStep(pid);
                distPIDs = v.ToTable(true, pid).AsEnumerable().Count();
                distRows = v.ToTable(true, allRows).AsEnumerable().Count();
            }

            return (double)distPIDs / distRows;
        }
    }
}
