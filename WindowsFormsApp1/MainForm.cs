using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using DevExpress.Utils.Extensions;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Font = new Font("隶书", 15);
            dataGridView1.EnableHeadersVisualStyles = false;
            //然后设置样式
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", 12, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            // 生成 50 条数据
            Random rand = new Random();
            for (int i = 1; i <= 50; i++)
            {
                dataGridView1.Rows.Add(
                    "正常",                 // 预诊状态
                    rand.Next(100, 500),    // 房号
                    "已就诊",               // 病例状态
                    i % 2 == 0 ? "男" : "女", // 性别
                    $"患者{i}",             // 姓名
                    rand.Next(1, 100),      // 年龄
                    "138" + rand.Next(10000000, 99999999), // 电话
                    "感冒",                 // 诊断
                    "汉族",                 // 民族
                    rand.Next(1000, 9999),  // 卡号
                    "BL" + rand.Next(10000, 99999), // 病历号
                    "普通号"                 // 挂号类别
                );
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void roundedButton3_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }

        private void roundedButton5_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void roundedButton6_Click(object sender, EventArgs e)
        {
            TemplateSet temp = new TemplateSet();
            temp.ShowDialog();
        }
    }
}
