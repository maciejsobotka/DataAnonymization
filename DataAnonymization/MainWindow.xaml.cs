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
        private DataTable dt2;
        public MainWindow()
        {
            InitializeComponent();
            InitializeDataGrid();
            InitializeDataGrid2();
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

        private void InitializeDataGrid2()
        {
            dt2 = new DataTable("dataTable2");
            if (File.Exists("dataTable2.xml"))
            {
                DataSet ds = new DataSet();
                ds.ReadXml("dataTable2.xml");
                dt2 = ds.Tables[0];
            }
            else
            {
                dt2.Columns.Add("Płeć");
                dt2.Columns.Add("Zawód");
                dt2.Columns.Add("Miasto");
                dt2.Columns.Add("Choroba");
                for (int i = 0; i < dt.Columns.Count; ++i)
                {
                    dt2.Columns[i].DefaultValue = String.Empty;
                }
            }
            tableGrid2.DataContext = dt2.DefaultView;
            tableGrid2.ColumnWidth = 80;
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

        //=====================================================================
        // k-Anonymization
        private void kAnonymization_Click(object sender, RoutedEventArgs e)
        {
            if (kValBox.Text == "")
                kValBox.Text = "4";
            if (pidValBox.Text == "")
                pidValBox.Text = "1,2,3";
            int k = 0;
            Int32.TryParse(kValBox.Text, out k);
            string[] pid = pidValBox.Text.Split(',');
            for (int i = 0; i < pid.Length; ++i )
                pid[i] = (dt.Columns[Int32.Parse(pid[i]) - 1].ColumnName);
            KAnonymization kA = new KAnonymization(dt);
            kValBox.Text = kA.KAnonymize(pid, k).ToString();
        }

        private void buttonToXML2_Click(object sender, RoutedEventArgs e)
        {
            dt2.WriteXml("dataTable2.xml");
        }

        private void buttonFromXML2_Click(object sender, RoutedEventArgs e)
        {
            dt2 = new DataTable("dataTable2");
            if (File.Exists("dataTable2.xml"))
            {
                DataSet ds = new DataSet();
                ds.ReadXml("dataTable2.xml");
                dt2 = ds.Tables[0];
            }
            tableGrid2.DataContext = dt2.DefaultView;
            tableGrid2.ColumnWidth = 80;
        }

        private void xyAnonymization_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
