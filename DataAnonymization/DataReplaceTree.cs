using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnonymization
{
    class DataReplaceTree
    {
        private Hashtable dataReplace;

        public DataReplaceTree()
        {
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
        }

        public object DataReplace(object data)
        {
            return dataReplace[data];
        }
    }
}
