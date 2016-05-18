using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataAnonymization
{
    class DataToFromXML
    {
        public void SaveDataToXML(DataTable dataTable, string tableName)
        {
            dataTable.WriteXml(tableName + ".xml");
        }

        public DataTable GetDataFromXML(DataGrid dataGrid, string tableName)
        {
            DataTable dataTable = new DataTable(tableName);
            if (!File.Exists(tableName + ".xml")) return null;
            
            DataSet ds = new DataSet();
            ds.ReadXml(tableName + ".xml");
            dataTable = ds.Tables[0];
            dataGrid.DataContext = dataTable.DefaultView;
            dataGrid.ColumnWidth = 80;

            return dataTable;
        }
    }
}
