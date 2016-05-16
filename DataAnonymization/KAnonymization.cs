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
        protected Hashtable dataReplace;
        protected DataTable dt;
        public KAnonymization(DataTable dt){
            dataReplace = new Hashtable();
            dataReplace.Add("*", "*");
            dataReplace.Add("M", "*");
            dataReplace.Add("K", "*");
            dataReplace.Add("Inżynier", "Techniczny");
            dataReplace.Add("Techniczny", "*");
            dataReplace.Add("Malarz", "Artystyczny");
            dataReplace.Add("Tancerz", "Artystyczny");
            dataReplace.Add("Muzyk", "Artystyczny");
            dataReplace.Add("Śpiewak", "Artystyczny");
            dataReplace.Add("Artystyczny", "*");
            dataReplace.Add("Kraków", "Małopolskie");
            dataReplace.Add("Małopolskie", "*");
            dataReplace.Add("Opole", "Opolskie");
            dataReplace.Add("Brzeg", "Opolskie");
            dataReplace.Add("Opolskie", "*");

            this.dt = dt;
        }

        public float KAnonymize(string[] pid,int k)
        {
            if (k > dt.Rows.Count)
                k = dt.Rows.Count;
            DataView v = new DataView(dt);
            int distPIDs = v.ToTable(true, pid).AsEnumerable().Count();
            while (k > (dt.Rows.Count / distPIDs))
            {
                KAnonymizationStep(pid);
                distPIDs = v.ToTable(true, pid).AsEnumerable().Count();
            }

            return (float)dt.Rows.Count / distPIDs;
        }

        private void KAnonymizationStep(string[] pid)
        {
            foreach (DataRow row in dt.Rows)
                foreach(string col in pid)
                    // change value with a higher one in tree
                    row[col] = dataReplace[row[col]];
        }
    }
}
