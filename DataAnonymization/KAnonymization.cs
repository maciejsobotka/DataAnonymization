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
        private Hashtable dataReplace;
        private DataTable dt;
        public KAnonymization(DataTable dt){
            dataReplace = new Hashtable();
            dataReplace.Add("M", "*");
            dataReplace.Add("K", "*");
            dataReplace.Add("Inżynier", "Techniczny");
            dataReplace.Add("Techniczny", "*");
            dataReplace.Add("Malarz", "Artystyczny");
            dataReplace.Add("Tancerz", "Artystyczny");
            dataReplace.Add("Muzyk", "Artystyczny");
            dataReplace.Add("Artystyczny", "*");
            dataReplace.Add("Kraków", "Małopolskie");
            dataReplace.Add("Małopolskie", "*");
            dataReplace.Add("Opole", "Opolskie");
            dataReplace.Add("Brzeg", "Opolskie");
            dataReplace.Add("Opolskie", "*");

            this.dt = dt;
        }

        public void KAnonymize(int[] PIDColumns,int k)
        {

        }
    }
}
