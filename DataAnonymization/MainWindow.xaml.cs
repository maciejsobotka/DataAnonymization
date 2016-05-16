using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataAnonymization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataTable dt;
        public MainWindow()
        {
            InitializeComponent();
            InitializeDataGrid();
        }

        //=====================================================================
        // Data Init
        private void InitializeDataGrid()
        {
            dt = new DataTable("dataTable");
            if (File.Exists("dataTable.xml"))
            {
                DataSet ds = new DataSet();
                ds.ReadXml("dataTable.xml");
                dt = ds.Tables[0];
            }
            else
            {
                dt.Columns.Add("Płeć");
                dt.Columns.Add("Zawód");
                dt.Columns.Add("Miasto");
                dt.Columns.Add("Choroba");
                for (int i = 0; i < dt.Columns.Count; ++i)
                {
                    dt.Columns[i].DefaultValue = String.Empty;
                }
            }
            tableGrid.DataContext = dt.DefaultView;
            tableGrid.ColumnWidth = 80;
        }

        //=====================================================================
        // XML Serialization
        private void buttonToXML_Click(object sender, RoutedEventArgs e)
        {
            dt.WriteXml("dataTable.xml");
        }

        private void buttonFromXML_Click(object sender, RoutedEventArgs e)
        {
            dt = new DataTable("dataTable");
            if (File.Exists("dataTable.xml"))
            {
                DataSet ds = new DataSet();
                ds.ReadXml("dataTable.xml");
                dt = ds.Tables[0];
            }
            tableGrid.DataContext = dt.DefaultView;
            tableGrid.ColumnWidth = 80;
        }
    }
}
