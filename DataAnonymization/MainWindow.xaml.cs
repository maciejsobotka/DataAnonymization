﻿using System;
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
        private DataTable dt3;
        public MainWindow()
        {
            InitializeComponent();
            dt = InitializeDataGrid(tableGrid, "dataTable");
            dt2 = InitializeDataGrid(tableGrid2, "dataTable2");
            dt3 = InitializeDataGrid(tableGrid3, "dataTable3");
        }

        //=====================================================================
        // Data Init
        private DataTable InitializeDataGrid(DataGrid dataGrid, string tableName)
        {
            DataTable dataTable = new DataTable(tableName);
            if (File.Exists(tableName + ".xml"))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(tableName + ".xml");
                dataTable = ds.Tables[0];
            }
            else
            {
                dataTable.Columns.Add("Płeć");
                dataTable.Columns.Add("Zawód");
                dataTable.Columns.Add("Miasto");
                dataTable.Columns.Add("Choroba");
                for (int i = 0; i < dataTable.Columns.Count; ++i)
                {
                    dataTable.Columns[i].DefaultValue = String.Empty;
                }
            }
            dataGrid.DataContext = dataTable.DefaultView;
            dataGrid.ColumnWidth = 80;

            return dataTable;
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
            int realK = kA.KAnonymize(pid, k);
            if (realK < Int32.Parse(kValBox.Text))
                kValBox.Text = realK.ToString();
        }

        private void xyAnonymization_Click(object sender, RoutedEventArgs e)
        {
            if (kValBox2.Text == "")
                kValBox2.Text = "3";
            if (xValBox2.Text == "")
                xValBox2.Text = "1,2";
            if (yValBox2.Text == "")
                yValBox2.Text = "3";
            int k = 0;
            Int32.TryParse(kValBox2.Text, out k);
            string[] x = xValBox2.Text.Split(',');
            string[] y = yValBox2.Text.Split(',');
            for (int i = 0; i < x.Length; ++i)
                x[i] = (dt2.Columns[Int32.Parse(x[i]) - 1].ColumnName);
            for (int i = 0; i < y.Length; ++i)
                y[i] = (dt2.Columns[Int32.Parse(y[i]) - 1].ColumnName);
            XYAnonymization xyA = new XYAnonymization(dt2);
            int realK = xyA.xyAnonymize(x, y, k);
            if (realK < Int32.Parse(kValBox2.Text))
                kValBox2.Text = realK.ToString();
        }

        private void akAnonymization_Click(object sender, RoutedEventArgs e)
        {

        }

        //=====================================================================
        // XML Serialization
        private void SaveDataToXML(DataTable dataTable, string tableName)
        {
            dataTable.WriteXml(tableName + ".xml");
        }

        private DataTable GetDataFromXML(DataGrid dataGrid, string tableName)
        {
            DataTable dataTable = new DataTable(tableName);
            if (File.Exists(tableName + ".xml"))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(tableName + ".xml");
                dataTable = ds.Tables[0];
            }
            dataGrid.DataContext = dataTable.DefaultView;
            dataGrid.ColumnWidth = 80;

            return dataTable;
        }

        private void buttonToXML_Click(object sender, RoutedEventArgs e)
        {
            SaveDataToXML(dt, "dataTable");
        }

        private void buttonFromXML_Click(object sender, RoutedEventArgs e)
        {
            dt = GetDataFromXML(tableGrid, "dataTable");
        }

        private void buttonToXML2_Click(object sender, RoutedEventArgs e)
        {
            SaveDataToXML(dt2, "dataTable2");
        }

        private void buttonFromXML2_Click(object sender, RoutedEventArgs e)
        {
            dt2 = GetDataFromXML(tableGrid2, "dataTable2");
        }

        private void buttonToXML3_Click(object sender, RoutedEventArgs e)
        {
            SaveDataToXML(dt3, "dataTable3");
        }

        private void buttonFromXML3_Click(object sender, RoutedEventArgs e)
        {
            dt3 = GetDataFromXML(tableGrid3, "dataTable3");
        }
    }
}
