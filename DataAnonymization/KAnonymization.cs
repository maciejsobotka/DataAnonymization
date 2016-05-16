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
            // All
            dataReplace.Add("*", "*");
            // Płeć
            dataReplace.Add("M", "*");
            dataReplace.Add("K", "*");
            // Zawód
            dataReplace.Add("Inżynier", "Techniczny");
            dataReplace.Add("Techniczny", "*");
            dataReplace.Add("Malarz", "Artystyczny");
            dataReplace.Add("Tancerz", "Artystyczny");
            dataReplace.Add("Muzyk", "Artystyczny");
            dataReplace.Add("Śpiewak", "Artystyczny");
            dataReplace.Add("Artystyczny", "*");
            // Miasto
            dataReplace.Add("Kraków", "Małopolskie");
            dataReplace.Add("Małopolskie", "*");
            dataReplace.Add("Opole", "Opolskie");
            dataReplace.Add("Brzeg", "Opolskie");
            dataReplace.Add("Opolskie", "*");
            dataReplace.Add("Wrocław", "Dolnośląskie");
            dataReplace.Add("Oława", "Dolnośląskie");
            dataReplace.Add("Dolnośląskie", "*");
            dataReplace.Add("Poznań", "Wielkopolskie");
            dataReplace.Add("Wielkopolskie", "*");
            dataReplace.Add("Warszawa", "Mazowieckie");
            dataReplace.Add("Mazowieckie", "*");

            this.dt = dt;
        }

        public int KAnonymize(string[] pid,int k)
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

            return dt.Rows.Count / distPIDs;
        }

        protected void KAnonymizationStep(string[] pid)
        {
            foreach (DataRow row in dt.Rows)
                foreach(string col in pid)
                    // change value with a higher one in tree
                    row[col] = dataReplace[row[col]];
        }
    }
}
