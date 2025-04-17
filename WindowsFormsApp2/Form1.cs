using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string path = "";

        private void button1_Click(object sender, EventArgs e)
        {
            ExcelSqlExporterNpoi excelSqlExporter = new ExcelSqlExporterNpoi();
            excelSqlExporter.Execute(); 
        }
    }
}
