using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataAnonymization
{
    class DataToFromDB
    {
        public void SaveDataToDB(DataTable dataTable, string tableName)
        {

        }

        public DataTable GetDataFromDB(DataGrid dataGrid, string tableName)
        {
            DataTable dataTable = new DataTable(tableName);

            return dataTable;
        }
    }
}
