using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            string connectionString = GetConnectionString();
            // Open a connection to the database.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Create the SqlBulkCopy object. 
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    // Map columns
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }
                    bulkCopy.BulkCopyTimeout = 600;
                    bulkCopy.DestinationTableName = "dbo." + tableName;
                    try
                    {
                        // Write from the source to the destination.
                        bulkCopy.WriteToServer(dataTable);
                    }
                    catch (Exception ex)
                    {
                        System.IO.StreamWriter file = new System.IO.StreamWriter("LOG.txt");
                        file.WriteLine(ex.Message);
                        file.Close();
                    }
                }
            }
        }

        public DataTable GetDataFromDB(DataGrid dataGrid, string tableName)
        {
            DataTable dataTable = new DataTable(tableName);
            string connectionString = GetConnectionString();
            string query = "SELECT * FROM dbo.[" + tableName + "]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();
                dataTable = new DataTable();
                dataTable.Load(cmd.ExecuteReader());
            }
            dataGrid.DataContext = dataTable.DefaultView;
            dataGrid.ColumnWidth = 80;
            return dataTable;
        }

        private string GetConnectionString()
        {
            return @"Data Source=(localdb)\Projects;Initial Catalog=DataAnonymizationDB;Integrated Security=True;Pooling=False;Connect Timeout=30";
        }
    }
}
