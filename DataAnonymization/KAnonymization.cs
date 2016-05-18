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
            if (k > dt.Rows.Count) k = dt.Rows.Count;
            DataView v = new DataView(dt);
            int smallGroup = 0;
            while (smallGroup < k)
            {
                DataTable allPIDs = v.ToTable(false, pid);  // all rows
                DataTable PIDs = v.ToTable(true, pid);      // distinct rows
                int[] groups = new int[PIDs.Rows.Count];
                for (int i = 0; i < PIDs.Rows.Count; ++i)
                    foreach (DataRow r in allPIDs.Rows)     // count rows in groups
                        if (r.ItemArray.SequenceEqual(PIDs.Rows[i].ItemArray))
                            groups[i]++;
                smallGroup = groups.Min();
                if (smallGroup < k) KAnonymizationStep(pid);
            }
            return smallGroup;
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
                row[indx] = dataReplace[row[indx]];
        }
    }
}
